using Infrastructure.Identity.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity
{
    public static class Register
    {
        public static IServiceCollection RegisterAuthentication(this IServiceCollection services)
        {
            services.AddSingleton<IAuthentication, Authentication>(); 
            return services;
        }
    }
}
