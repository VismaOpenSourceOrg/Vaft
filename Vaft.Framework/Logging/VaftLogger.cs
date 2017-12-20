using NLog;
using NLog.Config;
using NLog.Targets;

namespace Vaft.Framework.Logging
{
    public class VaftLogger : IVaftLogger
    {
        private readonly ILogger _log;
        private static IVaftLogger _vaftLogger;

        public VaftLogger()
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var fileTarget = new FileTarget();
            config.AddTarget("File", fileTarget);

            consoleTarget.Layout = @"${date:format=yyyy.MM.dd HH\:mm\:ss} ${level:uppercase=true} ${logger}   ${message}";
            fileTarget.FileName = "${basedir}/Logs/Vaft.log";
            fileTarget.Layout = @"${date:format=yyyy.MM.dd HH\:mm\:ss} ${level:uppercase=true} ${logger}   ${message}";

            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, consoleTarget));
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Warn, consoleTarget));
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Error, consoleTarget));

            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Error, fileTarget));

            LogManager.Configuration = config;
            _log = LogManager.GetLogger("VaftLog");
        }

        public static IVaftLogger Initialize()
        {
            return _vaftLogger ?? (_vaftLogger = new VaftLogger());
        }

        public void Info(string message)
        {
            _log.Info(message);
        }

        public void Warning(string message)
        {
            _log.Warn(message);
        }

        public void Error(string message)
        {
            _log.Error(message);
        }
    }
}
