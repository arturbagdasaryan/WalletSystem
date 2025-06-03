using Microsoft.EntityFrameworkCore;
using WalletSystem.Domain.Entities;

namespace WalletSystem.Infrastructure.Data
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options) { }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
    }
}