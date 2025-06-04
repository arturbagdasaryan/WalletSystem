using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using WalletSystem.Api.Controllers;

namespace WalletSystem.Api.DependencyResolver
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly WalletService _walletService;

        public DependencyResolver(WalletService walletService)
        {
            _walletService = walletService;
        }

        public IDependencyScope BeginScope() => this;
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(WalletController))
                return new WalletController(_walletService);
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType) => new List<object>();
        public void Dispose() { }
    }
}