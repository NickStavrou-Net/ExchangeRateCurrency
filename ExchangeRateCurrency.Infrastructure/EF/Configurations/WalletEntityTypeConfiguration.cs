using ExchangeRateCurrency.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeRateCurrency.Infrastructure.EF.Configurations;
public class WalletEntityTypeConfiguration : BaseEntityConfiguration<Wallet>
{
	public override void Configure(EntityTypeBuilder<Wallet> builder)
	{
		builder.ToTable("Wallets");

		builder.HasKey(w => w.Id);

		builder.HasIndex(w => w.Id).IsUnique();

		builder.Property(w => w.Balance)
				.IsRequired()
				.HasColumnType("decimal(18,2)");

		builder.Property(w => w.Currency)
				.IsRequired()
				.HasMaxLength(3);

	}
}
