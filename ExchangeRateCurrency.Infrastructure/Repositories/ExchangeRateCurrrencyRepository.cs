using ExchangeRateCurrency.Domain.Entities;
using ExchangeRateCurrency.Domain.Interfaces;
using ExchangeRateCurrency.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateCurrency.Infrastructure.Repositories;
public class ExchangeRateCurrrencyRepository : IExchangeRateCurrrencyRepository
{
	private readonly ExchageRateCurrencyDbContext _context;

	public ExchangeRateCurrrencyRepository(ExchageRateCurrencyDbContext context)
	{
		_context = context;
	}

	public Task<int> ExecuteCreateTempTableSqlAsync(string tableName, CancellationToken ct)
		=> _context.Database.ExecuteSqlRawAsync(
				FormattableString.Invariant(
					$"CREATE TABLE {tableName} (Currency NVARCHAR(50), Rate DECIMAL(18, 2), Date DATETIME, UpdatedDate DATETIME)"), ct);

	public async Task<int> ExecuteMergeSqlAsync(string tableName, CancellationToken ct)
		=> await _context.Database.ExecuteSqlRawAsync(
				FormattableString.Invariant(@$"MERGE INTO CurrencyRates AS targetTable
					USING (SELECT Currency, Rate, Date, UpdatedDate FROM {tableName}) AS sourceTable
					ON targetTable.Currency = sourceTable.Currency AND targetTable.Date = sourceTable.Date
					WHEN MATCHED AND targetTable.Rate <> sourceTable.Rate
					THEN
						UPDATE SET 
							targetTable.Rate = sourceTable.Rate,
							targetTable.UpdatedDate = sourceTable.UpdatedDate
					WHEN NOT MATCHED THEN
					INSERT (Currency, Rate, Date, CreatedDate, UpdatedDate)
					VALUES (sourceTable.Currency, sourceTable.Rate, sourceTable.Date, sourceTable.Date, sourceTable.UpdatedDate);"), ct);

	public async Task<IEnumerable<CurrencyRate>> GetCurrencyRatesAsync(CancellationToken ct)
		=> await _context.CurrencyRates.ToListAsync(ct);
}
