using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WalletSystem.Infrastructure.Data;
using WalletSystem.Infrastructure.Repositories;


namespace WalletSystem.Tests
{
    [TestClass]
    public class WalletSystemTests
    {
        private WalletDbContext _context;
        private WalletService _service;
        private Guid _walletId;

        [TestInitialize]
        public async Task Init()
        {
            var options = new DbContextOptionsBuilder<WalletDbContext>()
                .UseInMemoryDatabase("WalletDb")
                .Options;

            _context = new WalletDbContext(options);
            await _context.Database.EnsureCreatedAsync();

            var walletRepo = new WalletRepository(_context);
            var txRepo = new TransactionRepository(_context);
            _service = new WalletService(walletRepo, txRepo);

            _walletId = await _service.CreateWalletAsync();
        }

        [TestMethod]
        public async Task CreateWallet_ShouldStartWithZeroBalance()
        {
            var balance = await _service.GetBalanceAsync(_walletId);
            Assert.AreEqual(0m, balance);
        }

        [TestMethod]
        public async Task AddFunds_ShouldIncreaseBalance()
        {
            var txId = Guid.NewGuid();
            await _service.AddFundsAsync(_walletId, 100m, txId);
            var balance = await _service.GetBalanceAsync(_walletId);
            Assert.AreEqual(100m, balance);
        }

        [TestMethod]
        public async Task RemoveFunds_ShouldDecreaseBalance()
        {
            var addTxId = Guid.NewGuid();
            var removeTxId = Guid.NewGuid();
            await _service.AddFundsAsync(_walletId, 200m, addTxId);
            await _service.RemoveFundsAsync(_walletId, 50m, removeTxId);
            var balance = await _service.GetBalanceAsync(_walletId);
            Assert.AreEqual(150m, balance);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task RemoveFunds_ThrowsOnInsufficientBalance()
        {
            var txId = Guid.NewGuid();
            await _service.RemoveFundsAsync(_walletId, 100m, txId);
        }

        [TestMethod]
        public async Task AddFunds_IsIdempotent_WithSameTransaction()
        {
            var txId = Guid.NewGuid();
            await _service.AddFundsAsync(_walletId, 100m, txId);
            await _service.AddFundsAsync(_walletId, 100m, txId); // Idempotent
            var balance = await _service.GetBalanceAsync(_walletId);
            Assert.AreEqual(100m, balance);
        }

        [TestMethod]
        public async Task RemoveFunds_IsIdempotent_WithSameTransaction()
        {
            var addTxId = Guid.NewGuid();
            var removeTxId = Guid.NewGuid();
            await _service.AddFundsAsync(_walletId, 100m, addTxId);
            await _service.RemoveFundsAsync(_walletId, 50m, removeTxId);
            await _service.RemoveFundsAsync(_walletId, 50m, removeTxId); // Idempotent
            var balance = await _service.GetBalanceAsync(_walletId);
            Assert.AreEqual(50m, balance);
        }
    }
}