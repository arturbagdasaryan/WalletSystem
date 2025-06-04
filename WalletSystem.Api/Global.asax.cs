using Microsoft.EntityFrameworkCore;
using System.Web.Http;
using WalletSystem.Api.App_Start;
using WalletSystem.Infrastructure.Data;
using WalletSystem.Infrastructure.Repositories;

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

            GlobalConfiguration.Configuration.DependencyResolver = new DependencyResolver.DependencyResolver(walletService);
        }
    }
}