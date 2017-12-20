using System.Collections.Generic;
using BrowserStack;
using Vaft.Framework.Settings;

namespace Vaft.Framework.BrowserStack
{
    public static class BsTunnel
    {
        private static Local _local;

        public static void LaunchTunnel()
        {
            _local = new Local();
            _local.start(BsLocalOptions());
        }

        public static void StopTunnel()
        {
            _local.stop();
        }

        public static bool IsTunnelRunning()
        {
            return _local.isRunning();
        }

        private static List<KeyValuePair<string, string>> BsLocalOptions()
        {
            var bsKey = Config.Settings.BrowserStackSettings.BsKey;

            List<KeyValuePair<string, string>> options = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("key", bsKey),
//                new KeyValuePair<string, string>("localIdentifier", "identifier"),
//                new KeyValuePair<string, string>("f", "C:\\Users\\Admin\\Desktop\\"),
//                new KeyValuePair<string, string>("onlyAutomate", "true"),
//                new KeyValuePair<string, string>("verbose", "true"),
                new KeyValuePair<string, string>("forcelocal", "true"),
//                new KeyValuePair<string, string>("binarypath", "C:\\Users\\Admin\\Desktop\\BrowserStackLocal.exe"),
//                new KeyValuePair<string, string>("logFile", "C:\\Users\\Admin\\Desktop\\local.log"),
            };
            return options;
        }
    }
}
