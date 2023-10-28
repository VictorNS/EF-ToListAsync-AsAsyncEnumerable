# Entity Framework Memory Optimization: Comparing `ToListAsync` and `AsAsyncEnumerable`

## Idea
The main purpose of this project was to compare the memory consumption of the two methods `ToListAsync` and `AsAsyncEnumerable` and try to do it with something very similar to a real example. It would also be a good idea to check that the performance is still the same.

### A Little Bit of Theory
The `ToListAsync` method is part of Entity Framework and is commonly used when working with asynchronous queries. When applied to an Entity Framework query, `ToListAsync` asynchronously executes the query and materializes the results into a `List<T>`. This method is particularly useful when dealing with large datasets, as it allows for the asynchronous retrieval of data, preventing the blocking of the calling thread.

The `AsAsyncEnumerable` method, introduced in more recent versions of Entity Framework, is part of the broader support for asynchronous programming patterns. Unlike `ToListAsync`, which directly materializes results into a `List<T>`, `AsAsyncEnumerable` returns an `IAsyncEnumerable<T>`, a part of the `System.Collections.Async` namespace, representing a sequence of elements that can be asynchronously enumerated. This method is beneficial in scenarios where you want to work with the results in an asynchronous and streaming fashion, potentially consuming the data before the entire result set has been retrieved from the database.

In summary, while `ToListAsync` is used for asynchronous execution and immediate materialization of results into a list, `AsAsyncEnumerable` provides a more flexible, asynchronous, and potentially streaming interface for working with query results. The choice between them depends on the specific requirements of the application and how the data needs to be processed.

## Implementation
So, the project contains an Entity Framework data context with one model and a seed class that creates 50,000 records. The application then retrieves all records and writes them to a file. We do not store this file in memory, so it should not affect memory consumption.

A few words about `CancellationToken`: for `ToListAsync`, we use it directly `.ToListAsync(cancellationToken)`, for `AsAsyncEnumerable`, we have to do it manually `cancellationToken.ThrowIfCancellationRequested();`, and this can be a performance issue. But since I usually use this kind of code in web applications, I need the ability to interrupt the execution of processes.

## Experiment Result
On my computer, I got the following result:
```
ToListAsync
      00:00:07.44
      044044288 => 118222848 Process private memory usage
      003445800 => 003485128 GC.GetTotalMemory
AsAsyncEnumerable
      00:00:08.13
      098140160 => 049934336 Process private memory usage
      003485920 => 003496064 GC.GetTotalMemory
ToListAsync
      00:00:08.09
      049934336 => 114241536 Process private memory usage
      003495808 => 003493920 GC.GetTotalMemory
AsAsyncEnumerable
      00:00:08.08
      094244864 => 056999936 Process private memory usage
      003494680 => 003492216 GC.GetTotalMemory
```

In conclusion, the `AsAsyncEnumerable` method demonstrates significantly lower memory consumption compared to `ToListAsync`. Developers should consider these findings when optimizing applications with large datasets to ensure efficient resource utilization.

## Prerequisites
Before running the code, make sure to set up the database connection string in the appsettings.json file. 
``` JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "your_database_connection_string_here"
  }
}
```
