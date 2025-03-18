using ExchangeRateCurrency.Domain.Entities;
using ExchangeRateCurrency.Domain.Interfaces;

namespace ExchangeRateCurrency.Application.Stategies;
public class SubtractFundsStrategy : IWalletStrategyAdjustments
{
	private readonly IWalletService _walletService;

	public string StrategyName => "SubtractFundsStrategy";

	public SubtractFundsStrategy(IWalletService walletService)
	{
		_walletService = walletService;
	}

	public async Task AdjustBalance(Wallet wallet, decimal amount, CancellationToken cancellationToken)
	{
		await _walletService.SubtractFundsAsync(wallet.Id, amount, cancellationToken);
	}
}
