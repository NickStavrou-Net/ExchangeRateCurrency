using ExchangeRateCurrency.Domain.Entities;

namespace ExchangeRateCurrency.Domain.Interfaces;
public interface IWalletStrategyAdjustments
{
	Task AdjustBalance(Wallet wallet, decimal amount, CancellationToken cancellationToken);
	string StrategyName { get; }
}
