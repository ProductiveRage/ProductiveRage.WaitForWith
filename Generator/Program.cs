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
            var typeParams = string.Join(", ", Enumerable.Range(1, 1 + numberOfOtherTasks).Select(i => "T" + i));
            var othersParameterTypeNames = Enumerable.Range(1, numberOfOtherTasks).Select(i => $"Task<T{i + 1}>");
            var othersParameterNames = (numberOfOtherTasks == 1)
                ? new[] { "other" }
                : Enumerable.Range(1, numberOfOtherTasks).Select(i => $"other{i}");

            var allTaskNames = othersParameterNames.Prepend("source");
            var coreCombinedTask = "Task.WhenAll(" + string.Join(", ", allTaskNames) + ")";
            var returnAllResults = "return (" + string.Join(", ", allTaskNames.Select(name => name + ".Result")) + ");";

            // Method with CancellationToken option (will has a default value if not required - can't have this default AND the timeout one with a default otherwise the compiler would struggle with ambiguous overload matching)
            var cancellationTokenName = "cancellationToken";
            foreach (var line in GenerateMethodHeader("CancellationToken " + cancellationTokenName + " = default"))
                yield return line;
            yield return "";
            yield return initialIndentation + individualIndentation + "await WaitWithPossibilityOfCancellation(" + coreCombinedTask + ", " + cancellationTokenName + ").ConfigureAwait(false);";
            yield return initialIndentation + individualIndentation + returnAllResults;
            yield return initialIndentation + "}";
            yield return "";

            // Method with timeout option
            var timeoutParameterName = "timeout";
            foreach (var line in GenerateMethodHeader("TimeSpan " + timeoutParameterName))
                yield return line;
            yield return initialIndentation + individualIndentation + "ThrowForNegativeTimeout(" + timeoutParameterName + ", nameof(" + timeoutParameterName + "));";
            yield return "";
            yield return initialIndentation + individualIndentation + "await WaitWithPossibilityOfCancellation(" + coreCombinedTask + ", Task.Delay(" + timeoutParameterName + ")).ConfigureAwait(false);";
            yield return initialIndentation + individualIndentation + returnAllResults;
            yield return initialIndentation + "}";

            IEnumerable<string> GenerateMethodHeader(string optionalAdditionalArguments = null)
            {
                yield return string.Format(
                    "{0}public static async Task<({1})> WaitForWith<{1}>(this Task<T1> source, {2}{3})",
                    initialIndentation,
                    typeParams,
                    string.Join(", ", othersParameterTypeNames.Zip(othersParameterNames, (typeName, name) => typeName + " " + name)),
                    string.IsNullOrWhiteSpace(optionalAdditionalArguments) ? "" : (", " + optionalAdditionalArguments)
                );
                yield return initialIndentation + "{";
                foreach (var name in allTaskNames)
                    yield return initialIndentation + individualIndentation + "ThrowForNullTask(" + name + ", nameof(" + name + "));";
            }
        }
    }
}