# EF-ToListAsync-AsAsyncEnumerable

## Intro
The main purpose of this project is to compare the memory consumption of the two methods `ToListAsync` and `AsAsyncEnumerable`. It would also be nice to compare performance.

The program collects the following metrics::

* **Duration**: The time taken for the operation.
* **Process Private Memory Usage**: Memory used by the process.
* **Garbage Collection Total Memory**: Total memory used by the garbage collector.

A few words about CancellationToken.
For "ToListAsync" we use it directly `.ToListAsync(cancellationToken)`, for "AsAsyncEnumerable" we have to do it manually `cancellationToken.ThrowIfCancellationRequested();` and it can a performance issue.

## Getting Started
### Prerequisites
Before running the code, make sure to set up the database connection string in the appsettings.json file. The connection string should be named "DefaultConnection".

``` JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "your_database_connection_string_here"
  }
}
```
