using ExchangeRateCurrency.Domain.Entities;

namespace ExchangeRateCurrency.Application.Jobs;
public interface IExchangeRateCurrencyService
{
	Task ExecuteAsync(CancellationToken ct);

	Task<bool> UpdateDatabaseRatesAsync(IEnumerable<CurrencyRate> currencyRates, CancellationToken ct);
}
