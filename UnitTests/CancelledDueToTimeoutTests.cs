using System;
using System.Threading.Tasks;
using Xunit;
using static UnitTests.Helpers;

namespace UnitTests
{
    public static class CancelledDueToTimeoutTests
    {
        [Fact]
        public static async Task TwoTasksOfStringAndIntWhereNeitherCompletes()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(500));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(500));
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await task1.WaitForWith(task2, timeout: TimeSpan.FromMilliseconds(1)));
            Assert.False(task1.IsCompleted);
            Assert.False(task2.IsCompleted);
        }

        [Fact]
        public static async Task TwoTasksOfStringAndIntWhereOnlyOneCompletes()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(1));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(5_000));
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await task1.WaitForWith(task2, timeout: TimeSpan.FromMilliseconds(100)));
            Assert.True(task1.IsCompleted);
            Assert.False(task2.IsCompleted);
        }
    }
}