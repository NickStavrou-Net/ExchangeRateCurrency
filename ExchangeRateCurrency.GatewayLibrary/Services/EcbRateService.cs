using ExchangeRateCurrency.Domain.Entities;
using ExchangeRateCurrency.Domain.Interfaces;
using ExchangeRateCurrency.GatewayLibrary.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;

namespace ExchangeRateCurrency.GatewayLibrary.Services;
public class EcbRateService : IEcbRateService
{
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly IMemoryCache _cache;
	private readonly IServiceProvider _serviceProvider;
	private static readonly XNamespace EcbNamespace = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref";

	public EcbRateService(IHttpClientFactory httpClientFactory,
		IMemoryCache cache,
		IServiceProvider serviceProvider)
	{
		_httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
		_cache = cache;
		_serviceProvider = serviceProvider;
	}

	public async Task<IEnumerable<CurrencyRate>> GetCurrencyRatesAsync(CancellationToken ct)
	{
		if (_cache.TryGetValue("CurrencyRates", out IEnumerable<CurrencyRate>? cachedRates))
		{
			return cachedRates ?? [];
		}

		//not found in cache, fetch from ECB
		try
		{
			var client = _httpClientFactory.CreateClient("EcbClient");

			var xmlResponse = await client.GetStringAsync(string.Empty);

			var rates = ParseXmlResponse(xmlResponse);

			_cache.Set("CurrencyRates", rates, TimeSpan.FromMinutes(1));

			return rates;
		}
		catch (HttpRequestException ex)
		{

			throw new EcbException(ex.Message);
		}

	}
	public async Task<IEnumerable<CurrencyRate>> GetCachedRatesAsync(CancellationToken ct)
	{
		if (_cache.TryGetValue("CurrencyRates", out IEnumerable<CurrencyRate>? cachedCurrencyRates))
		{
			return cachedCurrencyRates ?? Array.Empty<CurrencyRate>();
		}

		using (var scope = _serviceProvider.CreateScope())
		{
			var repository = scope.ServiceProvider.GetRequiredService<IExchangeRateCurrrencyRepository>();
			var currencyRates = await repository.GetCurrencyRatesAsync(ct);

			_cache.Set("CurrencyRates", currencyRates, TimeSpan.FromMinutes(1));
			return currencyRates;
		}
	}
	private static IEnumerable<CurrencyRate> ParseXmlResponse(string xmlResponse)
	{
		try
		{
			XDocument doc = XDocument.Parse(xmlResponse);

			var cubeXmlElement = doc.Descendants(EcbNamespace + "Cube")
				.Elements(EcbNamespace + "Cube")
				.FirstOrDefault();

			if (cubeXmlElement == null)
			{
				throw new EcbException("Cube element not found in the XML response");
			}

			if (!DateTime.TryParse(cubeXmlElement.Attribute("time")?.Value, out DateTime time))
			{
				throw new EcbException("Failed to parse time attribute from the XML response");
			}

			var allRateList = cubeXmlElement.Elements(EcbNamespace + "Cube")
				.Select(x => new CurrencyRate
				{
					Currency = x.Attribute("currency")?.Value ?? string.Empty,
					Rate = decimal.Parse(x.Attribute("rate")?.Value ?? "0"),
					Date = time
				}).ToList();

			return allRateList;
		}
		catch (Exception ex) when (ex is not EcbException)
		{
			throw new EcbException("Failed to parse ECB XML response");
		}
	}
}
