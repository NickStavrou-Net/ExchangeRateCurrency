using ExchangeRateCurrency.Domain.Entities;

namespace ExchangeRateCurrency.Domain.Interfaces;
public interface IEcbRateService
{
	Task<IEnumerable<CurrencyRate>> GetCurrencyRatesAsync(CancellationToken cancellationToken);

	Task<IEnumerable<CurrencyRate>> GetCachedRatesAsync(CancellationToken ct);
}
