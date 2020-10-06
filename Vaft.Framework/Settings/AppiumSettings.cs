namespace Vaft.Framework.Settings
{
    /// <summary>Appium capabilities</summary>>
    public class AppiumSettings
    {
        public string AppiumHubUrl;
        public string PlatformName;
        public string PlatformVersion;
        public string DeviceName;
        public string BrowserName;
        public object AutoAcceptAlerts;

        public AppiumSettings()
        {
            AppiumHubUrl = Config.GetSettingValue("Appium", "AppiumHubUrl", "http://127.0.0.1:4723/wd/hub");
            PlatformName = Config.GetSettingValue("Appium", "PlatformName", "Android");
            PlatformVersion = Config.GetSettingValue("Appium", "PlatformVersion", null);
            DeviceName = Config.GetSettingValue("Appium", "DeviceName", null);
            BrowserName = Config.GetSettingValue("Appium", "BrowserName", null);
            AutoAcceptAlerts = Config.GetSettingValue("Appium", "AutoAcceptAlerts", null);
        }
    }
}
