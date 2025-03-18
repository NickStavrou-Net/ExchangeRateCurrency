namespace ExchangeRateCurrency.GatewayLibrary.Exceptions;
public class EcbException : Exception
{
	public string ExceptionValue { get; set; } = string.Empty;

	public EcbException() { }

	public EcbException(string exceptionValue)
		: base($"Failed to get response with message: {exceptionValue}")
	{
		ExceptionValue = exceptionValue;
	}

	public EcbException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
