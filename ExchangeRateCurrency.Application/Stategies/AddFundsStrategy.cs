using ExchangeRateCurrency.Domain.Entities;
using ExchangeRateCurrency.Domain.Interfaces;

namespace ExchangeRateCurrency.Application.Stategies;
public class AddFundsStrategy : IWalletStrategyAdjustments
{
	private readonly IWalletService _walletService;
	public string StrategyName => "AddFundsStrategy";

	public AddFundsStrategy(IWalletService walletService)
	{
		_walletService = walletService;
	}

	public async Task AdjustBalance(Wallet wallet, decimal amount, CancellationToken cancellationToken)
	{
		await _walletService.AddFundsAsync(wallet.Id, amount, cancellationToken);
	}
}