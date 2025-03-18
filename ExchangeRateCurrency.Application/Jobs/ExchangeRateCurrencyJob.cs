using ExchangeRateCurrency.Application.Options;
using ExchangeRateCurrency.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NCrontab;

namespace ExchangeRateCurrency.Application.Jobs;
public class ExchangeRateCurrencyJob : BackgroundService
{
	private readonly ILogger<ExchangeRateCurrencyJob> _logger;
	private readonly IOptions<CronOptions> _cronOptions;
	private readonly CrontabSchedule _schedule;
	private DateTime _nextRun;
	private readonly IServiceProvider _serviceProvider;

	public ExchangeRateCurrencyJob(
		ILogger<ExchangeRateCurrencyJob> logger,
		IOptions<CronOptions> options,
		IMemoryCache cache,
		IServiceProvider serviceProvider)
	{
		_logger = logger;
		_cronOptions = options;
		var schedule = string.IsNullOrEmpty(_cronOptions.Value.CronExpression)
			? "*/1 * * * *"
			: _cronOptions.Value.CronExpression;
		_schedule = CrontabSchedule.Parse(schedule);
		_nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);
		_serviceProvider = serviceProvider;
	}


	protected override async Task ExecuteAsync(CancellationToken ct)
	{
		if (!_cronOptions.Value.Enabled)
		{
			_logger.LogInformation("ExchangeRateCurrencyJob is disabled");
			return;
		}

		while (!ct.IsCancellationRequested)
		{
			// Wait until the next scheduled run
			if (_nextRun > DateTime.UtcNow)
			{
				await Task.Delay(_nextRun - DateTime.UtcNow, ct);
			}

			_logger.LogInformation("ExchangeRateCurrencyJob running at: {time}", DateTimeOffset.Now);

			try
			{
				using (var scope = _serviceProvider.CreateScope())
				{
					var repository = scope.ServiceProvider.GetRequiredService<IExchangeRateCurrrencyRepository>();
					var service = scope.ServiceProvider.GetRequiredService<IExchangeRateCurrencyService>();

					await service.ExecuteAsync(ct);

					_logger.LogInformation("Currency Rates fetched at: {time}", DateTimeOffset.Now);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred during the exchange rate update.");
			}

			_nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);
		}
	}
}
