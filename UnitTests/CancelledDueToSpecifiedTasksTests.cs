using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static UnitTests.Helpers;

namespace UnitTests
{
    public static class CancelledDueToSpecifiedTasksTests
    {
        [Fact]
        public static async Task TwoTasksOfStringAndIntWhereOneGetsCancelled()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(500));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(500), throwOperationCanceledExceptionAfterDelay: true);
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(1));
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await task1.WaitForWith(task2, cancellationTokenSource.Token));
            Assert.False(task1.IsCompleted);
            Assert.False(task2.IsCompleted);
        }
    }
}