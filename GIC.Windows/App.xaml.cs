using GIC.Common.Services;
using GIC.RestApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace GIC.Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        private IConfigurationRoot configuration;
        internal static string[] Arguments;

        public App()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();

            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton(configuration);
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<MainWindow>();
        }

        protected void OnStartup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
                Arguments = e.Args;

            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
