using ExchangeRateCurrency.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeRateCurrency.Infrastructure.EF.Configurations;
public class CurrencyRateEntityTypeConfiguration : BaseEntityConfiguration<CurrencyRate>
{
	public override void Configure(EntityTypeBuilder<CurrencyRate> builder)
	{
		builder.ToTable("CurrencyRates");

		builder.HasKey(c => new { c.Currency, c.Date });
	}
}
