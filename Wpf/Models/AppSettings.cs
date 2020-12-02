using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace GamingInterfaceCommandServer.Models
{
    class AppSettings : IAppSettings
    {
        public string Password
        {
            get => configuration.GetValue<string>("Password");
            set => configuration["Password"] = value;
        }
        public string Port
        {
            get => configuration.GetValue<string>("Port");
            set => configuration["Port"] = value;
        }

        public List<string> Applications
        {
            get => configuration.GetValue<List<string>>("Applications");
            set
            {
                //configuration["Applications"] = JsonConvert.SerializeObject(value);
            }
        }

        private IConfiguration configuration;

        public AppSettings(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

    }
}
