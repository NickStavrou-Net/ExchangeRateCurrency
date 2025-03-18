namespace ExchangeRateCurrency.Domain.Entities;
public class Wallet : BaseEntity
{
	public long Id { get; private set; }
	public decimal Balance { get; private set; }
	public string Currency { get; set; } = string.Empty;

	public Wallet(decimal balance, string currency)
	{
		Balance = balance;
		Currency = currency;
	}
	public Wallet() { }

	public void SetBalance(decimal balance)
	{
		Balance = balance;
	}
	public void AddFunds(decimal amount)
	{
		Balance += amount;
	}

	public void SubtractFunds(decimal amount)
	{
		Balance -= amount;
	}

	public void ForceSubtractFunds(decimal amount)
	{
		Balance -= amount; // negative balance is allowed in this case
	}
}
