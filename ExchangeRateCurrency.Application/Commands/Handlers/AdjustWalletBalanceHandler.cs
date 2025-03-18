using ExchangeRateCurrency.Domain.Interfaces;
using MediatR;

namespace ExchangeRateCurrency.Application.Commands.Handlers;
public class AdjustWalletBalanceHandler : IRequestHandler<AdjustWalletBalanceCommand>
{
	private readonly IWalletService _walletService;

	public AdjustWalletBalanceHandler(IWalletService walletService)
	{
		_walletService = walletService;
	}

	public async Task Handle(AdjustWalletBalanceCommand request, CancellationToken cancellationToken)
	{
		await _walletService.AdjustWalletBalanceAsync(request.WalletId, request.Amount, request.StrategyName, cancellationToken);
	}
}
