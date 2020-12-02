using Microsoft.Extensions.DependencyInjection;
using System;

namespace GIC.Console
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Startup startup = new Startup();

            IServiceCollection services = new ServiceCollection();
            startup.ConfigureServices(services);

            services.AddTransient<ConsoleApp>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ConsoleApp>().Run(args);
        }
    }
}
