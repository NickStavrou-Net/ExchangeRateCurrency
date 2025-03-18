using ExchangeRateCurrency.Application.Commands;
using ExchangeRateCurrency.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateCurrency.API.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class WalletsController : ControllerBase
{
	private readonly IMediator _mediator;
	public WalletsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost()]
	public async Task<IActionResult> CreateWallet([FromBody] CreateWalletCommand command)
	{
		var walletId = await _mediator.Send(command);
		return CreatedAtAction(nameof(GetBalance), new { walletId }, walletId);
	}
	[HttpPost("{walletId}/adjust-balance")]
	public async Task<IActionResult> AdjustBalance([FromRoute] long walletId,
		[FromQuery] decimal amount,
		[FromQuery] string currency,
		[FromQuery] string strategyName)
	{
		var command = new AdjustWalletBalanceCommand
		{
			WalletId = walletId,
			Amount = amount,
			Currency = currency,
			StrategyName = strategyName
		};

		await _mediator.Send(command);
		return Ok();
	}
	[HttpGet("{walletId}")]
	public async Task<IActionResult> GetBalance([FromRoute] long walletId, [FromQuery] string currency)
	{
		var query = new GetWalletBalanceQuery
		{
			WalletId = walletId,
			Currency = currency
		};
		var balance = await _mediator.Send(query);
		return Ok(balance);
	}
}
