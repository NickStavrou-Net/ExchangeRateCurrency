using MediatR;

namespace ExchangeRateCurrency.Application.Commands;
public class CreateWalletCommand : IRequest<long>
{
	public string Currency { get; set; } = string.Empty;
	public decimal InitialBalance { get; set; } = 0;
}
