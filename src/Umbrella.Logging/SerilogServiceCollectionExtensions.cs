using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ILogger = Serilog.ILogger;
using Umbrella.Logging.Serilog;
using Serilog;

namespace Umbrella.Logging
{
    /// <summary>
    /// Extensions to inject Logging for Umbrella Applications
    /// </summary>
    public static class SerilogServiceCollectionExtensions
    {

        /// <summary>
        /// INjects Serilog implementation for logger in DI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="environmentCode"></param>
        /// <param name="applicationName"></param>
        public static ILogger AddUmbrellaLogging(this IServiceCollection services, IConfiguration config,
                                            string environmentCode,
                                            string applicationName = "UI.Web.MVCPortal")
        {
            if(services == null)
                throw new ArgumentNullException(nameof(services));
            if(config == null)
                throw new ArgumentNullException(nameof(config));

            // bbuild serilog logger
            var serilog = new LoggerConfiguration()
                            .ReadFrom.Configuration(config)
                            .WithUmbrellaEnrichers()
                            .UsingDestructLimits()
                            .UsingDefaultProperties(applicationName, environmentCode)
                            .CreateLogger();

            // instance MSFT logger factory
            services.AddSingleton<ILoggerFactory>(ctx => 
            {
                var factory = LoggerFactory.Create(builder => builder.AddSerilog(serilog));
                // inject other log providers
                var providers = ctx.GetServices<ILoggerProvider>();
                if(providers != null && providers.Any())
                    providers.ToList().ForEach(p => factory.AddProvider(p));
                return factory;
            });

            // instance the MSFT logger for campatibility
            services.AddTransient<Microsoft.Extensions.Logging.ILogger>(ctx =>
            {
                // get configuration
                var logFactory = ctx.GetService<ILoggerFactory>();
                if(logFactory == null)
                    throw new InvalidOperationException($"DI Exception: unable to resolve ILoggerFactory for Logging Setup");
                return logFactory.CreateLogger(applicationName);
            });

            return serilog;
        }
    }
}
