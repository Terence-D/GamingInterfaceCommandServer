using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace GIC.Console.Services
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
                configuration["Applications"] = JsonSerializer.Serialize(value);
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
                    jsonValues[key] = JsonSerializer.Serialize(newValue);

                    JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                    string newJson = JsonSerializer.Serialize(jsonValues, options);
                    File.WriteAllText(configFilePaths[0], newJson);
                }
            }
        }
    }
}
