using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WalletSystem.Domain.Entities;
using WalletSystem.Infrastructure.Repositories;

public class WalletService
{
    private readonly WalletRepository _walletRepository;
    private readonly TransactionRepository _transactionRepository;
    private readonly ILogger<WalletService> _logger;

    public WalletService(WalletRepository walletRepository, TransactionRepository transactionRepository, ILogger<WalletService> logger)
    {
        _walletRepository = walletRepository;
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    public async Task<Guid> CreateWalletAsync()
    {
        var wallet = new Wallet { Id = Guid.NewGuid(), Balance = 0 };
        await _walletRepository.AddAsync(wallet);
        _logger.LogInformation($"Created wallet {wallet.Id}");
        return wallet.Id;
    }

    public async Task<decimal> GetBalanceAsync(Guid walletId)
    {
        var wallet = await _walletRepository.GetByIdAsync(walletId);
        return wallet?.Balance ?? throw new InvalidOperationException("Wallet not found");
    }

    public async Task AddFundsAsync(Guid walletId, decimal amount, Guid txId)
    {
        if (await _transactionRepository.GetByIdAsync(txId) != null) return; // Prevent double-spending

        var wallet = await _walletRepository.GetByIdAsync(walletId);
        wallet.Balance += amount;

        await _walletRepository.UpdateAsync(wallet);
        await _transactionRepository.AddAsync(new WalletTransaction
        {
            Id = txId,
            WalletId = walletId,
            Amount = amount,
            CreatedAt = DateTime.UtcNow
        });

        _logger.LogInformation($"Added {amount} to wallet {walletId}");
    }

    public async Task RemoveFundsAsync(Guid walletId, decimal amount, Guid txId)
    {
        if (await _transactionRepository.GetByIdAsync(txId) != null) return; // Prevent double-spending

        var wallet = await _walletRepository.GetByIdAsync(walletId);
        if (wallet.Balance < amount) throw new InvalidOperationException("Insufficient balance");

        wallet.Balance -= amount;
        await _walletRepository.UpdateAsync(wallet);
        await _transactionRepository.AddAsync(new WalletTransaction
        {
            Id = txId,
            WalletId = walletId,
            Amount = -amount,
            CreatedAt = DateTime.UtcNow
        });

        _logger.LogInformation($"Removed {amount} from wallet {walletId}");
    }
}