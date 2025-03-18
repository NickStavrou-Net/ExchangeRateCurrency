using ExchangeRateCurrency.Domain.Entities;
using ExchangeRateCurrency.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateCurrency.API.Controllers;

[Route("api/exchange-rate-currency")]
[ApiController]
public class ExchangeRateCurrencyController : ControllerBase
{
	private readonly IEcbRateService _ecbRateService;
	private readonly ILogger<ExchangeRateCurrencyController> _logger;

	public ExchangeRateCurrencyController(IEcbRateService ecbRateService,
		ILogger<ExchangeRateCurrencyController> logger)
	{
		_ecbRateService = ecbRateService;
		_logger = logger;
	}

	[HttpGet("rates")]
	public async Task<ActionResult<IEnumerable<CurrencyRate>>> GetAllRates(CancellationToken token)
	{
		try
		{
			_logger.LogInformation("Fetching all currency rates");
			var rates = await _ecbRateService.GetCurrencyRatesAsync(token);
			return Ok(rates);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error fetching currency rates");
			return StatusCode(500, new { error = ex.Message });
		}
	}
}
