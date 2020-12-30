using CommandLine;
using GIC.Common;
using GIC.Common.Services;
using System;
using System.Collections.Generic;

namespace GIC.Linux
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
            Parser.Default.ParseArguments<CommandLineParameters>(args)
                .WithParsed(RestApi)
                .WithNotParsed(HandleParseError);
        }

        private void RestApi(CommandLineParameters opts)
        {
            if (string.IsNullOrEmpty(opts.Password))
            {
                opts.Password = configurationService.Password;
            }
            else
            {
                configurationService.Password = opts.Password;
            }
            if (opts.Port == 0)
            {
                opts.Port = configurationService.Port;
            }
            else
            {
                configurationService.Port = opts.Port;
            }
            GIC.RestApi.Program.Start(opts);
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            Environment.Exit(1);
        }
    }
}
