using EF_ToListAsync_AsAsyncEnumerable.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EF_ToListAsync_AsAsyncEnumerable.Services;

public class ApplicationContextFactory
{
	readonly IConfiguration _configuration;

	public ApplicationContextFactory(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public ApplicationContext CreateContext()
	{
		var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
		optionsBuilder
			.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")!)
			.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		return new ApplicationContext(optionsBuilder.Options);
	}
}
