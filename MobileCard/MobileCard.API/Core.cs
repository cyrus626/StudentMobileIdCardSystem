using FluentScheduler;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace MobileCard.API
{
    public static class Core
    {
        #region Properties
        public static IWebHostEnvironment HostEnvironment { get; private set; }
        public static Logger Log { get; } = LogManager.GetCurrentClassLogger();

        #region Solids

        #region Names
        public const string PRODUCT_NAME = "MobileCard.API";
        public const string SHORT_PRODUCT_NAME = "MobileCard";
        public const string PRODUCT_VERSION = "0.1";
        public static string FULL_PRODUCT_NAME => $"{Core.PRODUCT_NAME} {Core.PRODUCT_VERSION}";
        public static string AUTHOR = "Cyrus Momoh";

        public const string EnvKey = "MOBILE_CARD_ENV_KEY";
        public static string BASE_URL = "";
        #endregion

        #region Directories
        public static readonly string BASE_DIR = Directory.GetCurrentDirectory();
        public static readonly string WORK_DIR = Path.Combine(BASE_DIR, "App");
        public static readonly string DATA_DIR = Path.Combine(WORK_DIR, "Data");
        public static readonly string RESOURCES_DIR = Path.Combine(WORK_DIR, "Resources");
        public static readonly string INDEX_DIR = Path.Combine(WORK_DIR, "Indexes");
        public readonly static string LOG_DIR = Path.Combine(WORK_DIR, "Logs");
        #endregion

        #region Paths
        public static string RUNTIME_LOG_PATH => Path.Combine(LOG_DIR, RUNTIME_LOG_NAME + ".log");
        public static string ERROR_LOG_PATH => Path.Combine(LOG_DIR, ERROR_LOG_NAME + ".log");
        public const string DATABASE_VERSION = "v1";
        public readonly static string DATABASE_PATH = Path.Combine(DATA_DIR, $"Data-v{DATABASE_VERSION}.sqlite");
        #endregion

        #region Routes
        public const string DOCS_ROUTE = "/api/docs";
        #endregion

        #region JWT Claim Identifiers
        public const string JWT_CLAIM_ID = "id";
        public const string JWT_CLAIM_ROL = "rol";
        public const string JWT_CLAIM_ROLES = "roles";
        public const string JWT_CLAIM_VERIFIED = "ver";
        public const string JWT_CLAIM_APP_ACCESS = "aacc";
        #endregion

        #region JWT Claims
        public const string JWT_CLAIM_API_USER = "api_user";
        public const string JWT_CLAIM_API_ACCESS = "api_access";
        #endregion

        const string CONSOLE_LOG_NAME = "console-debugger";
        const string LOG_LAYOUT = "${longdate}|${uppercase:${level}}| ${message} ${exception:format=tostring}";
        const string FULL_LOG_LAYOUT = "${longdate} | ${callsite:fileName=true}\n${message} ${exception:format=tostring}\n";
        static string ERROR_LOG_NAME = $"Errors_{DateTime.Now:MM-yyyy}";
        static string RUNTIME_LOG_NAME = $"Runtime_{DateTime.Now:MM-yyyy}";

        #endregion

        #endregion

        #region Methods
        public static void Init(string[] args)
        {
            ConfigureLogger();
        }
        #endregion

        public static void ConfigureLogger()
        {
            var config = new LoggingConfiguration();

#if DEBUG
            var debugConsole = new ColoredConsoleTarget()
            {
                Name = CONSOLE_LOG_NAME,
                Layout = FULL_LOG_LAYOUT,
                Header = $"{PRODUCT_NAME} Debugger"
            };

            var debugRule = new LoggingRule("*", NLog.LogLevel.Debug, debugConsole);
            config.LoggingRules.Add(debugRule);
#endif

            var errorFileTarget = new FileTarget()
            {
                Name = ERROR_LOG_NAME,
                FileName = ERROR_LOG_PATH,
                Layout = LOG_LAYOUT
            };

            config.AddTarget(errorFileTarget);

            var errorRule = new LoggingRule("*", NLog.LogLevel.Error, errorFileTarget);
            config.LoggingRules.Add(errorRule);

            var runtimeFileTarget = new FileTarget()
            {
                Name = RUNTIME_LOG_NAME,
                FileName = RUNTIME_LOG_PATH,
                Layout = LOG_LAYOUT
            };
            config.AddTarget(runtimeFileTarget);

            var runtimeRule = new LoggingRule("*", NLog.LogLevel.Trace, runtimeFileTarget);
            config.LoggingRules.Add(runtimeRule);

            LogManager.Configuration = config;

            LogManager.ReconfigExistingLoggers();

            DateTime oneMonthLater = DateTime.Now.AddMonths(1);
            DateTime nextMonth = new DateTime(oneMonthLater.Year, oneMonthLater.Month, 1);

            JobManager.AddJob(() =>
            {
                Core.Log.Debug("*** Monthly Session Ended ***");
                ConfigureLogger();
            }, s => s.ToRunOnceAt(nextMonth));
        }
    }
}
