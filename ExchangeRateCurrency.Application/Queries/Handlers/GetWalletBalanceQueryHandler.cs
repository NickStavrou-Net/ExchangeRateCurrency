using ExchangeRateCurrency.Domain.Interfaces;
using MediatR;

namespace ExchangeRateCurrency.Application.Queries.Handlers;
public class GetWalletBalanceQueryHandler : IRequestHandler<GetWalletBalanceQuery, decimal>
{
	private readonly IWalletService _walletService;
	public GetWalletBalanceQueryHandler(IWalletService walletService)
	{
		_walletService = walletService;
	}
	public async Task<decimal> Handle(GetWalletBalanceQuery request, CancellationToken cancellationToken)
	{
		return await _walletService.GetBalanceAsync(request.WalletId, cancellationToken);
	}
}
