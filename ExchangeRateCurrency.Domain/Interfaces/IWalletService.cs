namespace ExchangeRateCurrency.Domain.Interfaces;
public interface IWalletService
{
	Task<long> CreateWalletAsync(string currency, CancellationToken cancellationToken, decimal initialBalance = 0);
	Task<decimal> GetBalanceAsync(long walletId, CancellationToken cancellationToken);
	Task AdjustWalletBalanceAsync(long walletId, decimal amount, string strategyName, CancellationToken cancellationToken);

	Task AddFundsAsync(long walletId, decimal amount, CancellationToken cancellationToken);
	Task SubtractFundsAsync(long walletId, decimal amount, CancellationToken cancellationToken);
	Task ForceSubtractFundsAsync(long walletId, decimal amount, CancellationToken cancellationToken);
}
