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
            RestApi(args);
        }

        private void RestApi(string[] args)
        {
            GIC.RestApi.Program.Main(args);
        }
    }
}
