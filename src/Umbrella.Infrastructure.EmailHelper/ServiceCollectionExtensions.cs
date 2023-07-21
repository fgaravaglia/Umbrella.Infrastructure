using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbrella.Infrastructure.EmailHelper
{
    public static class ServiceCollectionExtensions
    {
        internal static EmailSettings GetEmailSettings(this IConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            EmailSettings settings = new EmailSettings();
            config.GetSection("Email").Bind(settings);

            if (string.IsNullOrEmpty(settings.DefaultSenderName))
                throw new InvalidOperationException($"Wrong Configuration: Default Sender Address cannot be null");
            if (string.IsNullOrEmpty(settings.SmtpServer))
                throw new InvalidOperationException($"Wrong Configuration: smtpServer cannot be null");
            if (string.IsNullOrEmpty(settings.SmtpUsername))
                throw new InvalidOperationException($"Wrong Configuration: smtpServerUsername cannot be null");
            if (string.IsNullOrEmpty(settings.SmtpPassword))
                throw new InvalidOperationException($"Wrong Configuration: smtpServerPwd cannot be null");

            return settings;
        }

        /// <summary>
        /// Adds email service to send email
        /// <para>
        /// for more information, see https://github.com/lukencode/FluentEmail
        /// </para>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AddEmailServices(this IServiceCollection services, IConfiguration config)
        {
            if(services == null)
                throw new ArgumentNullException(nameof(services));
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            // read default sender from config
            var settings = config.GetEmailSettings();
            services.AddFluentEmail(settings.DefaultSenderAddress, settings.DefaultSenderName)
                        .AddLiquidRenderer(x =>
                        {
                            if (string.IsNullOrEmpty(settings.SmtpPassword))
                                throw new InvalidOperationException($"Wrong Configuration: TemplateFolderPath cannot be null");
                            x.FileProvider = new PhysicalFileProvider(settings.TemplateFolderPath);
                        })
                        //.AddSmtpSender(smtpServer, smtpServerPort)
                        .AddMailtrapSender(settings.SmtpUsername, settings.SmtpPassword, settings.SmtpServer, settings.SmtpServerPort);
        }
    }
}
