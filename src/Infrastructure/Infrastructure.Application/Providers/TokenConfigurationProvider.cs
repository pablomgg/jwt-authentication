using System;
using Infrastructure.Application.Settings;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Application.Providers
{
    public class TokenConfigurationProvider
    {
        public string Secret { get; }
        public int Expiration { get; }

        public TokenConfigurationProvider(IConfiguration configuration)
        {
            Secret = configuration.GetSection(AppSettingsKeys.TokenConfiguration.Secret)?.Value;
            Expiration = Convert.ToInt32(configuration.GetSection(AppSettingsKeys.TokenConfiguration.Expiration)?.Value);
        }
    }
}