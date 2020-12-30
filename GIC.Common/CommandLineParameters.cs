using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace GIC.Common
{
    public class CommandLineParameters
    {
        [Option("app", Required = true, HelpText = "Application to send commands to")]
        public string Application { get; set; }
        [Option("port", Required = false, HelpText = "IP Port to Listen on, if none provided uses the value in appsettings.json")]
        public ushort Port { get; set; }
        [Option("password", Required = false, HelpText = "Password to expect from client, if none provided uses the value in appsettings.json")]
        public string Password { get; set; }
    }
}
