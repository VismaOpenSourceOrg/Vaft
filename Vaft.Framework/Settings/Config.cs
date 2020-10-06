using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.IO;

namespace Vaft.Framework.Settings
{
    public static class Config
    {

        public static IConfiguration Configuration;


        static Config()
        {

            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine("appsettings.json"))
                .AddEnvironmentVariables();
            Configuration = configurationBuilder.Build();
        }

        public static ConfigurationSettings Settings { get; set; }

        public static string GetSettingValue(string section, string key)
        {
            string result = Configuration.GetSection(section).GetSection(key).Value;
            return result;
        }

        public static string GetSettingValue(string section, string key, string defaultValue)
        {

            var setting = Configuration.GetSection(section).GetSection(key).Value ?? defaultValue;
            return setting;
        }

        public static bool GetBooleanSettingValue(string section, string key)
        {
            bool settingValue;
            Boolean.TryParse(GetSettingValue(section, key), out settingValue);
            return settingValue;
        }

        public static bool GetBooleanSettingValue(string section, string key, bool defaultValue)
        {
            var setting = GetSettingValue(section, key);

            if (setting == null)
            {
                return defaultValue;
            }

            bool settingValue;
            Boolean.TryParse(setting, out settingValue);
            return settingValue;
        }
    }
}
