# WaitForWith

For waiting for multiple Tasks to run to completion, in a strongly-typed manner, where they don't all return the same result type.

## Why is this useful?

.NET already has some mechanisms to wait for multiple tasks to complete. For example, to simulate initiating two expensive / long-running tasks that return the same type and then waiting for both of those tasks to complete and to retrieve the results, we could do the following:

	var timer = Stopwatch.StartNew();
	var task1 = ExpensiveTask1();
	var task2 = ExpensiveTask2();
	await Task.WhenAll(task1, task2);
	timer.Stop();
	Console.WriteLine("Completed in " + timer.Elapsed);
	Console.WriteLine("Result 1: " + task1.Result);
	Console.WriteLine("Result 2: " + task2.Result);

	static async Task<string> ExpensiveTask1()
	{
		await Task.Delay(TimeSpan.FromSeconds(1));
		return "ABC";
	}

	static async Task<string> ExpensiveTask2()
	{
		await Task.Delay(TimeSpan.FromSeconds(2));
		return "XYZ";
	}

This will show that it took about 2s for them *both* to have completed, since the second task takes 2s to execute while the first only takes 1s.

An alternative approach to the above would be to use the result of `Task.WhenAll` like this:

	var timer = Stopwatch.StartNew();
    var results = await Task.WhenAll(ExpensiveTask1(), ExpensiveTask2());
    timer.Stop();
    Console.WriteLine("Completed in " + timer.Elapsed);
    Console.WriteLine("Result 1: " + results[0]);
    Console.WriteLine("Result 2: " + results[1]);

	static async Task<string> ExpensiveTask1()
	{
		await Task.Delay(TimeSpan.FromSeconds(1));
		return "ABC";
	}

	static async Task<string> ExpensiveTask2()
	{
		await Task.Delay(TimeSpan.FromSeconds(2));
		return "XYZ";
	}

This code is shorter because we don't have to keep local references to the original tasks but it introduces the potential for error because `Task.WhenAll` returns an array and so you could write code that accesses an out of bounds result and only encounter the failure at runtime as an `IndexOutOfRangeException` -

	var timer = Stopwatch.StartNew();
    var results = await Task.WhenAll(ExpensiveTask1(), ExpensiveTask2());
    timer.Stop();
    Console.WriteLine("Completed in " + timer.Elapsed);
    Console.WriteLine("Result 1: " + results[0]);
    Console.WriteLine("Result 2: " + results[1]);
    Console.WriteLine("Result 3: " + results[2]); // OOPS! There IS no third element!

	static async Task<string> ExpensiveTask1()
	{
		await Task.Delay(TimeSpan.FromSeconds(1));
		return "ABC";
	}

	static async Task<string> ExpensiveTask2()
	{
		await Task.Delay(TimeSpan.FromSeconds(2));
		return "XYZ";
	}

Another disadvantage to this approach is that all of the Tasks must have the same return type and so the following will not compile:

    // FAIL: This line will not compile as the only Task.WhenAll overload that will take Tasks with
	// different return types is one that returns void and so we can't assign 'results' value
	var results = await Task.WhenAll(ExpensiveTask1(), ExpensiveTask2());
    Console.WriteLine("Result 1: " + results[0]);
    Console.WriteLine("Result 2: " + results[1]);

    static async Task<string> ExpensiveTask1()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return "ABC";
    }

    static async Task<int> ExpensiveTask2()
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        return 123;
    }
	
One option is to return to the first approach because that doesn't require the return value from `Task.WhenAll`, though it can result on more verbose code.

What I *really* want is for there to already be some .NET magic where I can have the succinct approach *and* multiple return types. Failing that, what I'd like is to add my own method overloads to the static `Task` class. However, neither of these are possible. So I've gone for writing a library that allows the following compromise:

    var timer = Stopwatch.StartNew();
    var (result1, result2) = await ExpensiveTask1().WaitForWith(ExpensiveTask2());
    timer.Stop();
    Console.WriteLine("Completed in " + timer.Elapsed);
    Console.WriteLine("Result 1: " + result1);
    Console.WriteLine("Result 2: " + result2);

    static async Task<string> ExpensiveTask1()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        return "ABC";
    }

    static async Task<int> ExpensiveTask2()
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        return 123;
    }

If you like, the `WaitForWith` extension method *can* be called in a manner more similar to `Task.WhenAll` but it's a little more verbose -

    var (result1, result2) = await ProductiveRageTaskExtensions.WaitForWith(ExpensiveTask1(), ExpensiveTask2());

.. compared to:

    var (result1, result2) = await ExpensiveTask1().WaitForWith(ExpensiveTask2());

I prefer the syntax of the extension method code (the second example) but I guess that it's just a matter of taste!

## How many tasks can be combined in this manner?

The example above only shows two tasks being waited to complete but the library has method overloads that will handle up to ten. So you *could*, if you needed to, write something like:

    var (r1, r2, r3, r4, r5, r6, r7, r8, r9, r10) = await ExpensiveTask1().WaitForWith(
	    ExpensiveTask2(),
	    ExpensiveTask3(),
	    ExpensiveTask4(),
	    ExpensiveTask5(),
	    ExpensiveTask6(),
	    ExpensiveTask7(),
	    ExpensiveTask8(),
	    ExpensiveTask9(),
	    ExpensiveTask10()
	);

Hopefully this will be more than enough but maybe I'll extend the library's overloads further one day if there's need!

I *suspect* that if there were a use case for more than ten tasks that it's more likely that they would have the same return type and the existing `Task.WhenAll` and its returned array would suffice\* but that's just my gut instinct and not based on any real data at this time.

\* *(Though it would still have the potential for an out-of-bounds array access in the returned reference, which would be thrown at runtime and a primary drive for me to have written this code is to enable the compiler to catch more mistakes for me!)*

## Optional "cancellationToken" and "timeout" parameters

The methods in this library have some additional overloads that allow either a cancellation token or a timeout to be passed which can result in the returned task (the tuple that would have the results of all of the combined tasks if they all ran to completion) to be cancelled - eg.

	var (result1, result2) = await ExpensiveTask1().WaitForWith(
	    ExpensiveTask2(),
		timeout: TimeSpan.FromSeconds(15)
	);

or

	var (result1, result2) = await ExpensiveTask1().WaitForWith(
	    ExpensiveTask2(),
		cancellationToken
	);

If these mechanisms are used to cancel the work then it's important to bear in mind that they only cancel the task that the extension method returns, it is unable to cancel the individual tasks. As such, these overloads should probably only be considered a convenience for when there is no way to specify a method of cancellation for the original tasks.

The ideal way to handle timeouts would be for each of the original tasks to have a time-based cancellation token, like this:

	var cancellationTokenSource = new CancellationTokenSource();
	cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(15));
	var (result1, result2) = await ExpensiveTask1(cancellationTokenSource.Token).WaitForWith(
	    ExpensiveTask2(cancellationTokenSource.Token)
	);

.. because this would ensure that each of the source tasks were cancelled (which would then result in the combined-result-from-WaitForWith task being cancelled as well).

Cancelling the combined result task without the individual tasks being cancelled will, in most cases, be a waste of resources because the individual tasks will still be doing their thing while the caller of `WaitForWith` has had a cancellation exception thrown.

However, there *are* some occasions where this could be useful. Firstly, if there really is no way to create the source tasks without being able to provide them with a cancellation token (which whould never be the case in an ideal world - if a library has a public method that returns a `Task` then it should, really, always take an optional `CancellationToken` parameter). Secondly, you might have a situation where you don't want the individual tasks to be cancelled if they don't complete within {x} number of seconds but you *do* want the calling code to continue. The following is perhaps a slightly contrived example but hopefully it makes the point -

	try
	{
	    return await GetResultsIfReadyQuickly(acceptableTimeToWait: TimeSpan.FromSeconds(15));
	}
	catch (OperationCanceledException)
	{
	    // The results weren't generated quickly enough, so we'll return an empty response and show a "not-ready-yet"
		// message to the User for now. When the results ARE fully generated, they'll be cached and so should be available
		// for future requests.
		return default;
	}
	
	private static Task<(string result1, int result2)> GetResultsIfReadyQuickly(TimeSpan acceptableTimeToWait)
	{
	    return ExpensiveTask1().WaitForWith(ExpensiveTask2(), acceptableTimeToWait);
	}

## A note on the method name and its lack of an "Async" suffix

In times gone by, I would have used the "Async" suffix on methods that returned a Task (ie. I would have called the extension method "WaitForWithAsync", as opposed to just "WaitForWith") as Microsoft previously documented a convention that

> You should add "Async" as the suffix of every async method name you write.
> - https://docs.microsoft.com/en-us/dotnet/csharp/async#important-info-and-advice

However, there is a school of thought that this was only specified to clearly differentiate between methods in APIs where there were a combination of synchronous and asynchronous methods. Since the public API here is 100% asynchronous (by necessity, as it is dealing with asynchronous tasks), there is no ambiguity and so the "Async" suffix has not been appended to the method names (as inspired by [ParticularDocs' No Async Suffix](https://docs.particular.net/nservicebus/upgrades/5to6/async-suffix) article).