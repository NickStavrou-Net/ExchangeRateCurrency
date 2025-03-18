namespace ExchangeRateCurrency.Application.Options;

public class CronOptions
{
	public string CronExpression { get; set; } = string.Empty;
	public bool Enabled { get; set; }
}
