using ExchangeRateCurrency.Domain.Entities;
using ExchangeRateCurrency.Infrastructure.EF.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateCurrency.Infrastructure.EF;
public class ExchageRateCurrencyDbContext : DbContext
{
	public ExchageRateCurrencyDbContext(DbContextOptions<ExchageRateCurrencyDbContext> options)
		: base(options) { }

	public DbSet<CurrencyRate> CurrencyRates { get; set; }
	public DbSet<Wallet> Wallets { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new CurrencyRateEntityTypeConfiguration());
		modelBuilder.ApplyConfiguration(new WalletEntityTypeConfiguration());

		foreach (var entity in modelBuilder.Model.GetEntityTypes())
		{
			// Convert table names to snake_case
			entity.SetTableName(ToSnakeCase(entity.GetTableName()));

			foreach (var property in entity.GetProperties())
			{
				// Convert column names to snake_case
				property.SetColumnName(ToSnakeCase(property.GetColumnName()));
			}

			foreach (var key in entity.GetKeys())
			{
				key.SetName(ToSnakeCase(key.GetName()));
			}

			foreach (var foreignKey in entity.GetForeignKeys())
			{
				foreignKey.SetConstraintName(ToSnakeCase(foreignKey.GetConstraintName()));
			}
		}

		foreach (var entity in modelBuilder.Model.GetEntityTypes())
		{
			foreach (var index in entity.GetIndexes())
			{
				modelBuilder.Entity(entity.ClrType).HasIndex(index.Properties.Select(p => p.Name).ToArray())
					.HasDatabaseName(ToSnakeCase(index.Name));
			}
		}

		base.OnModelCreating(modelBuilder);
	}

	private string ToSnakeCase(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
			return input;

		return string.Concat(input
			.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()))
			.ToLower();
	}
}
