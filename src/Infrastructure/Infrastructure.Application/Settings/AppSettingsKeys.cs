namespace Infrastructure.Application.Settings
{
    public struct AppSettingsKeys
    {
        public struct Swagger
        {
            public const string Version = "Swagger:Version";
            public const string Title = "Swagger:Title";
            public const string Description = "Swagger:Description";
            public const string Name = "Swagger:Name";
            public const string Url = "Swagger:Url";
        }

        public struct TokenConfiguration
        {
            public const string Expiration = "TokenConfigurations:Expiration";
            public const string Secret = "TokenConfigurations:Secret";
        }
    }
}