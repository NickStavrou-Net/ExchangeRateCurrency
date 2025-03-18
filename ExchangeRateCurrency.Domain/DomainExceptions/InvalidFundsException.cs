namespace ExchangeRateCurrency.Domain.DomainExceptions;
public class InvalidFundsException : Exception
{
	public InvalidFundsException() : base()
	{
	}

	public InvalidFundsException(string? message) : base(message)
	{
	}

	public InvalidFundsException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
