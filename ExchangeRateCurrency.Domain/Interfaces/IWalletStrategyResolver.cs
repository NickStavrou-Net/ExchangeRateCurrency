namespace ExchangeRateCurrency.Domain.Interfaces;
public interface IWalletStrategyResolver
{
	IWalletStrategyAdjustments Resolve(string strategyName);
}
