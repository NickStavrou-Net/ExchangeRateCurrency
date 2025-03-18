using ExchangeRateCurrency.Domain.Entities;

namespace ExchangeRateCurrency.Domain.Interfaces;
public interface IExchangeRateCurrrencyRepository
{
	Task<int> ExecuteMergeSqlAsync(string tableName, CancellationToken token);
	Task<int> ExecuteCreateTempTableSqlAsync(string tableName, CancellationToken token);
	Task<IEnumerable<CurrencyRate>> GetCurrencyRatesAsync(CancellationToken ct);
}
