using ExchangeRateCurrency.Domain.DomainExceptions;
using ExchangeRateCurrency.Domain.Entities;
using ExchangeRateCurrency.Domain.Interfaces;

namespace ExchangeRateCurrency.Application.Services;
public class WalletService : IWalletService
{
	private readonly IWalletRepository _walletRepository;
	private readonly IWalletStrategyResolver _walletStrategyResolver;

	public WalletService(IWalletRepository walletRepository,
		IWalletStrategyResolver walletStrategyResolver)
	{
		_walletRepository = walletRepository;
		_walletStrategyResolver = walletStrategyResolver;
	}

	public async Task AdjustWalletBalanceAsync(long walletId, decimal amount, string strategyName, CancellationToken cancellationToken)
	{
		var wallet = await _walletRepository.GetByIdAsync(walletId, cancellationToken);
		if (wallet == null)
			throw new WalletNotFoundException($"Wallet with  id '{walletId}' not found.");

		var strategy = _walletStrategyResolver.Resolve(strategyName);
		await strategy.AdjustBalance(wallet, amount);

		await _walletRepository.UpdateAsync(wallet, cancellationToken);
	}

	public async Task<long> CreateWalletAsync(string currency, CancellationToken cancellationToken, decimal initialBalance = 0)
	{
		var newWallet = new Wallet(initialBalance, currency);
		await _walletRepository.CreateAsync(newWallet, cancellationToken);
		return newWallet.Id;
	}

	public async Task<decimal> GetBalanceAsync(long walletId, CancellationToken cancellationToken)
	{
		var wallet = await _walletRepository.GetByIdAsync(walletId, cancellationToken);
		if (wallet == null)
			throw new WalletNotFoundException($"Wallet with  id '{walletId}' not found.");

		return wallet.Balance;
	}

	public async Task ForceSubtractFundsAsync(long walletId, decimal amount, CancellationToken cancellationToken)
	{
		if (amount <= 0)
			throw new InvalidFundsException("Amount must be positive.");

		var wallet = await _walletRepository.GetByIdAsync(walletId, cancellationToken);
		if (wallet is null)
			throw new WalletNotFoundException($"Wallet with '{walletId}' does not exist!");

		wallet?.ForceSubtractFunds(amount);

		await _walletRepository.UpdateAsync(wallet!, cancellationToken);
	}

	public async Task AddFundsAsync(long walletId, decimal amount, CancellationToken cancellationToken)
	{
		if (amount <= 0)
			throw new InvalidFundsException("Amount must be positive.");

		var wallet = await _walletRepository.GetByIdAsync(walletId, cancellationToken);

		if (wallet is null)
			throw new WalletNotFoundException($"Wallet with '{walletId}' does not exist!");

		wallet.AddFunds(amount);

		await _walletRepository.UpdateAsync(wallet, cancellationToken);
	}

	public async Task SubtractFundsAsync(long walletId, decimal amount, CancellationToken cancellationToken)
	{
		if (amount <= 0) throw new InvalidFundsException("Amount must be positive.");

		var wallet = await _walletRepository.GetByIdAsync(walletId, cancellationToken);

		if (wallet is null)
			throw new WalletNotFoundException($"Wallet with '{walletId}' does not exist!");

		if (amount > wallet?.Balance) throw new InvalidFundsException("Insufficient funds.");

		wallet?.SubtractFunds(amount);

		await _walletRepository.UpdateAsync(wallet!, cancellationToken);
	}
}
