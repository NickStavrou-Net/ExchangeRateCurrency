using ExchangeRateCurrency.Domain.Interfaces;
using MediatR;

namespace ExchangeRateCurrency.Application.Commands.Handlers;
public class CreateWalletHandler : IRequestHandler<CreateWalletCommand, long>
{
	private readonly IWalletService _walletService;

	public CreateWalletHandler(IWalletService walletService)
	{
		_walletService = walletService;
	}
	public async Task<long> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
	{
		return await _walletService.CreateWalletAsync(request.Currency, cancellationToken ,request.InitialBalance);
	}
}
