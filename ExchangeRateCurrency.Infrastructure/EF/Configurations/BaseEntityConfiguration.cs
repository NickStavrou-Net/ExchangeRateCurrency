using ExchangeRateCurrency.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeRateCurrency.Infrastructure.EF.Configurations;
public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.Property(x => x.CreatedDate)
			.IsRequired()
			.HasDefaultValueSql("CURRENT_TIMESTAMP");

		builder.Property(x => x.UpdatedDate).IsRequired()
			.HasDefaultValueSql("CURRENT_TIMESTAMP").
			ValueGeneratedOnAddOrUpdate();
	}
}
