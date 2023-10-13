using Microsoft.EntityFrameworkCore;

namespace EF_ToListAsync_AsAsyncEnumerable.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext() { }

	public ApplicationContext(DbContextOptions options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) { }

	public virtual DbSet<Entity> Entities { get; set; } = null!;
}
