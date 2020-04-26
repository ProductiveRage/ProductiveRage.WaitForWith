using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Generator
{
    // TODO: Note in docs lack of "Async" suffix(?) - https://docs.particular.net/nservicebus/upgrades/5to6/async-suffix
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            const string outputFilename = "ProductiveRageTaskExtensions.cs";

            var outputFolderName = args?.FirstOrDefault(arg => !string.IsNullOrWhiteSpace(arg));
            if (outputFolderName is null)
            {
                Console.WriteLine($"Must specify the output folder to write {outputFilename} to as a command line argument");
                Environment.Exit(1);
                return;
            }
            var outputFolder = new DirectoryInfo(outputFolderName);
            if (!outputFolder.Exists)
            {
                Console.WriteLine($"The folder to write {outputFilename} to must already exist: " + outputFolderName);
                Environment.Exit(1);
                return;
            }

            var templateContent = await File.ReadAllLinesAsync("ProductiveRageTaskExtensions.template");
            var firstLineWithIndentation = templateContent.First(line => (line.Length != 0) && char.IsWhiteSpace(line[0]));

            var completedContent = string.Join(
                Environment.NewLine,
                templateContent.SelectMany(line =>
                    line.Trim() == "# Start Here"
                        ? GenerateMethods(initialIndentation: GetIndentationFromLine(line), individualIndentation: GetIndentationFromLine(firstLineWithIndentation))
                        : new[] { line }
                )
            );

            var outputFilePath = Path.Combine(outputFolderName, outputFilename);
            File.WriteAllText(outputFilePath, completedContent);

            static string GetIndentationFromLine(string line) => new string(line.TakeWhile(char.IsWhiteSpace).ToArray());
        }

        private static IEnumerable<string> GenerateMethods(string initialIndentation, string individualIndentation)
        {
            var haveWrittenAnything = false;
            for (var numberOfOtherTasks = 1; numberOfOtherTasks <= 9; numberOfOtherTasks++)
            {
                if (haveWrittenAnything)
                    yield return "";
                var lines = GenerateMethods(initialIndentation, individualIndentation, numberOfOtherTasks);
                foreach (var line in lines)
                    yield return line;
                haveWrittenAnything = true;
            }
        }

        private static IEnumerable<string> GenerateMethods(string initialIndentation, string individualIndentation, int numberOfOtherTasks)
        {
            var allTypeParamNames = Enumerable.Range(1, 1 + numberOfOtherTasks).Select(i => "T" + i);
            var othersParameterTypes = allTypeParamNames.Skip(1).Select(name => $"Task<{name}>");
            var othersParameterNames = (numberOfOtherTasks == 1)
                ? new[] { "other" }
                : Enumerable.Range(1, numberOfOtherTasks).Select(i => $"other{i}");

            var allTaskNames = othersParameterNames.Prepend("source");
            var coreCombinedTask = "Task.WhenAll(" + string.Join(", ", allTaskNames) + ")";
            var returnAllResults = "return (" + string.Join(", ", allTaskNames.Select(name => name + ".Result")) + ");";

            // Method with CancellationToken option (will has a default value if not required - can't have this default AND the timeout one with a default otherwise the compiler would struggle with ambiguous overload matching)
            // TODO: Need summary comments (no fancy business like automatically adding "/// <summary>" cos want full control over what include or don't - summary, paraeter, return value, etc..)
            foreach (var line in GenerateSummaryCommentForCancellationTokenOverride())
                yield return line;
            var cancellationTokenName = "cancellationToken";
            foreach (var line in GenerateMethodHeader("CancellationToken " + cancellationTokenName + " = default"))
                yield return line;
            yield return "";
            yield return initialIndentation + individualIndentation + "await WaitWithPossibilityOfCancellation(" + coreCombinedTask + ", " + cancellationTokenName + ").ConfigureAwait(false);";
            foreach (var line in GenerateCheckForIndividualTaskCancellations())
                yield return line;
            yield return initialIndentation + individualIndentation + returnAllResults;
            yield return initialIndentation + "}";
            yield return "";

            // Method with timeout option
            // TODO: Need summary comments (no fancy business like automatically adding "/// <summary>" cos want full control over what include or don't - summary, paraeter, return value, etc..)
            var timeoutParameterName = "timeout";
            foreach (var line in GenerateMethodHeader("TimeSpan " + timeoutParameterName))
                yield return line;
            yield return initialIndentation + individualIndentation + "ThrowForNegativeTimeout(" + timeoutParameterName + ", nameof(" + timeoutParameterName + "));";
            yield return "";
            yield return initialIndentation + individualIndentation + "await WaitWithPossibilityOfCancellation(" + coreCombinedTask + ", Task.Delay(" + timeoutParameterName + ")).ConfigureAwait(false);";
            foreach (var line in GenerateCheckForIndividualTaskCancellations())
                yield return line;
            yield return initialIndentation + individualIndentation + returnAllResults;
            yield return initialIndentation + "}";

            IEnumerable<string> GenerateSummaryCommentForCancellationTokenOverride()
            {
                var summaryCommentLines = ApplyIndentationToSummaryCommentLines(@"
                    /// <summary>
                    /// Wait for multiple Tasks that return a value to complete and returns each of their values as a Tuple, in the order in which they were provided (if called as an extension method then the result from the first 'source' Task will be returned first followed
                    /// the the other results). This method optionally takes a CancellationToken - if this is set before the Tasks complete then an OperationCanceledException will be raised. Ideally, this parameter would never be given a value and, if cancellation is required
                    /// then the CancellationToken would have been shared between whatever provided each of the Tasks originally but sometimes this is not feasible and the optional argument here may be convenient (note that setting it will cancel the Task returned by this
                    /// method but it can not cancel each of the provided Tasks - they will run to completion, even though this method will no longer be waiting for their results). If any Task is cancelled while another Task is still running then an OperationCanceledException
                    /// will be raised.
                    /// </summary>"
                );
                foreach (var line in summaryCommentLines)
                    yield return line;

                foreach (var (typeParamName, parameterName) in allTypeParamNames.Zip(allTaskNames, (typeParamName, parameterName) => (typeParamName, parameterName)))
                    yield return initialIndentation + $"/// <typeparam name=\"{typeParamName}\">The result type of the '{parameterName}' Task</typeparam>";

                foreach (var (parameterName, index) in allTaskNames.Select((parameterName, index) => (parameterName, index)))
                    yield return initialIndentation + $"/// <param name=\"{parameterName}\">The {GetParamaterDescription(index)} Task to wait for</param>";

                var cancellationTokenParamLines = ApplyIndentationToSummaryCommentLines(@"
                    /// <param name=""cancellationToken"">An optional CancellationToken that will cancel the Task returned by this method (but which can not cancel the individual Tasks provided - they will continue to run to completion unless the same CancellationToken was used
                    /// in the initialisation of each of those, in which case it is redundant to provide it here</param>"
                );
                foreach (var line in cancellationTokenParamLines)
                    yield return line;

                yield return initialIndentation + "/// <returns>A Tuple containing the results of the Tasks, in the order in which they were specified - so long as they all completed successfully (and, if the cancellationToken parameter was given a value, before that was set)</returns>";
            }

            IEnumerable<string> ApplyIndentationToSummaryCommentLines(string content) => content.Split(Environment.NewLine).Select(line => line.Trim()).SkipWhile(line => line.Length == 0).Select(line => initialIndentation + line);

            IEnumerable<string> GenerateMethodHeader(string optionalAdditionalArguments = null)
            {
                yield return string.Format(
                    "{0}public static async Task<({1})> WaitForWith<{1}>(this Task<T1> source, {2}{3})",
                    initialIndentation,
                    string.Join(", ", allTypeParamNames),
                    string.Join(", ", othersParameterTypes.Zip(othersParameterNames, (typeName, name) => typeName + " " + name)),
                    string.IsNullOrWhiteSpace(optionalAdditionalArguments) ? "" : (", " + optionalAdditionalArguments)
                );
                yield return initialIndentation + "{";
                foreach (var name in allTaskNames)
                    yield return initialIndentation + individualIndentation + "ThrowForNullTask(" + name + ", nameof(" + name + "));";
            }

            IEnumerable<string> GenerateCheckForIndividualTaskCancellations()
            {
                yield return initialIndentation + individualIndentation + "if (" + string.Join(" || ", allTaskNames.Select(name => name + ".IsCanceled")) + ")";
                yield return initialIndentation + individualIndentation + individualIndentation + "throw new OperationCanceledException();";
            }
        }

        private static IReadOnlyList<string> _availableDescriptions = new[] { "first", "second", "third", "fourth", "fifth", "sixth", "seventh", "eigth", "ninth", "tenth" }.ToList().AsReadOnly();
        static string GetParamaterDescription(int index)
        {
            if ((index < 0) || (index >= _availableDescriptions.Count))
                throw new ArgumentOutOfRangeException(nameof(index), index, "GetParamaterDescription can not provide an answer for this value");
            return _availableDescriptions[index];
        }
    }
}