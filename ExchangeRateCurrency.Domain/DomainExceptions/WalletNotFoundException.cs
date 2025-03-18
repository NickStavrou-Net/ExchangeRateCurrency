namespace ExchangeRateCurrency.Domain.DomainExceptions;
public class WalletNotFoundException : Exception
{
	public WalletNotFoundException() : base()
	{
	}

	public WalletNotFoundException(string? message) : base(message)
	{
	}

	public WalletNotFoundException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
