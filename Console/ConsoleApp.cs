using GIC.Common.Services;
using GIC.Wpf;
using System;
using System.Linq;

namespace GIC.Console
{
    class ConsoleApp
    {
        private IConfigurationService configurationService;

        public ConsoleApp(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public void Run(string[] args)
        {
            if (args.Contains("--console"))
            {
                MainMenu();
            }
            else if (args.Contains("--web"))
            {
                RestApi(args);
            }
            else
            {
                Gui();
            }
        }

        private void MainMenu()
        {
            throw new NotImplementedException();
        }

        private void Gui()
        {
            System.Console.WriteLine("Now starting GICS GUI, please wait.  Server output will be displayed here");
            var application = new App();
            application.Run(new MainWindow(configurationService));
        }

        private void RestApi(string[] args)
        {
            GIC.RestApi.Program.Main(args);
        }
    }
}
