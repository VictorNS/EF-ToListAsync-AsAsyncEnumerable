using Microsoft.EntityFrameworkCore;

namespace EF_ToListAsync_AsAsyncEnumerable.Services;

public class ExportService
{
	readonly ApplicationContextFactory _applicationContextFactory;
	readonly FileWriterFactory _fileWriterFactory;
	readonly MetricsCollectorFactory _metricsCollectorFactory;

	public ExportService(ApplicationContextFactory applicationContextFactory, FileWriterFactory fileWriterFactory, MetricsCollectorFactory metricsCollectorFactory)
	{
		_applicationContextFactory = applicationContextFactory;
		_fileWriterFactory = fileWriterFactory;
		_metricsCollectorFactory = metricsCollectorFactory;
	}

	public async Task ExportToListAsync(int? count, CancellationToken cancellationToken)
	{
		var metricsCollector = _metricsCollectorFactory.CreateMetricsCollector("ToListAsync");
		metricsCollector.Start();

		using (var context = _applicationContextFactory.CreateContext())
		{
			var writer = _fileWriterFactory.CreateWriter();

			var list = count is null
				? await context.Entities.ToListAsync(cancellationToken)
				: await context.Entities.Take(count.Value).ToListAsync(cancellationToken);

			foreach (var x in list)
			{
				writer.Write($"{x.Id} {x.Name} {x.CreatedAt} {x.SomeProperty1} {x.SomeProperty2} {x.SomeProperty3} {x.SomeProperty4} {x.SomeProperty5} {x.SomeProperty6} {x.SomeProperty7} {x.SomeProperty8} {x.SomeProperty9}");
			}
		}

		await Task.Delay(1000, cancellationToken);
		metricsCollector.End(count);
	}

	public async Task ExportAsAsyncEnumerable(int? count, CancellationToken cancellationToken)
	{
		var metricsCollector = _metricsCollectorFactory.CreateMetricsCollector("AsAsyncEnumerable");
		metricsCollector.Start();

		using (var context = _applicationContextFactory.CreateContext())
		{
			var writer = _fileWriterFactory.CreateWriter();

			var list = count is null
				? context.Entities.AsAsyncEnumerable()
				: context.Entities.Take(count.Value).AsAsyncEnumerable();

			await foreach (var x in list)
			{
				cancellationToken.ThrowIfCancellationRequested();
				writer.Write($"{x.Id} {x.Name} {x.CreatedAt} {x.SomeProperty1} {x.SomeProperty2} {x.SomeProperty3} {x.SomeProperty4} {x.SomeProperty5} {x.SomeProperty6} {x.SomeProperty7} {x.SomeProperty8} {x.SomeProperty9}");
			}
		}

		await Task.Delay(1000, cancellationToken);
		metricsCollector.End(count);
	}
}
