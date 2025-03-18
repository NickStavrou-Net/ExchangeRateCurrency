using ExchangeRateCurrency.Domain.Entities;
using ExchangeRateCurrency.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ExchangeRateCurrency.Application.Jobs;
public class ExchangeRateCurrencyService : IExchangeRateCurrencyService
{
	private readonly IEcbRateService _ecbRateService;
	private readonly ILogger<ExchangeRateCurrencyService> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly IExchangeRateCurrrencyRepository _exchangeRateCurrrencyRepository;
	private readonly IDatabaseConnection _databaseConnection;

	public ExchangeRateCurrencyService(IEcbRateService ecbRateService,
		ILogger<ExchangeRateCurrencyService> logger,
		IServiceProvider serviceProvider,
		IExchangeRateCurrrencyRepository exchangeRateCurrrencyRepository,
		IDatabaseConnection databaseConnection)
	{
		_ecbRateService = ecbRateService;
		_logger = logger;
		_serviceProvider = serviceProvider;
		_exchangeRateCurrrencyRepository = exchangeRateCurrrencyRepository;
		_databaseConnection = databaseConnection;
	}

	public async Task ExecuteAsync(CancellationToken ct)
	{
		try
		{
			_logger.LogInformation("start fetching currency rates");
			var currencyRates = await _ecbRateService.GetCurrencyRatesAsync(ct);

			if (currencyRates == null || !currencyRates.Any())
			{
				_logger.LogWarning("No currency rates found");
				return;
			}

			await UpdateDatabaseRatesAsync(currencyRates, ct);

			_logger.LogInformation("Currency rate update job completed successfully");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error executing currency rate update job");
			throw;
		}
	}

	public async Task<bool> UpdateDatabaseRatesAsync(IEnumerable<CurrencyRate> currencyRates, CancellationToken ct)
	{
		using (var scope = _serviceProvider.CreateScope())
		{
			string tableName = "TempCurrencyRates_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
			SqlConnection? connection = null;
			bool wasOpen = false;

			try
			{
				_logger.LogInformation("Starting currency rate update. TempTable: {TableName}, RateCount: {RateCount}",
									   tableName, currencyRates.Count());

				// Step 1: Create the temporary table
				await _exchangeRateCurrrencyRepository.ExecuteCreateTempTableSqlAsync(tableName, ct);

				// Step 2: Bulk insert data into the temporary table
				var dataTable = ToDataTable(currencyRates);

				connection = _databaseConnection.GetSqlConnection();
				wasOpen = connection.State == ConnectionState.Open;

				if (!wasOpen)
					await connection.OpenAsync(ct);

				using (var bulkCopy = new SqlBulkCopy(connection))
				{
					bulkCopy.DestinationTableName = tableName;
					await bulkCopy.WriteToServerAsync(dataTable, ct);
				}

				_logger.LogInformation("Bulk insert completed. Performing MERGE operation.");

				// Step 3: Perform the MERGE operation between the temporary table and the CurrencyRates table
				await _exchangeRateCurrrencyRepository.ExecuteMergeSqlAsync(tableName, ct);

				_logger.LogInformation("Merge operation completed successfully.");
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating currency rates. TempTable: {TableName}, RateCount: {RateCount}",
								 tableName, currencyRates.Count());
				return false;
			}
			finally
			{
				if (connection != null)
				{
					try
					{
						using (var command = connection.CreateCommand())
						{
							command.CommandText = $"IF OBJECT_ID('{tableName}', 'U') IS NOT NULL DROP TABLE {tableName}";
							await command.ExecuteNonQueryAsync(ct);
						}

						_logger.LogInformation("Temporary table {TableName} dropped.", tableName);
					}
					catch (Exception dropEx)
					{
						_logger.LogError(dropEx, "Error dropping temporary table {TableName}.", tableName);
					}

					if (!wasOpen && connection.State == ConnectionState.Open)
						await connection.CloseAsync();

					//explicitly dispose the connection
					await connection.DisposeAsync();
				}
			}
		}
	}



	private static DataTable ToDataTable(IEnumerable<CurrencyRate> rates)
	{
		var table = new DataTable();
		table.Columns.Add("Currency", typeof(string));
		table.Columns.Add("Rate", typeof(decimal));
		table.Columns.Add("Date", typeof(DateTime));
		table.Columns.Add("UpdatedDate", typeof(DateTime)); // Include UpdatedDate for comparison

		foreach (var rate in rates)
		{
			table.Rows.Add(rate.Currency, rate.Rate, rate.Date, rate.UpdatedDate);
		}

		return table;
	}

}