using System;
using System.Threading.Tasks;
using WalletSystem.Domain.Entities;
using WalletSystem.Infrastructure.Data;

namespace WalletSystem.Infrastructure.Repositories
{
    public class TransactionRepository
    {
        private readonly WalletDbContext _context;
        public TransactionRepository(WalletDbContext context) => _context = context;

        public Task<WalletTransaction> GetByIdAsync(Guid id) => _context.WalletTransactions.FindAsync(id).AsTask();
        public async Task AddAsync(WalletTransaction tx)
        {
            _context.WalletTransactions.Add(tx);
            await _context.SaveChangesAsync();
        }
    }
}