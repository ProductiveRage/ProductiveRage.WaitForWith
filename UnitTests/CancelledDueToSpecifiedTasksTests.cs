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
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(1));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(500), throwOperationCanceledExceptionAfterDelay: true);
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await task1.WaitForWith(task2));
            Assert.True(task1.IsCompleted);
            Assert.True(task2.IsCanceled);
        }

        [Fact]
        public static async Task TwoTasksOfStringAndIntWhereOneGetsCancelledBeforeCancellationTokenIsSet()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(1));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(500), throwOperationCanceledExceptionAfterDelay: true);
            using var cancellationTokenSource = new CancellationTokenSource();
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await task1.WaitForWith(task2, cancellationTokenSource.Token));
            Assert.True(task1.IsCompleted);
            Assert.True(task2.IsCanceled);
        }

        [Fact]
        public static async Task TwoTasksOfStringAndIntWhereOneGetsCancelledBeforeTimeoutIsHit()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(1));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(500), throwOperationCanceledExceptionAfterDelay: true);
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await task1.WaitForWith(task2, timeout: TimeSpan.FromSeconds(1)));
            Assert.True(task1.IsCompleted);
            Assert.True(task2.IsCanceled);
        }
    }
}