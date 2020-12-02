using System.Collections.Generic;

namespace GamingInterfaceCommandServer.Models
{
    interface IAppSettings
    {
        List<string> Applications { get; }
        string Password { get; set; }
        string Port { get; set; }
    }
}