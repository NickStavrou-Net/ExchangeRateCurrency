using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExchangeRateCurrency.Domain.Entities;
public abstract class BaseEntity
{
	protected BaseEntity()
	{
		CreatedDate = DateTime.UtcNow;
		SetUpdatedDate();
	}

	[JsonIgnore]
	public DateTime CreatedDate { get; protected set; }

	[ConcurrencyCheck]
	[JsonIgnore]
	public DateTime UpdatedDate { get; protected set; }

	public void SetUpdatedDate()
	{
		UpdatedDate = DateTime.UtcNow;
	}
}
