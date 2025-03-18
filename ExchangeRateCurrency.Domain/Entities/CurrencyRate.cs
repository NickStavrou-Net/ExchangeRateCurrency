namespace ExchangeRateCurrency.Domain.Entities;
public class CurrencyRate : BaseEntity
{
	public string Currency { get; set; } = string.Empty;
	public decimal Rate { get; set; }
	public DateTime Date { get; set; }
}
