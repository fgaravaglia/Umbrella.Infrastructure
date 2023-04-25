using System;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;

namespace Umbrella.Logging.Serilog
{
    /// <summary>
    /// Extesnions to fluently create configuration specific for Serilog
    /// </summary>
    public static class SerilogLoggerConfigurationExtensions
    {
        /// <summary>
        /// Sets all default enrichers for logger
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static LoggerConfiguration WithUmbrellaEnrichers(this LoggerConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            config.Enrich.WithSpan(new SpanOptions()
            {
                IncludeBaggage = true
            })
                    .Enrich.WithEnvironmentName()
                    .Enrich.WithEnvironmentUserName()
                    .Enrich.WithThreadId()
                    .Enrich.WithClientIp()
                    .Enrich.WithClientAgent()
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithProcessId()
                    .Enrich.WithProcessName()
                    .Enrich.FromLogContext();

            return config;
        }
        /// <summary>
        /// Sets the proper Limit of Destructuring
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static LoggerConfiguration UsingDestructLimits(this LoggerConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            config.Destructure.ToMaximumStringLength(100)
                    .Destructure.ToMaximumDepth(4)
                    .Destructure.ToMaximumCollectionCount(10);

            return config;
        }
        /// <summary>
        /// Adds the default properties that must to be there for each log entry
        /// </summary>
        /// <param name="config"></param>
        /// <param name="applicationName"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static LoggerConfiguration UsingDefaultProperties(this LoggerConfiguration config, string applicationName, string environment)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (String.IsNullOrEmpty(applicationName))
                throw new ArgumentNullException(nameof(applicationName));
            if (String.IsNullOrEmpty(environment))
                throw new ArgumentNullException(nameof(environment));

            config.Enrich.WithProperty("Application", applicationName)
                    .Enrich.WithProperty("Env", environment);

            return config;

        }
    }
}