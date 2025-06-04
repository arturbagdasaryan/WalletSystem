using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;
using System;
using System.Web.Http;
using WalletSystem.Api.App_Start;
using WalletSystem.Infrastructure.Data;
using WalletSystem.Infrastructure.Repositories;

namespace WalletSystem.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static ILoggerFactory LoggerFactory;
        public static ILogger<WalletService> Logger;
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            SwaggerConfig.Register();

            var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "log.txt");
            Console.WriteLine($"Serilog log file path: {logPath}");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Setup LoggerFactory
            LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
            });

            Logger = LoggerFactory.CreateLogger<WalletService>();

            // DI setup
            var builder = new DbContextOptionsBuilder<WalletDbContext>();
            builder.UseInMemoryDatabase("WalletDb");

            var dbContext = new WalletDbContext(builder.Options);
            var walletRepository = new WalletRepository(dbContext);
            var transactionRepository = new TransactionRepository(dbContext);
            var walletService = new WalletService(walletRepository, transactionRepository, Logger);

            GlobalConfiguration.Configuration.DependencyResolver = new DependencyResolver.DependencyResolver(walletService);
        }
    }
}