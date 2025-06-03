using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WalletSystem.Domain.Entities;
using WalletSystem.Infrastructure.Data;

namespace WalletSystem.Infrastructure.Repositories
{
    public class WalletRepository
    {
        private readonly WalletDbContext _context;
        public WalletRepository(WalletDbContext context) => _context = context;

        public Task<Wallet> GetByIdAsync(Guid id) => _context.Wallets.FindAsync(id).AsTask();
        public async Task AddAsync(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Wallet wallet)
        {
            _context.Entry(wallet).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}