using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WalletSystem.Domain.Entities;
using WalletSystem.Infrastructure.Repositories;

public class WalletService
{
    private readonly WalletRepository _walletRepo;
    private readonly TransactionRepository _txRepo;
    private readonly ILogger<WalletService> _logger;

    public WalletService(WalletRepository walletRepo, TransactionRepository txRepo, ILogger<WalletService> logger)
    {
        _walletRepo = walletRepo;
        _txRepo = txRepo;
        _logger = logger;
    }

    public async Task<Guid> CreateWalletAsync()
    {
        var wallet = new Wallet { Id = Guid.NewGuid(), Balance = 0 };
        await _walletRepo.AddAsync(wallet);
        _logger.LogInformation($"Created wallet {wallet.Id}");
        return wallet.Id;
    }

    public async Task<decimal> GetBalanceAsync(Guid walletId)
    {
        var wallet = await _walletRepo.GetByIdAsync(walletId);
        return wallet?.Balance ?? throw new InvalidOperationException("Wallet not found");
    }

    public async Task AddFundsAsync(Guid walletId, decimal amount, Guid txId)
    {
        if (await _txRepo.GetByIdAsync(txId) != null) return; // Prevent double-spending

        var wallet = await _walletRepo.GetByIdAsync(walletId);
        wallet.Balance += amount;

        await _walletRepo.UpdateAsync(wallet);
        await _txRepo.AddAsync(new WalletTransaction
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
        if (await _txRepo.GetByIdAsync(txId) != null) return; // Prevent double-spending

        var wallet = await _walletRepo.GetByIdAsync(walletId);
        if (wallet.Balance < amount) throw new InvalidOperationException("Insufficient balance");

        wallet.Balance -= amount;
        await _walletRepo.UpdateAsync(wallet);
        await _txRepo.AddAsync(new WalletTransaction
        {
            Id = txId,
            WalletId = walletId,
            Amount = -amount,
            CreatedAt = DateTime.UtcNow
        });

        _logger.LogInformation($"Removed {amount} from wallet {walletId}");
    }
}