using System.Diagnostics;

namespace System.Threading.Tasks
{
    public static class ProductiveRageTaskExtensions
    {
        public static async Task<(T1, T2)> WaitForWith<T1, T2>(this Task<T1> source, Task<T2> other, CancellationToken cancellationToken = default)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other, nameof(other));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other), cancellationToken).ConfigureAwait(false);
            if (source.IsCanceled || other.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other.Result);
        }

        public static async Task<(T1, T2)> WaitForWith<T1, T2>(this Task<T1> source, Task<T2> other, TimeSpan timeout)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other, nameof(other));
            ThrowForNegativeTimeout(timeout, nameof(timeout));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other), Task.Delay(timeout)).ConfigureAwait(false);
            if (source.IsCanceled || other.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other.Result);
        }

        public static async Task<(T1, T2, T3)> WaitForWith<T1, T2, T3>(this Task<T1> source, Task<T2> other1, Task<T3> other2, CancellationToken cancellationToken = default)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2), cancellationToken).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result);
        }

        public static async Task<(T1, T2, T3)> WaitForWith<T1, T2, T3>(this Task<T1> source, Task<T2> other1, Task<T3> other2, TimeSpan timeout)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));
            ThrowForNegativeTimeout(timeout, nameof(timeout));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2), Task.Delay(timeout)).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result);
        }

        public static async Task<(T1, T2, T3, T4)> WaitForWith<T1, T2, T3, T4>(this Task<T1> source, Task<T2> other1, Task<T3> other2, Task<T4> other3, CancellationToken cancellationToken = default)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));
            ThrowForNullTask(other3, nameof(other3));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2, other3), cancellationToken).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled || other3.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result, other3.Result);
        }

        public static async Task<(T1, T2, T3, T4)> WaitForWith<T1, T2, T3, T4>(this Task<T1> source, Task<T2> other1, Task<T3> other2, Task<T4> other3, TimeSpan timeout)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));
            ThrowForNullTask(other3, nameof(other3));
            ThrowForNegativeTimeout(timeout, nameof(timeout));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2, other3), Task.Delay(timeout)).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled || other3.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result, other3.Result);
        }

        public static async Task<(T1, T2, T3, T4, T5)> WaitForWith<T1, T2, T3, T4, T5>(this Task<T1> source, Task<T2> other1, Task<T3> other2, Task<T4> other3, Task<T5> other4, CancellationToken cancellationToken = default)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));
            ThrowForNullTask(other3, nameof(other3));
            ThrowForNullTask(other4, nameof(other4));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2, other3, other4), cancellationToken).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled || other3.IsCanceled || other4.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result, other3.Result, other4.Result);
        }

        public static async Task<(T1, T2, T3, T4, T5)> WaitForWith<T1, T2, T3, T4, T5>(this Task<T1> source, Task<T2> other1, Task<T3> other2, Task<T4> other3, Task<T5> other4, TimeSpan timeout)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));
            ThrowForNullTask(other3, nameof(other3));
            ThrowForNullTask(other4, nameof(other4));
            ThrowForNegativeTimeout(timeout, nameof(timeout));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2, other3, other4), Task.Delay(timeout)).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled || other3.IsCanceled || other4.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result, other3.Result, other4.Result);
        }

        public static async Task<(T1, T2, T3, T4, T5, T6)> WaitForWith<T1, T2, T3, T4, T5, T6>(this Task<T1> source, Task<T2> other1, Task<T3> other2, Task<T4> other3, Task<T5> other4, Task<T6> other5, CancellationToken cancellationToken = default)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));
            ThrowForNullTask(other3, nameof(other3));
            ThrowForNullTask(other4, nameof(other4));
            ThrowForNullTask(other5, nameof(other5));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2, other3, other4, other5), cancellationToken).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled || other3.IsCanceled || other4.IsCanceled || other5.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result, other3.Result, other4.Result, other5.Result);
        }

        public static async Task<(T1, T2, T3, T4, T5, T6)> WaitForWith<T1, T2, T3, T4, T5, T6>(this Task<T1> source, Task<T2> other1, Task<T3> other2, Task<T4> other3, Task<T5> other4, Task<T6> other5, TimeSpan timeout)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));
            ThrowForNullTask(other3, nameof(other3));
            ThrowForNullTask(other4, nameof(other4));
            ThrowForNullTask(other5, nameof(other5));
            ThrowForNegativeTimeout(timeout, nameof(timeout));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2, other3, other4, other5), Task.Delay(timeout)).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled || other3.IsCanceled || other4.IsCanceled || other5.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result, other3.Result, other4.Result, other5.Result);
        }

        public static async Task<(T1, T2, T3, T4, T5, T6, T7)> WaitForWith<T1, T2, T3, T4, T5, T6, T7>(this Task<T1> source, Task<T2> other1, Task<T3> other2, Task<T4> other3, Task<T5> other4, Task<T6> other5, Task<T7> other6, CancellationToken cancellationToken = default)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));
            ThrowForNullTask(other3, nameof(other3));
            ThrowForNullTask(other4, nameof(other4));
            ThrowForNullTask(other5, nameof(other5));
            ThrowForNullTask(other6, nameof(other6));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2, other3, other4, other5, other6), cancellationToken).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled || other3.IsCanceled || other4.IsCanceled || other5.IsCanceled || other6.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result, other3.Result, other4.Result, other5.Result, other6.Result);
        }

        public static async Task<(T1, T2, T3, T4, T5, T6, T7)> WaitForWith<T1, T2, T3, T4, T5, T6, T7>(this Task<T1> source, Task<T2> other1, Task<T3> other2, Task<T4> other3, Task<T5> other4, Task<T6> other5, Task<T7> other6, TimeSpan timeout)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));
            ThrowForNullTask(other3, nameof(other3));
            ThrowForNullTask(other4, nameof(other4));
            ThrowForNullTask(other5, nameof(other5));
            ThrowForNullTask(other6, nameof(other6));
            ThrowForNegativeTimeout(timeout, nameof(timeout));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2, other3, other4, other5, other6), Task.Delay(timeout)).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled || other3.IsCanceled || other4.IsCanceled || other5.IsCanceled || other6.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result, other3.Result, other4.Result, other5.Result, other6.Result);
        }

        public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8)> WaitForWith<T1, T2, T3, T4, T5, T6, T7, T8>(this Task<T1> source, Task<T2> other1, Task<T3> other2, Task<T4> other3, Task<T5> other4, Task<T6> other5, Task<T7> other6, Task<T8> other7, CancellationToken cancellationToken = default)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));
            ThrowForNullTask(other3, nameof(other3));
            ThrowForNullTask(other4, nameof(other4));
            ThrowForNullTask(other5, nameof(other5));
            ThrowForNullTask(other6, nameof(other6));
            ThrowForNullTask(other7, nameof(other7));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2, other3, other4, other5, other6, other7), cancellationToken).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled || other3.IsCanceled || other4.IsCanceled || other5.IsCanceled || other6.IsCanceled || other7.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result, other3.Result, other4.Result, other5.Result, other6.Result, other7.Result);
        }

        public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8)> WaitForWith<T1, T2, T3, T4, T5, T6, T7, T8>(this Task<T1> source, Task<T2> other1, Task<T3> other2, Task<T4> other3, Task<T5> other4, Task<T6> other5, Task<T7> other6, Task<T8> other7, TimeSpan timeout)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));
            ThrowForNullTask(other3, nameof(other3));
            ThrowForNullTask(other4, nameof(other4));
            ThrowForNullTask(other5, nameof(other5));
            ThrowForNullTask(other6, nameof(other6));
            ThrowForNullTask(other7, nameof(other7));
            ThrowForNegativeTimeout(timeout, nameof(timeout));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2, other3, other4, other5, other6, other7), Task.Delay(timeout)).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled || other3.IsCanceled || other4.IsCanceled || other5.IsCanceled || other6.IsCanceled || other7.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result, other3.Result, other4.Result, other5.Result, other6.Result, other7.Result);
        }

        public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> WaitForWith<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Task<T1> source, Task<T2> other1, Task<T3> other2, Task<T4> other3, Task<T5> other4, Task<T6> other5, Task<T7> other6, Task<T8> other7, Task<T9> other8, CancellationToken cancellationToken = default)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));
            ThrowForNullTask(other3, nameof(other3));
            ThrowForNullTask(other4, nameof(other4));
            ThrowForNullTask(other5, nameof(other5));
            ThrowForNullTask(other6, nameof(other6));
            ThrowForNullTask(other7, nameof(other7));
            ThrowForNullTask(other8, nameof(other8));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2, other3, other4, other5, other6, other7, other8), cancellationToken).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled || other3.IsCanceled || other4.IsCanceled || other5.IsCanceled || other6.IsCanceled || other7.IsCanceled || other8.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result, other3.Result, other4.Result, other5.Result, other6.Result, other7.Result, other8.Result);
        }

        public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> WaitForWith<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Task<T1> source, Task<T2> other1, Task<T3> other2, Task<T4> other3, Task<T5> other4, Task<T6> other5, Task<T7> other6, Task<T8> other7, Task<T9> other8, TimeSpan timeout)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));
            ThrowForNullTask(other3, nameof(other3));
            ThrowForNullTask(other4, nameof(other4));
            ThrowForNullTask(other5, nameof(other5));
            ThrowForNullTask(other6, nameof(other6));
            ThrowForNullTask(other7, nameof(other7));
            ThrowForNullTask(other8, nameof(other8));
            ThrowForNegativeTimeout(timeout, nameof(timeout));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2, other3, other4, other5, other6, other7, other8), Task.Delay(timeout)).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled || other3.IsCanceled || other4.IsCanceled || other5.IsCanceled || other6.IsCanceled || other7.IsCanceled || other8.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result, other3.Result, other4.Result, other5.Result, other6.Result, other7.Result, other8.Result);
        }

        public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> WaitForWith<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Task<T1> source, Task<T2> other1, Task<T3> other2, Task<T4> other3, Task<T5> other4, Task<T6> other5, Task<T7> other6, Task<T8> other7, Task<T9> other8, Task<T10> other9, CancellationToken cancellationToken = default)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));
            ThrowForNullTask(other3, nameof(other3));
            ThrowForNullTask(other4, nameof(other4));
            ThrowForNullTask(other5, nameof(other5));
            ThrowForNullTask(other6, nameof(other6));
            ThrowForNullTask(other7, nameof(other7));
            ThrowForNullTask(other8, nameof(other8));
            ThrowForNullTask(other9, nameof(other9));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2, other3, other4, other5, other6, other7, other8, other9), cancellationToken).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled || other3.IsCanceled || other4.IsCanceled || other5.IsCanceled || other6.IsCanceled || other7.IsCanceled || other8.IsCanceled || other9.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result, other3.Result, other4.Result, other5.Result, other6.Result, other7.Result, other8.Result, other9.Result);
        }

        public static async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> WaitForWith<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Task<T1> source, Task<T2> other1, Task<T3> other2, Task<T4> other3, Task<T5> other4, Task<T6> other5, Task<T7> other6, Task<T8> other7, Task<T9> other8, Task<T10> other9, TimeSpan timeout)
        {
            ThrowForNullTask(source, nameof(source));
            ThrowForNullTask(other1, nameof(other1));
            ThrowForNullTask(other2, nameof(other2));
            ThrowForNullTask(other3, nameof(other3));
            ThrowForNullTask(other4, nameof(other4));
            ThrowForNullTask(other5, nameof(other5));
            ThrowForNullTask(other6, nameof(other6));
            ThrowForNullTask(other7, nameof(other7));
            ThrowForNullTask(other8, nameof(other8));
            ThrowForNullTask(other9, nameof(other9));
            ThrowForNegativeTimeout(timeout, nameof(timeout));

            await WaitWithPossibilityOfCancellation(Task.WhenAll(source, other1, other2, other3, other4, other5, other6, other7, other8, other9), Task.Delay(timeout)).ConfigureAwait(false);
            if (source.IsCanceled || other1.IsCanceled || other2.IsCanceled || other3.IsCanceled || other4.IsCanceled || other5.IsCanceled || other6.IsCanceled || other7.IsCanceled || other8.IsCanceled || other9.IsCanceled)
                throw new OperationCanceledException();
            return (source.Result, other1.Result, other2.Result, other3.Result, other4.Result, other5.Result, other6.Result, other7.Result, other8.Result, other9.Result);
        }

        private static Task WaitWithPossibilityOfCancellation(Task work, CancellationToken cancellationToken)
        {
            ThrowForNullTask(work, nameof(work));

            return cancellationToken.CanBeCanceled
                ? WaitWithPossibilityOfCancellation(work, cancellationToken.AsTask())
                : work;
        }

        private static async Task WaitWithPossibilityOfCancellation(Task work, Task cancellerIfAny)
        {
            ThrowForNullTask(work, nameof(work));

            if (cancellerIfAny is null)
            {
                await work.ConfigureAwait(false);
                return;
            }

            var taskThatWon = await Task.WhenAny(work, cancellerIfAny).ConfigureAwait(false);
            if (taskThatWon == cancellerIfAny)
                throw new OperationCanceledException();
        }

        private static Task AsTask(this CancellationToken cancellationToken) // Courtesy of https://github.com/StephenCleary/AsyncEx
        {
            var tcs = new TaskCompletionSource<object>();
            cancellationToken.Register(
                () => tcs.TrySetCanceled(),
                useSynchronizationContext: false
            );
            return tcs.Task;
        }

        [DebuggerStepThrough]
        private static void ThrowForNullTask(Task value, string parameterName)
        {
            if (value is null)
                throw new ArgumentNullException(parameterName);
        }

        [DebuggerStepThrough]
        private static void ThrowForNegativeTimeout(TimeSpan value, string timeoutParameterName)
        {
            if (value.Ticks < 0)
                throw new ArgumentOutOfRangeException(timeoutParameterName, "must be zero / default(meaning do-not-timeout) or a positive duration");
        }
    }
}