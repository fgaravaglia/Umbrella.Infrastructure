using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Umbrella.Infrastructure.Firestore.Abstractions;

namespace Umbrella.Infrastructure.Firestore.Extensions
{
    /// <summary>
    /// Extensions to load implementaiton of repository inside DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a repsoitory. 
        /// <para>
        /// In case of Localhost environment, it also set credentials into APSNET Variable
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <typeparam name="Tentity"></typeparam>
        /// <param name="services"></param>
        /// <param name="instanceFactory"></param>
        /// <param name="environmentName"></param>
        /// <param name="jsonCredentialsFilePath">full path to json file of credential for GCP project</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddRepository<T, TImpl, Tentity>(this IServiceCollection services, 
                                                            Func<IServiceProvider, TImpl> instanceFactory, 
                                                            string environmentName, string jsonCredentialsFilePath = "")
            where T : class, IModelEntityRepository<Tentity>
            where TImpl : class, T 
            where Tentity : class
        {
            if(services == null)
                throw new ArgumentNullException(nameof(services));
            if(String.IsNullOrEmpty(environmentName))
                throw new ArgumentNullException(nameof(environmentName));
            if (instanceFactory == null)
                throw new ArgumentNullException(nameof(instanceFactory));
            
            if(String.Equals(environmentName, "localhost", StringComparison.InvariantCultureIgnoreCase))
            {
                // I need to copy local files for credentials into variable
                if (String.IsNullOrEmpty(jsonCredentialsFilePath))
                    throw new ArgumentNullException(nameof(jsonCredentialsFilePath), $"Json file path for credentials is required only in LOCALHOST environment");

                var variableValue = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
                if (String.IsNullOrEmpty(variableValue))
                    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Path.GetFullPath(jsonCredentialsFilePath));
            }

            services.AddTransient<T, TImpl>(instanceFactory);
        }
    }
}