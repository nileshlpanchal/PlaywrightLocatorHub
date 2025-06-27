using Microsoft.Extensions.Configuration;

namespace PlaywrightFramework.Utilities
{
    /// <summary>
    /// Configuration reader for test settings and application configuration
    /// </summary>
    public static class ConfigReader
    {
        private static IConfiguration? _configuration;

        static ConfigReader()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize configuration from appsettings.json
        /// </summary>
        private static void Initialize()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();

                _configuration = builder.Build();
                Logger.Information("Configuration initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing configuration: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get test settings configuration
        /// </summary>
        public static TestSettings GetTestSettings()
        {
            try
            {
                var testSettings = new TestSettings();
                _configuration?.GetSection("TestSettings").Bind(testSettings);
                return testSettings;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error reading test settings: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get logging configuration
        /// </summary>
        public static LoggingSettings GetLoggingSettings()
        {
            try
            {
                var loggingSettings = new LoggingSettings();
                _configuration?.GetSection("Logging").Bind(loggingSettings);
                return loggingSettings;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error reading logging settings: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get configuration value by key
        /// </summary>
        public static string GetConfigurationValue(string key, string defaultValue = "")
        {
            try
            {
                return _configuration?[key] ?? defaultValue;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error reading configuration value {key}: {ex.Message}");
                return defaultValue;
            }
        }

        /// <summary>
        /// Get connection string
        /// </summary>
        public static string GetConnectionString(string name)
        {
            try
            {
                return _configuration?.GetConnectionString(name) ?? string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error reading connection string {name}: {ex.Message}");
                return string.Empty;
            }
        }
    }

    /// <summary>
    /// Test settings configuration model
    /// </summary>
    public class TestSettings
    {
        public string BaseUrl { get; set; } = "http://localhost:5000";
        public string Browser { get; set; } = "chromium";
        public bool Headless { get; set; } = false;
        public int Timeout { get; set; } = 30000;
        public int SlowMo { get; set; } = 100;
        public int ViewportWidth { get; set; } = 1920;
        public int ViewportHeight { get; set; } = 1080;
        public bool Screenshot { get; set; } = true;
        public bool Video { get; set; } = false;
        public bool Trace { get; set; } = true;
    }

    /// <summary>
    /// Logging settings configuration model
    /// </summary>
    public class LoggingSettings
    {
        public string Level { get; set; } = "Information";
        public bool LogToFile { get; set; } = true;
        public string LogPath { get; set; } = "logs/test-{Date}.log";
    }
}
