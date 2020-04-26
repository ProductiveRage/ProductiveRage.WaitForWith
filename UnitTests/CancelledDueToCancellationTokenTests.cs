using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static UnitTests.Helpers;

namespace UnitTests
{
    public static class CancelledDueToCancellationTokenTests
    {
        [Fact]
        public static async Task TwoTasksOfStringAndIntWhereNeitherCompletes()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(500));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(500));
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(1));
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await task1.WaitForWith(task2, cancellationTokenSource.Token));
            Assert.False(task1.IsCompleted);
            Assert.False(task2.IsCompleted);
        }
        
        [Fact]
        public static async Task TwoTasksOfStringAndIntWhereOnlyOneCompletes()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(1));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(500));
            using var cancellationTokenSource = new CancellationTokenSource();
            _ = task1.ContinueWith(_ => cancellationTokenSource.Cancel());
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await task1.WaitForWith(task2, cancellationTokenSource.Token));
            Assert.True(task1.IsCompleted);
            Assert.False(task2.IsCompleted);
        }
    }
}