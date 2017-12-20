namespace Vaft.Framework.Settings
{
    public class ConfigurationSettings
    {
        public RuntimeSettings RuntimeSettings;
        public BrowserStackSettings BrowserStackSettings;
        public AppiumSettings AppiumSettings;

        public ConfigurationSettings()
        {
            RuntimeSettings = new RuntimeSettings();
            BrowserStackSettings = new BrowserStackSettings();
            AppiumSettings = new AppiumSettings();
        }
    }
}
