using EF_ToListAsync_AsAsyncEnumerable.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EF_ToListAsync_AsAsyncEnumerable.Services;

public class SeedService
{
	readonly ILogger<SeedService> _logger;
	readonly ApplicationContext _context;

	public SeedService(ILogger<SeedService> logger, ApplicationContext context)
	{
		_logger = logger;
		_context = context;
	}

	public async Task MigrateAndSeed(CancellationToken cancellationToken)
	{
		await _context.Database.MigrateAsync(cancellationToken);

		if (!await _context.Entities.AnyAsync(cancellationToken))
		{
			for (int i = 0; i < 5; i++)
			{
				for (int n = 0; n < 10000; n++)
				{
					_context.Database.ExecuteSqlRaw(@"INSERT INTO Entities (Name, CreatedAt, SomeProperty1, SomeProperty2, SomeProperty3, SomeProperty4, SomeProperty5, SomeProperty6, SomeProperty7, SomeProperty8, SomeProperty9)
VALUES ({0}, GETUTCDATE(), NEWID(), NEWID(), NEWID(), NEWID(), NEWID(), NEWID(), NEWID(), NEWID(), NEWID())", $"Generated name {i}-{n}");
				}

				_logger.LogInformation("10000 records are added");
			}
		}
	}
}
