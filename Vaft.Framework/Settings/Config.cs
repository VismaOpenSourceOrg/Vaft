using System;
using System.Configuration;

namespace Vaft.Framework.Settings
{
    public static class Config
    {
        public static ConfigurationSettings Settings { get; set; }

        public static string GetSettingValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string GetSettingValue(string key, string defaultValue)
        {
            var setting = GetSettingValue(key) ?? defaultValue;
            return setting;
        }

        public static bool GetBooleanSettingValue(string key)
        {
            bool settingValue;
            Boolean.TryParse(GetSettingValue("BrowserStack"), out settingValue);
            return settingValue;
        }

        public static bool GetBooleanSettingValue(string key, bool defaultValue)
        {
            var setting = GetSettingValue(key);

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
