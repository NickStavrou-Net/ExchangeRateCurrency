using ExchangeRateCurrency.Application.Jobs;
using ExchangeRateCurrency.Application.Options;
using ExchangeRateCurrency.Domain.Interfaces;
using ExchangeRateCurrency.GatewayLibrary.Services;
using Microsoft.Extensions.Options;

namespace ExchangeRateCurrency.API;

public static class Extensions
{
	public static IServiceCollection AddRateService(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<BaseUrlOptions>(configuration.GetSection("BaseUrlOptions"));

		services.AddHttpClient("EcbClient", (serviceProvider, client) =>
		{
			var baseUrlOptions = serviceProvider.GetRequiredService<IOptions<BaseUrlOptions>>().Value;

			if (string.IsNullOrWhiteSpace(baseUrlOptions.BaseUrl))
			{
				throw new InvalidOperationException("ECB Base URL is not configured properly.");
			}

			client.DefaultRequestHeaders.Add("User-Agent", "ECB-Rate-Client/1.0");

			// Explicitly request XML format as the ECB base url is an xml
			client.DefaultRequestHeaders.Add("Accept", "application/xml");

			client.BaseAddress = new Uri(baseUrlOptions.BaseUrl);
		});

		services.AddScoped<IEcbRateService, EcbRateService>();

		return services;
	}

	public static IServiceCollection AddExchangeRateCurrencyJob(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<CronOptions>(configuration.GetSection("CronOptions"));

		services.AddScoped<IExchangeRateCurrencyService, ExchangeRateCurrencyService>();

		services.AddHostedService<ExchangeRateCurrencyJob>();

		return services;
	}
}
