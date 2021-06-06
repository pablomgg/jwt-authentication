using Infrastructure.Application.Providers;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Application.Settings
{
    public class AppSettings
    {
        public TokenConfigurationProvider TokenConfiguration { get; set; }
        public SwaggerProvider Swagger { get; set; }

        public AppSettings() { }

        public AppSettings(IConfiguration configuration)
        {
            TokenConfiguration = new TokenConfigurationProvider(configuration);
            Swagger = new SwaggerProvider(configuration);
        }
    }
}