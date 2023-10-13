using Microsoft.Extensions.Logging;

namespace EF_ToListAsync_AsAsyncEnumerable.Services;

public class MetricsCollectorFactory
{
	readonly ILogger<MetricsCollector> _logger;

	public MetricsCollectorFactory(ILogger<MetricsCollector> logger)
	{
		_logger = logger;
	}

	public MetricsCollector CreateMetricsCollector(string name)
	{
		return new MetricsCollector(_logger, name);
	}
}

public class MetricsCollector
{
	readonly ILogger<MetricsCollector> _logger;
	readonly string _name;
	DateTime _commandStartTime = DateTime.UtcNow;
	DateTime _commandEndTime = DateTime.UtcNow;
	long _privateMemorySize64;
	long _cgTotalMemory;

	public MetricsCollector(ILogger<MetricsCollector> logger, string name)
	{
		_logger = logger;
		_name = name;
	}

	public void Start()
	{
		_commandStartTime = DateTime.UtcNow;

		using (var proc = System.Diagnostics.Process.GetCurrentProcess())
		{
			proc.Refresh();
			_privateMemorySize64 = proc.PrivateMemorySize64;
		}

		_cgTotalMemory = GC.GetTotalMemory(true);
	}

	public void End(object? message)
	{
		_commandEndTime = DateTime.UtcNow;
		WriteDump(message);
	}

	void WriteDump(object? message)
	{
		long privateMemorySize64;

		using (var proc = System.Diagnostics.Process.GetCurrentProcess())
		{
			proc.Refresh();
			privateMemorySize64 = proc.PrivateMemorySize64;
		}

		var cgTotalMemory = GC.GetTotalMemory(true);

		_logger.LogInformation(@"==={name}==={message}
{duration}
{_privateMemorySize64:000000000} => {privateMemorySize64:000000000} Process private memory usage
{_cgTotalMemory:000000000} => {cgTotalMemory:000000000} GC.GetTotalMemory",
			_name, message,
			GetFormatedDuration(_commandStartTime, _commandEndTime),
			_privateMemorySize64, privateMemorySize64,
			_cgTotalMemory, cgTotalMemory);
	}

	static string GetFormatedDuration(DateTime startTime, DateTime endTime)
	{
		var ts = endTime - startTime;
		return $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{(ts.Milliseconds / 10):00}";
	}
}
