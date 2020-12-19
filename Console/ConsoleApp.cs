using GIC.Common.Services;
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
            if (args.Contains("--web"))
            {
                RestApi(args);
            } else
            {
                MainMenu();
            }
        }

        private void MainMenu()
        {
            throw new NotImplementedException();
        }

        private void RestApi(string[] args)
        {
            GIC.RestApi.Program.Main(args);
        }
    }
}
