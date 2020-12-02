using System.Collections.Generic;

namespace GIC.Console.Services
{
    interface IConfigurationService
    {
        List<string> Applications { get; set; }
        string Password { get; set; }
        string Port { get; set; }
    }
}