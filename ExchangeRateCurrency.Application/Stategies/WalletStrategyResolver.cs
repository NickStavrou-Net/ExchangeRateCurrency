using ExchangeRateCurrency.Domain.Interfaces;

namespace ExchangeRateCurrency.Application.Stategies;
public class WalletStrategyResolver : IWalletStrategyResolver
{
	private readonly Dictionary<string, IWalletStrategyAdjustments> _strategies;

	public WalletStrategyResolver(IEnumerable<IWalletStrategyAdjustments> strategies)
	{
		_strategies = strategies.ToDictionary(s => s.StrategyName, StringComparer.OrdinalIgnoreCase);
	}

	public IWalletStrategyAdjustments Resolve(string strategyName)
	{
		if (!_strategies.TryGetValue(strategyName, out var strategy))
		{
			throw new InvalidOperationException($"Unknown strategy: {strategyName}");
		}

		return strategy;
	}
}
