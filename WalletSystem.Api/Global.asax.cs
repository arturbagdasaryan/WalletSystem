using Microsoft.EntityFrameworkCore;
using System.Web.Http;
using Unity.Lifetime;
using Unity;
using Unity.WebApi;
using WalletSystem.Api.App_Start;
using WalletSystem.Infrastructure.Data;
using WalletSystem.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using System;
using WalletSystem.Api.Controllers;

namespace WalletSystem.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            SwaggerConfig.Register();

            // DI setup
            var builder = new DbContextOptionsBuilder<WalletDbContext>();
            builder.UseInMemoryDatabase("WalletDb");

            var dbContext = new WalletDbContext(builder.Options);
            var walletRepository = new WalletRepository(dbContext);
            var transactionRepository = new TransactionRepository(dbContext);
            var walletService = new WalletService(walletRepository, transactionRepository);

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorResolver(walletService);
        }
    }
    public class SimpleInjectorResolver : IDependencyResolver
    {
        private readonly WalletService _walletService;

        public SimpleInjectorResolver(WalletService walletService)
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