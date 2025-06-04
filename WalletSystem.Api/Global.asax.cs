using Microsoft.EntityFrameworkCore;
using System.Web.Http;
using Unity.Lifetime;
using Unity;
using Unity.WebApi;
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

            var container = new UnityContainer();

            // Register DbContext
            container.RegisterFactory<WalletDbContext>(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<WalletDbContext>();
                optionsBuilder.UseInMemoryDatabase("WalletDb");
                return new WalletDbContext(optionsBuilder.Options);
            }, new TransientLifetimeManager());

            // Register repositories and service
            container.RegisterType<WalletRepository>();
            container.RegisterType<TransactionRepository>();
            container.RegisterType<WalletService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}