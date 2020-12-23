namespace Vaft.Framework.Settings
{
    /// <summary>BrowserStack capabilities
    /// http://www.browserstack.com/automate/capabilities
    /// </summary>
    public class BrowserStackSettings
    {
        public string BsUser;
        public string BsKey;
        public bool BsTunnel;
        public string BsProjectName;
        public string BsBuildVersion;
        public string BsOs;
        public string BsOsVersion;
        public string BsBrowser;
        public string BsBrowserVersion;
        public object BsDebug;
        public object BsAcceptSslCerts;
        public object BsResolution;
        public string BsUrl;
        public string BsBrowserName;
        public string BsPlatform;
        public string BsDevice;

        public BrowserStackSettings()
        {
            BsUser = Config.GetSettingValue("BrowserStack", "BrowserStack_User", null);
            BsKey = Config.GetSettingValue("BrowserStack", "BrowserStack_Key", null);
            BsTunnel = Config.GetBooleanSettingValue("BrowserStack", "BrowserStack_Tunnel", true);
            BsProjectName = Config.GetSettingValue("BrowserStack", "BrowserStack_Project", null);
            BsBuildVersion = Config.GetSettingValue("BrowserStack", "BrowserStack_Build", null);
            BsOs = Config.GetSettingValue("BrowserStack", "BrowserStack_Os", null);
            BsOsVersion = Config.GetSettingValue("BrowserStack", "BrowserStack_OsVersion", null);
            BsBrowser = Config.GetSettingValue("BrowserStack", "BrowserStack_Browser", null);
            BsBrowserVersion = Config.GetSettingValue("BrowserStack", "BrowserStack_BrowserVersion", null);
            BsDebug = Config.GetSettingValue("BrowserStack", "BrowserStack_Debug", null);
            BsAcceptSslCerts = Config.GetSettingValue("BrowserStack", "BrowserStack_AcceptSslCerts", null);
            BsResolution = Config.GetSettingValue("BrowserStack", "BrowserStack_Resolution", null);
            BsUrl = Config.GetSettingValue("BrowserStack", "BrowserStack_Url", "hub.browserstack.com");
            BsBrowserName = Config.GetSettingValue("BrowserStack", "BrowserStack_BrowserName", null);
            BsPlatform = Config.GetSettingValue("BrowserStack", "BrowseStack_Platform", "Desktop");
            BsDevice = Config.GetSettingValue("BrowserStack", "BrowserStack_Device", null);
        }
    }
}
