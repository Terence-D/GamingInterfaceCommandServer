using GIC.Console.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GIC.Console
{
    class Startup
    {
        private IConfigurationRoot configuration;

        public Startup()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(configuration);

            services.AddSingleton<IConfigurationService, ConfigurationService>();
        }
    }
}
