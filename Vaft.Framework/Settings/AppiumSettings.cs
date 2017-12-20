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
            AppiumHubUrl = Config.GetSettingValue("AppiumHubUrl", "http://127.0.0.1:4723/wd/hub");
            PlatformName = Config.GetSettingValue("PlatformName", "Android");
            PlatformVersion = Config.GetSettingValue("PlatformVersion", null);
            DeviceName = Config.GetSettingValue("deviceName", null);
            BrowserName = Config.GetSettingValue("BrowserName", null);
            AutoAcceptAlerts = Config.GetSettingValue("AutoAcceptAlerts", null);
        }
    }
}
