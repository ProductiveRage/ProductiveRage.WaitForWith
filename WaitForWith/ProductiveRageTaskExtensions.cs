using System.Diagnostics;

namespace System.Threading.Tasks
{
    /// <summary>
    /// Extension methods for Tasks that return values that provide functionality similar to Task.WhenAll except that the return values are accessible in a Tuple, making them easy to retrieve (when using Task.WhenAll with Tasks that return different result types,
    /// the options are to access the original Tasks' Result values or to call it and explicitly set its TResult type parameter to object and cast the items in the returned array to the appropriate types - even if Task.WhenAll is used with Tasks that all return
    /// the same type, there is potential for error in it returning an array because calling code could try to access an item outside the bounds of the array and fail at runtime, which is not possible if these methods are used because a Tuple of values is
    /// returned instead of an array).
    /// </summary>
    public static class ProductiveRageTaskExtensions
    {
        /// <summary>
        /// Wait for multiple Tasks that return a value to complete and returns each of their values as a Tuple, in the order in which they were provided (if called as an extension method then the result from the first 'source' Task will be returned first followed
        /// the the other results). This method optionally takes a CancellationToken - if this is set before the Tasks complete then an OperationCanceledException will be raised. Ideally, this parameter would never be given a value and, if cancellation is required
        /// then the CancellationToken would have been shared between whatever provided each of the Tasks originally but sometimes this is not feasible and the optional argument here may be convenient (note that setting it will cancel the Task returned by this
        /// method but it can not cancel each of the provided Tasks - they will run to completion, even though this method will no longer be waiting for their results). If any Task is cancelled while another Task is still running then an OperationCanceledException
        /// will be raised.
        /// </summary>
        /// <typeparam name="T1">The result type of the 'source' Task</typeparam>
        /// <typeparam name="T2">The result type of the 'other' Task</typeparam>
        /// <param name="source">The first Task to wait for</param>
        /// <param name="other">The second Task to wait for</param>
        /// <param name="cancellationToken">An optional CancellationToken that will cancel the Task returned by this method (but which can not cancel the individual Tasks provided - they will continue to run to completion unless the same CancellationToken was used
        /// in the initialisation of each of those, in which case it is redundant to provide it here</param>
        /// <returns>A Tuple containing the results of the Tasks, in the order in which they were specified - so long as they all completed successfully (and, if the cancellationToken parameter was given a value, before that was set)</returns>
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

        /// <summary>
        /// Wait for multiple Tasks that return a value to complete and returns each of their values as a Tuple, in the order in which they were provided (if called as an extension method then the result from the first 'source' Task will be returned first followed
        /// the the other results). This method optionally takes a CancellationToken - if this is set before the Tasks complete then an OperationCanceledException will be raised. Ideally, this parameter would never be given a value and, if cancellation is required
        /// then the CancellationToken would have been shared between whatever provided each of the Tasks originally but sometimes this is not feasible and the optional argument here may be convenient (note that setting it will cancel the Task returned by this
        /// method but it can not cancel each of the provided Tasks - they will run to completion, even though this method will no longer be waiting for their results). If any Task is cancelled while another Task is still running then an OperationCanceledException
        /// will be raised.
        /// </summary>
        /// <typeparam name="T1">The result type of the 'source' Task</typeparam>
        /// <typeparam name="T2">The result type of the 'other1' Task</typeparam>
        /// <typeparam name="T3">The result type of the 'other2' Task</typeparam>
        /// <param name="source">The first Task to wait for</param>
        /// <param name="other1">The second Task to wait for</param>
        /// <param name="other2">The third Task to wait for</param>
        /// <param name="cancellationToken">An optional CancellationToken that will cancel the Task returned by this method (but which can not cancel the individual Tasks provided - they will continue to run to completion unless the same CancellationToken was used
        /// in the initialisation of each of those, in which case it is redundant to provide it here</param>
        /// <returns>A Tuple containing the results of the Tasks, in the order in which they were specified - so long as they all completed successfully (and, if the cancellationToken parameter was given a value, before that was set)</returns>
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

        /// <summary>
        /// Wait for multiple Tasks that return a value to complete and returns each of their values as a Tuple, in the order in which they were provided (if called as an extension method then the result from the first 'source' Task will be returned first followed
        /// the the other results). This method optionally takes a CancellationToken - if this is set before the Tasks complete then an OperationCanceledException will be raised. Ideally, this parameter would never be given a value and, if cancellation is required
        /// then the CancellationToken would have been shared between whatever provided each of the Tasks originally but sometimes this is not feasible and the optional argument here may be convenient (note that setting it will cancel the Task returned by this
        /// method but it can not cancel each of the provided Tasks - they will run to completion, even though this method will no longer be waiting for their results). If any Task is cancelled while another Task is still running then an OperationCanceledException
        /// will be raised.
        /// </summary>
        /// <typeparam name="T1">The result type of the 'source' Task</typeparam>
        /// <typeparam name="T2">The result type of the 'other1' Task</typeparam>
        /// <typeparam name="T3">The result type of the 'other2' Task</typeparam>
        /// <typeparam name="T4">The result type of the 'other3' Task</typeparam>
        /// <param name="source">The first Task to wait for</param>
        /// <param name="other1">The second Task to wait for</param>
        /// <param name="other2">The third Task to wait for</param>
        /// <param name="other3">The fourth Task to wait for</param>
        /// <param name="cancellationToken">An optional CancellationToken that will cancel the Task returned by this method (but which can not cancel the individual Tasks provided - they will continue to run to completion unless the same CancellationToken was used
        /// in the initialisation of each of those, in which case it is redundant to provide it here</param>
        /// <returns>A Tuple containing the results of the Tasks, in the order in which they were specified - so long as they all completed successfully (and, if the cancellationToken parameter was given a value, before that was set)</returns>
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

        /// <summary>
        /// Wait for multiple Tasks that return a value to complete and returns each of their values as a Tuple, in the order in which they were provided (if called as an extension method then the result from the first 'source' Task will be returned first followed
        /// the the other results). This method optionally takes a CancellationToken - if this is set before the Tasks complete then an OperationCanceledException will be raised. Ideally, this parameter would never be given a value and, if cancellation is required
        /// then the CancellationToken would have been shared between whatever provided each of the Tasks originally but sometimes this is not feasible and the optional argument here may be convenient (note that setting it will cancel the Task returned by this
        /// method but it can not cancel each of the provided Tasks - they will run to completion, even though this method will no longer be waiting for their results). If any Task is cancelled while another Task is still running then an OperationCanceledException
        /// will be raised.
        /// </summary>
        /// <typeparam name="T1">The result type of the 'source' Task</typeparam>
        /// <typeparam name="T2">The result type of the 'other1' Task</typeparam>
        /// <typeparam name="T3">The result type of the 'other2' Task</typeparam>
        /// <typeparam name="T4">The result type of the 'other3' Task</typeparam>
        /// <typeparam name="T5">The result type of the 'other4' Task</typeparam>
        /// <param name="source">The first Task to wait for</param>
        /// <param name="other1">The second Task to wait for</param>
        /// <param name="other2">The third Task to wait for</param>
        /// <param name="other3">The fourth Task to wait for</param>
        /// <param name="other4">The fifth Task to wait for</param>
        /// <param name="cancellationToken">An optional CancellationToken that will cancel the Task returned by this method (but which can not cancel the individual Tasks provided - they will continue to run to completion unless the same CancellationToken was used
        /// in the initialisation of each of those, in which case it is redundant to provide it here</param>
        /// <returns>A Tuple containing the results of the Tasks, in the order in which they were specified - so long as they all completed successfully (and, if the cancellationToken parameter was given a value, before that was set)</returns>
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

        /// <summary>
        /// Wait for multiple Tasks that return a value to complete and returns each of their values as a Tuple, in the order in which they were provided (if called as an extension method then the result from the first 'source' Task will be returned first followed
        /// the the other results). This method optionally takes a CancellationToken - if this is set before the Tasks complete then an OperationCanceledException will be raised. Ideally, this parameter would never be given a value and, if cancellation is required
        /// then the CancellationToken would have been shared between whatever provided each of the Tasks originally but sometimes this is not feasible and the optional argument here may be convenient (note that setting it will cancel the Task returned by this
        /// method but it can not cancel each of the provided Tasks - they will run to completion, even though this method will no longer be waiting for their results). If any Task is cancelled while another Task is still running then an OperationCanceledException
        /// will be raised.
        /// </summary>
        /// <typeparam name="T1">The result type of the 'source' Task</typeparam>
        /// <typeparam name="T2">The result type of the 'other1' Task</typeparam>
        /// <typeparam name="T3">The result type of the 'other2' Task</typeparam>
        /// <typeparam name="T4">The result type of the 'other3' Task</typeparam>
        /// <typeparam name="T5">The result type of the 'other4' Task</typeparam>
        /// <typeparam name="T6">The result type of the 'other5' Task</typeparam>
        /// <param name="source">The first Task to wait for</param>
        /// <param name="other1">The second Task to wait for</param>
        /// <param name="other2">The third Task to wait for</param>
        /// <param name="other3">The fourth Task to wait for</param>
        /// <param name="other4">The fifth Task to wait for</param>
        /// <param name="other5">The sixth Task to wait for</param>
        /// <param name="cancellationToken">An optional CancellationToken that will cancel the Task returned by this method (but which can not cancel the individual Tasks provided - they will continue to run to completion unless the same CancellationToken was used
        /// in the initialisation of each of those, in which case it is redundant to provide it here</param>
        /// <returns>A Tuple containing the results of the Tasks, in the order in which they were specified - so long as they all completed successfully (and, if the cancellationToken parameter was given a value, before that was set)</returns>
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

        /// <summary>
        /// Wait for multiple Tasks that return a value to complete and returns each of their values as a Tuple, in the order in which they were provided (if called as an extension method then the result from the first 'source' Task will be returned first followed
        /// the the other results). This method optionally takes a CancellationToken - if this is set before the Tasks complete then an OperationCanceledException will be raised. Ideally, this parameter would never be given a value and, if cancellation is required
        /// then the CancellationToken would have been shared between whatever provided each of the Tasks originally but sometimes this is not feasible and the optional argument here may be convenient (note that setting it will cancel the Task returned by this
        /// method but it can not cancel each of the provided Tasks - they will run to completion, even though this method will no longer be waiting for their results). If any Task is cancelled while another Task is still running then an OperationCanceledException
        /// will be raised.
        /// </summary>
        /// <typeparam name="T1">The result type of the 'source' Task</typeparam>
        /// <typeparam name="T2">The result type of the 'other1' Task</typeparam>
        /// <typeparam name="T3">The result type of the 'other2' Task</typeparam>
        /// <typeparam name="T4">The result type of the 'other3' Task</typeparam>
        /// <typeparam name="T5">The result type of the 'other4' Task</typeparam>
        /// <typeparam name="T6">The result type of the 'other5' Task</typeparam>
        /// <typeparam name="T7">The result type of the 'other6' Task</typeparam>
        /// <param name="source">The first Task to wait for</param>
        /// <param name="other1">The second Task to wait for</param>
        /// <param name="other2">The third Task to wait for</param>
        /// <param name="other3">The fourth Task to wait for</param>
        /// <param name="other4">The fifth Task to wait for</param>
        /// <param name="other5">The sixth Task to wait for</param>
        /// <param name="other6">The seventh Task to wait for</param>
        /// <param name="cancellationToken">An optional CancellationToken that will cancel the Task returned by this method (but which can not cancel the individual Tasks provided - they will continue to run to completion unless the same CancellationToken was used
        /// in the initialisation of each of those, in which case it is redundant to provide it here</param>
        /// <returns>A Tuple containing the results of the Tasks, in the order in which they were specified - so long as they all completed successfully (and, if the cancellationToken parameter was given a value, before that was set)</returns>
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

        /// <summary>
        /// Wait for multiple Tasks that return a value to complete and returns each of their values as a Tuple, in the order in which they were provided (if called as an extension method then the result from the first 'source' Task will be returned first followed
        /// the the other results). This method optionally takes a CancellationToken - if this is set before the Tasks complete then an OperationCanceledException will be raised. Ideally, this parameter would never be given a value and, if cancellation is required
        /// then the CancellationToken would have been shared between whatever provided each of the Tasks originally but sometimes this is not feasible and the optional argument here may be convenient (note that setting it will cancel the Task returned by this
        /// method but it can not cancel each of the provided Tasks - they will run to completion, even though this method will no longer be waiting for their results). If any Task is cancelled while another Task is still running then an OperationCanceledException
        /// will be raised.
        /// </summary>
        /// <typeparam name="T1">The result type of the 'source' Task</typeparam>
        /// <typeparam name="T2">The result type of the 'other1' Task</typeparam>
        /// <typeparam name="T3">The result type of the 'other2' Task</typeparam>
        /// <typeparam name="T4">The result type of the 'other3' Task</typeparam>
        /// <typeparam name="T5">The result type of the 'other4' Task</typeparam>
        /// <typeparam name="T6">The result type of the 'other5' Task</typeparam>
        /// <typeparam name="T7">The result type of the 'other6' Task</typeparam>
        /// <typeparam name="T8">The result type of the 'other7' Task</typeparam>
        /// <param name="source">The first Task to wait for</param>
        /// <param name="other1">The second Task to wait for</param>
        /// <param name="other2">The third Task to wait for</param>
        /// <param name="other3">The fourth Task to wait for</param>
        /// <param name="other4">The fifth Task to wait for</param>
        /// <param name="other5">The sixth Task to wait for</param>
        /// <param name="other6">The seventh Task to wait for</param>
        /// <param name="other7">The eigth Task to wait for</param>
        /// <param name="cancellationToken">An optional CancellationToken that will cancel the Task returned by this method (but which can not cancel the individual Tasks provided - they will continue to run to completion unless the same CancellationToken was used
        /// in the initialisation of each of those, in which case it is redundant to provide it here</param>
        /// <returns>A Tuple containing the results of the Tasks, in the order in which they were specified - so long as they all completed successfully (and, if the cancellationToken parameter was given a value, before that was set)</returns>
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

        /// <summary>
        /// Wait for multiple Tasks that return a value to complete and returns each of their values as a Tuple, in the order in which they were provided (if called as an extension method then the result from the first 'source' Task will be returned first followed
        /// the the other results). This method optionally takes a CancellationToken - if this is set before the Tasks complete then an OperationCanceledException will be raised. Ideally, this parameter would never be given a value and, if cancellation is required
        /// then the CancellationToken would have been shared between whatever provided each of the Tasks originally but sometimes this is not feasible and the optional argument here may be convenient (note that setting it will cancel the Task returned by this
        /// method but it can not cancel each of the provided Tasks - they will run to completion, even though this method will no longer be waiting for their results). If any Task is cancelled while another Task is still running then an OperationCanceledException
        /// will be raised.
        /// </summary>
        /// <typeparam name="T1">The result type of the 'source' Task</typeparam>
        /// <typeparam name="T2">The result type of the 'other1' Task</typeparam>
        /// <typeparam name="T3">The result type of the 'other2' Task</typeparam>
        /// <typeparam name="T4">The result type of the 'other3' Task</typeparam>
        /// <typeparam name="T5">The result type of the 'other4' Task</typeparam>
        /// <typeparam name="T6">The result type of the 'other5' Task</typeparam>
        /// <typeparam name="T7">The result type of the 'other6' Task</typeparam>
        /// <typeparam name="T8">The result type of the 'other7' Task</typeparam>
        /// <typeparam name="T9">The result type of the 'other8' Task</typeparam>
        /// <param name="source">The first Task to wait for</param>
        /// <param name="other1">The second Task to wait for</param>
        /// <param name="other2">The third Task to wait for</param>
        /// <param name="other3">The fourth Task to wait for</param>
        /// <param name="other4">The fifth Task to wait for</param>
        /// <param name="other5">The sixth Task to wait for</param>
        /// <param name="other6">The seventh Task to wait for</param>
        /// <param name="other7">The eigth Task to wait for</param>
        /// <param name="other8">The ninth Task to wait for</param>
        /// <param name="cancellationToken">An optional CancellationToken that will cancel the Task returned by this method (but which can not cancel the individual Tasks provided - they will continue to run to completion unless the same CancellationToken was used
        /// in the initialisation of each of those, in which case it is redundant to provide it here</param>
        /// <returns>A Tuple containing the results of the Tasks, in the order in which they were specified - so long as they all completed successfully (and, if the cancellationToken parameter was given a value, before that was set)</returns>
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

        /// <summary>
        /// Wait for multiple Tasks that return a value to complete and returns each of their values as a Tuple, in the order in which they were provided (if called as an extension method then the result from the first 'source' Task will be returned first followed
        /// the the other results). This method optionally takes a CancellationToken - if this is set before the Tasks complete then an OperationCanceledException will be raised. Ideally, this parameter would never be given a value and, if cancellation is required
        /// then the CancellationToken would have been shared between whatever provided each of the Tasks originally but sometimes this is not feasible and the optional argument here may be convenient (note that setting it will cancel the Task returned by this
        /// method but it can not cancel each of the provided Tasks - they will run to completion, even though this method will no longer be waiting for their results). If any Task is cancelled while another Task is still running then an OperationCanceledException
        /// will be raised.
        /// </summary>
        /// <typeparam name="T1">The result type of the 'source' Task</typeparam>
        /// <typeparam name="T2">The result type of the 'other1' Task</typeparam>
        /// <typeparam name="T3">The result type of the 'other2' Task</typeparam>
        /// <typeparam name="T4">The result type of the 'other3' Task</typeparam>
        /// <typeparam name="T5">The result type of the 'other4' Task</typeparam>
        /// <typeparam name="T6">The result type of the 'other5' Task</typeparam>
        /// <typeparam name="T7">The result type of the 'other6' Task</typeparam>
        /// <typeparam name="T8">The result type of the 'other7' Task</typeparam>
        /// <typeparam name="T9">The result type of the 'other8' Task</typeparam>
        /// <typeparam name="T10">The result type of the 'other9' Task</typeparam>
        /// <param name="source">The first Task to wait for</param>
        /// <param name="other1">The second Task to wait for</param>
        /// <param name="other2">The third Task to wait for</param>
        /// <param name="other3">The fourth Task to wait for</param>
        /// <param name="other4">The fifth Task to wait for</param>
        /// <param name="other5">The sixth Task to wait for</param>
        /// <param name="other6">The seventh Task to wait for</param>
        /// <param name="other7">The eigth Task to wait for</param>
        /// <param name="other8">The ninth Task to wait for</param>
        /// <param name="other9">The tenth Task to wait for</param>
        /// <param name="cancellationToken">An optional CancellationToken that will cancel the Task returned by this method (but which can not cancel the individual Tasks provided - they will continue to run to completion unless the same CancellationToken was used
        /// in the initialisation of each of those, in which case it is redundant to provide it here</param>
        /// <returns>A Tuple containing the results of the Tasks, in the order in which they were specified - so long as they all completed successfully (and, if the cancellationToken parameter was given a value, before that was set)</returns>
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