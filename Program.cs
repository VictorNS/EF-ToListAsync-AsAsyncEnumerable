using EF_ToListAsync_AsAsyncEnumerable.Data;
using EF_ToListAsync_AsAsyncEnumerable.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host
	.CreateDefaultBuilder()
	.ConfigureLogging(loggingBuilder =>
	{
		loggingBuilder.ClearProviders();
		loggingBuilder.AddConsole();
	})
	.ConfigureServices((hostContext, services) =>
	{
		// Validation
		var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
		if (string.IsNullOrEmpty(connectionString))
			throw new Exception("ConnectionStrings 'DefaultConnection' is expected in appsettings.json");

		services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
		services.AddScoped<SeedService>();
		services.AddScoped<ApplicationContextFactory>();
		services.AddScoped<FileWriterFactory>();
		services.AddScoped<MetricsCollectorFactory>();
		services.AddScoped<ExportService>();
	})
	.Build();

using (var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
	await host.Services.GetRequiredService<SeedService>().MigrateAndSeed(CancellationToken.None);
}

using (var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
	for (int i = 0; i < 2; i++)
	{
		await host.Services.GetRequiredService<ExportService>().ExportToListAsync(10, CancellationToken.None);
		await host.Services.GetRequiredService<ExportService>().ExportAsAsyncEnumerable(10, CancellationToken.None);
	}

	for (int i = 0; i < 2; i++)
	{
		await host.Services.GetRequiredService<ExportService>().ExportToListAsync(null, CancellationToken.None);
		await host.Services.GetRequiredService<ExportService>().ExportAsAsyncEnumerable(null, CancellationToken.None);
	}
}
