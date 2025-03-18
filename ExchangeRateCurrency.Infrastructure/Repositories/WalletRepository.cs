using ExchangeRateCurrency.Domain.Entities;
using ExchangeRateCurrency.Domain.Interfaces;
using ExchangeRateCurrency.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRateCurrency.Infrastructure.Repositories;
public class WalletRepository : IWalletRepository
{
	private readonly ExchageRateCurrencyDbContext _context;

	public WalletRepository(ExchageRateCurrencyDbContext context)
	{
		_context = context;
	}

	public async Task CreateAsync(Wallet wallet, CancellationToken cancellationToken)
	{
		_context.Wallets.Add(wallet);
		await _context.SaveChangesAsync();
	}

	public async Task<Wallet?> GetByIdAsync(long id, CancellationToken cancellationToken)
		=> await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

	public async Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken)
	{
		_context.Wallets.Update(wallet);
		await _context.SaveChangesAsync(cancellationToken);
	}
}
