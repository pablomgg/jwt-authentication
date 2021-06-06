using Infrastructure.Application.Extensions;
using Infrastructure.Application.Settings;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private AppSettings _appSettings;
        private readonly string _myAllowSpecificOrigins = "AllowSpecificOriginsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _appSettings = new AppSettings(configuration);
        } 

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .SwaggerConfigure(_appSettings.Swagger)
                .MvcConfigure(_myAllowSpecificOrigins)
                .AuthenticationConfigure(_appSettings.TokenConfiguration)
                .RegisterAuthentication();

            services.Configure<AppSettings>(opt =>
            {
                opt.TokenConfiguration = _appSettings.TokenConfiguration;
                opt.Swagger = _appSettings.Swagger;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app
                    .UseDeveloperExceptionPage();
            }

            app
                .UseHttpsRedirection();

            app
                .UseRouting();

            app
                .UseCors(_myAllowSpecificOrigins);

            app
                .UseAuthentication()
                .UseAuthorization();

            app
                .UseSwagger()
                .UseSwaggerUI(cfg =>
                {
                    cfg.SwaggerEndpoint(_appSettings.Swagger.Url, _appSettings.Swagger.Name);
                    cfg.DocExpansion(DocExpansion.None);
                });

            app
                .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
