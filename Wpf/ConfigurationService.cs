using GIC.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace GIC.Wpf
{
    class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRoot configuration;

        public ConfigurationService(IConfigurationRoot configurationRoot)
        {
            configuration = configurationRoot;
        }


        public string Password
        {
            get => configuration.GetValue<string>("Password");
            set => SetValue("Password", value);
        }
        public ushort Port
        {
            get => configuration.GetValue<ushort>("Port");
            set => SetValue("Port", value);
        }

        public int SelectedApp
        {
            get => configuration.GetValue<int>("SelectedApp");
            set => SetValue("SelectedApp", value);
        }

        public List<string> Applications
        {
            get => configuration.GetSection("Applications").Get<List<string>>();
            set
            {
                SetValue("Applications", value);
            }
        }

        private void SetValue<T>(string key, T newValue)
        {
            string[] configFilePaths = configuration.Providers.Cast<JsonConfigurationProvider>().Select(p => p.Source.Path).ToArray();
            if (configFilePaths.Length > 0)
            {
                Dictionary<string, object> jsonValues = JsonSerializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(configFilePaths[0]));
                if (jsonValues.ContainsKey(key))
                {
                    jsonValues[key] = newValue;

                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                    string newJson = JsonSerializer.Serialize(jsonValues, options);
                    File.WriteAllText(configFilePaths[0], newJson);
                }
            }
        }
    }
}
