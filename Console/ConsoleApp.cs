using GIC.Console.Services;

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
            
        }
    }
}
