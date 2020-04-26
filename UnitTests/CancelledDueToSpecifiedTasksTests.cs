using System;
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
    }
}