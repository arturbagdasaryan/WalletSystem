using System.Web.Http;
using Swashbuckle.Application;

namespace WalletSystem.Api.App_Start
{
    public static class SwaggerConfig
    {
        public static void Register()
        {
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Wallet API");
                })
                .EnableSwaggerUi();
        }
    }
}