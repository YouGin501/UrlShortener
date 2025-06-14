using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
	public class DataContext : IdentityDbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{ }

		public DbSet<ShortUrl> ShortUrls { get; set; }
		public DbSet<AboutPage> AboutPages { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<ShortUrl>()
				.HasIndex(x => x.ShortCode)
				.IsUnique();

			builder.Entity<ShortUrl>()
				.HasIndex(x => x.OriginalUrl)
				.IsUnique();
		}
	}

}
