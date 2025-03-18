using ExchangeRateCurrency.Domain.Entities;

namespace ExchangeRateCurrency.Domain.Interfaces;
public interface IWalletRepository
{
	Task<Wallet?> GetByIdAsync(long id, CancellationToken cancellationToken);
	Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken);
	Task CreateAsync(Wallet wallet, CancellationToken cancellationToken);
}
