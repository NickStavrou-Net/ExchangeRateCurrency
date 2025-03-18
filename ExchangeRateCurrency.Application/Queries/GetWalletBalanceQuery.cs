using MediatR;

namespace ExchangeRateCurrency.Application.Queries;
public class GetWalletBalanceQuery : IRequest<decimal>
{
	public long WalletId { get; set; }
	public string Currency { get; set; } = string.Empty;
}
