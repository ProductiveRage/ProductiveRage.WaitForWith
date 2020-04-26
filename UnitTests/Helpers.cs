using System;
using System.Threading.Tasks;

namespace UnitTests
{
    internal static class Helpers
    {
        public static async Task<T> GetValueWithDelay<T>(T value, TimeSpan delay, bool throwOperationCanceledExceptionAfterDelay = false)
        {
            await Task.Delay(delay);
            if (throwOperationCanceledExceptionAfterDelay)
                throw new OperationCanceledException();
            return value;
        }
    }
}