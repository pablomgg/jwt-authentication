using Infrastructure.Application.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection SwaggerConfigure(this IServiceCollection services, SwaggerProvider provider)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc(provider.Version, new OpenApiInfo
                {
                    Title = provider.Title,
                    Version = provider.Version,
                    Description = provider.Description
                });

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }

        public static IServiceCollection MvcConfigure(this IServiceCollection services, string myAllowSpecificOrigins)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            services
                .AddAuthorization();

            services
                .AddCors(opt =>
                {
                    opt
                        .AddPolicy(myAllowSpecificOrigins,
                            cor =>
                            {
                                cor.WithOrigins(new string[]
                                {
                                    "http://localhost:4200"
                                });
                                cor.AllowAnyHeader();
                                cor.AllowAnyMethod();
                                cor.AllowCredentials();
                            });
                });
             
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }

        public static IServiceCollection AuthenticationConfigure(this IServiceCollection services, TokenConfigurationProvider provider)
        {
            var key = Encoding.UTF8.GetBytes(provider.Secret);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            return services;
        }
    }
}