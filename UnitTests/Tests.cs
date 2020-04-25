using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public static class Tests
    {
        [Fact]
        public static async Task TwoTasksOfStringAndIntReturnValues()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(1));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(1));
            var (result1, result2) = await task1.WaitForWith(task2);
            Assert.Equal("abc", result1);
            Assert.Equal(123, result2);
        }

        [Fact]
        public static async Task TwoTasksOfStringAndIntWhereOneGetsCancelled()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(500));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(500), throwOperationCanceledExceptionAfterDelay: true);
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(1));
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await task1.WaitForWith(task2, cancellationTokenSource.Token));
        }

        [Fact]
        public static async Task TwoTasksWithCancellationTokenThatIsNotSetBeforeTasksComplete()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(500));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(500));
            var (result1, result2) = await task1.WaitForWith(task2);
            Assert.Equal("abc", result1);
            Assert.Equal(123, result2);
        }

        [Fact]
        public static async Task TwoTasksWithCancellationTokenThatIsSetBeforeTasksComplete()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(500));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(500));
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(1));
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await task1.WaitForWith(task2, cancellationTokenSource.Token));
        }

        [Fact]
        public static async Task TwoTasksWithTimeoutThatIsNotReachedBeforeTasksComplete()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(500));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(500));
            var (result1, result2) = await task1.WaitForWith(task2, timeout: TimeSpan.FromMilliseconds(1_000));
            Assert.Equal("abc", result1);
            Assert.Equal(123, result2);
        }

        [Fact]
        public static async Task TwoTasksWithTimeoutThatIsReachedBeforeTasksComplete()
        {
            var task1 = GetValueWithDelay("abc", TimeSpan.FromMilliseconds(500));
            var task2 = GetValueWithDelay(123, TimeSpan.FromMilliseconds(500));
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(1));
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await task1.WaitForWith(task2, timeout: TimeSpan.FromMilliseconds(1)));
        }

        private static async Task<T> GetValueWithDelay<T>(T value, TimeSpan delay, bool throwOperationCanceledExceptionAfterDelay = false)
        {
            await Task.Delay(delay);
            if (throwOperationCanceledExceptionAfterDelay)
                throw new OperationCanceledException();
            return value;
        }
    }
}