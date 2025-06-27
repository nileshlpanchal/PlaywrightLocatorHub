namespace PlaywrightFramework.Utilities
{
    /// <summary>
    /// Logger utility for test framework logging
    /// </summary>
    public static class Logger
    {
        private static bool _isInitialized = false;

        /// <summary>
        /// Initialize logger with configuration
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized) return;

            try
            {
                var loggingSettings = ConfigReader.GetLoggingSettings();
                
                var loggerConfig = new LoggerConfiguration()
                    .MinimumLevel.Is(GetLogLevel(loggingSettings.Level))
                    .WriteTo.Console(outputTemplate: 
                        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");

                if (loggingSettings.LogToFile)
                {
                    var logPath = loggingSettings.LogPath.Replace("{Date}", DateTime.Now.ToString("yyyyMMdd"));
                    Directory.CreateDirectory(Path.GetDirectoryName(logPath) ?? "logs");
                    
                    loggerConfig.WriteTo.File(
                        logPath,
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}");
                }

                Log.Logger = loggerConfig.CreateLogger();
                _isInitialized = true;
                
                Information("Logger initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing logger: {ex.Message}");
                
                // Fallback to basic console logger
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .CreateLogger();
                _isInitialized = true;
            }
        }

        /// <summary>
        /// Convert string log level to Serilog LogEventLevel
        /// </summary>
        private static Serilog.Events.LogEventLevel GetLogLevel(string level)
        {
            return level.ToLower() switch
            {
                "verbose" => Serilog.Events.LogEventLevel.Verbose,
                "debug" => Serilog.Events.LogEventLevel.Debug,
                "information" => Serilog.Events.LogEventLevel.Information,
                "warning" => Serilog.Events.LogEventLevel.Warning,
                "error" => Serilog.Events.LogEventLevel.Error,
                "fatal" => Serilog.Events.LogEventLevel.Fatal,
                _ => Serilog.Events.LogEventLevel.Information
            };
        }

        /// <summary>
        /// Log information message
        /// </summary>
        public static void Information(string message)
        {
            if (!_isInitialized) Initialize();
            Log.Information($"[{GetCallerInfo()}] {message}");
        }

        /// <summary>
        /// Log warning message
        /// </summary>
        public static void Warning(string message)
        {
            if (!_isInitialized) Initialize();
            Log.Warning($"[{GetCallerInfo()}] {message}");
        }

        /// <summary>
        /// Log error message
        /// </summary>
        public static void Error(string message)
        {
            if (!_isInitialized) Initialize();
            Log.Error($"[{GetCallerInfo()}] {message}");
        }

        /// <summary>
        /// Log error message with exception
        /// </summary>
        public static void Error(Exception exception, string message)
        {
            if (!_isInitialized) Initialize();
            Log.Error(exception, $"[{GetCallerInfo()}] {message}");
        }

        /// <summary>
        /// Log debug message
        /// </summary>
        public static void Debug(string message)
        {
            if (!_isInitialized) Initialize();
            Log.Debug($"[{GetCallerInfo()}] {message}");
        }

        /// <summary>
        /// Log verbose message
        /// </summary>
        public static void Verbose(string message)
        {
            if (!_isInitialized) Initialize();
            Log.Verbose($"[{GetCallerInfo()}] {message}");
        }

        /// <summary>
        /// Log fatal message
        /// </summary>
        public static void Fatal(string message)
        {
            if (!_isInitialized) Initialize();
            Log.Fatal($"[{GetCallerInfo()}] {message}");
        }

        /// <summary>
        /// Get caller information for logging context
        /// </summary>
        private static string GetCallerInfo()
        {
            try
            {
                var stackTrace = new System.Diagnostics.StackTrace(2, true);
                var frame = stackTrace.GetFrame(0);
                var method = frame?.GetMethod();
                var className = method?.DeclaringType?.Name ?? "Unknown";
                var methodName = method?.Name ?? "Unknown";
                
                return $"{className}.{methodName}";
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// Start a timed operation
        /// </summary>
        public static IDisposable StartTimedOperation(string operationName)
        {
            if (!_isInitialized) Initialize();
            Information($"Starting operation: {operationName}");
            return new TimedOperation(operationName);
        }

        /// <summary>
        /// Timed operation helper class
        /// </summary>
        private class TimedOperation : IDisposable
        {
            private readonly string _operationName;
            private readonly DateTime _startTime;

            public TimedOperation(string operationName)
            {
                _operationName = operationName;
                _startTime = DateTime.Now;
            }

            public void Dispose()
            {
                var duration = DateTime.Now - _startTime;
                Information($"Completed operation: {_operationName} (Duration: {duration.TotalMilliseconds:F0}ms)");
            }
        }
    }
}
