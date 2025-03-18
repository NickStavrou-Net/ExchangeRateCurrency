using MediatR;

namespace ExchangeRateCurrency.Application.Commands;
public class AdjustWalletBalanceCommand : IRequest
{
	public long WalletId { get; set; }
	public decimal Amount { get; set; }
	public string Currency { get; set; } = string.Empty;
	public string StrategyName { get; set; } = string.Empty;
}
