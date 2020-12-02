﻿using GIC.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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

            if (args.Contains("--console"))
            {
                ConsoleApp(services, args);
            } else if (args.Contains("--web"))
            {
                RestApi(args);
            }
            else
            {
                Gui();
            }
        }

        private static void Gui()
        {
            var application = new App();
            application.Run(new MainWindow());
        }

        private static void RestApi(string[] args)
        {
            GIC.RestApi.Program.Main(args);
        }

        private static void ConsoleApp(IServiceCollection services, string[] args)
        {
            services.AddTransient<ConsoleApp>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ConsoleApp>().Run(args);
        }
    }
}