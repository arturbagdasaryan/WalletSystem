using System;

namespace WalletSystem.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
    }
}