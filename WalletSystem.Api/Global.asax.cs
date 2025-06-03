using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

            var services = new ServiceCollection();
            services.AddDbContext<WalletDbContext>(opt => opt.UseInMemoryDatabase("WalletDb"));
            services.AddScoped<WalletRepository>();
            services.AddScoped<TransactionRepository>();
            services.AddScoped<WalletService>();
            services.AddLogging(builder => builder.AddConsole());

            var provider = services.BuildServiceProvider();
            GlobalConfiguration.Configuration.DependencyResolver = new DefaultDependencyResolver(provider);
        }
    }
}