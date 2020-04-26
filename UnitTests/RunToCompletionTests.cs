using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static UnitTests.Helpers;

namespace UnitTests
{
    public static class RunToCompletionTests
    {
        [Fact]
        public static async Task TwoTasksOfStringAndInt()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(1));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(1));
            var (result1, result2) = await task1.WaitForWith(task2);
            Assert.Equal("abc", result1);
            Assert.Equal(123, result2);
        }

        [Fact]
        public static async Task TwoTasksOfStringAndIntWithCancellationToken()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(500));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(500));
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(1_000));
            var (result1, result2) = await task1.WaitForWith(task2, cancellationTokenSource.Token);
            Assert.Equal("abc", result1);
            Assert.Equal(123, result2);
        }

        [Fact]
        public static async Task TwoTasksOfStringAndIntWithTimeout()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(500));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(500));
            var (result1, result2) = await task1.WaitForWith(task2, timeout: TimeSpan.FromMilliseconds(1_000));
            Assert.Equal("abc", result1);
            Assert.Equal(123, result2);
        }
    }
}