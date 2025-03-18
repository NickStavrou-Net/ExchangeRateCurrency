using ExchangeRateCurrency.Domain.Entities;
using ExchangeRateCurrency.Domain.Interfaces;

namespace ExchangeRateCurrency.Application.Stategies;
public class ForceSubtractFundsStrategy : IWalletStrategyAdjustments
{
	private readonly IWalletService _walletService;
	public string StrategyName => "ForceSubtractFundsStrategy";

	public ForceSubtractFundsStrategy(IWalletService walletService)
	{
		_walletService = walletService;
	}

	public async Task AdjustBalance(Wallet wallet, decimal amount, CancellationToken cancellationToken)
	{
		await _walletService.ForceSubtractFundsAsync(wallet.Id, amount, cancellationToken);
	}
}
