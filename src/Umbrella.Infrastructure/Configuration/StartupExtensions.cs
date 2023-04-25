using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Umbrella.Infrastructure.Configuration
{
    public static class StartupExtensions
    {
        /// <summary>
        /// Registers the Umbrella reader into DI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void AddUmbrellaConfigurationReader(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton(x => new UmbrellaConfigurationReader(config));
        }
    }
}