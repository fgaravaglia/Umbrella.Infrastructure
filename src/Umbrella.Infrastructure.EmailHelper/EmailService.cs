using FluentEmail.Core;
using Microsoft.Extensions.Logging;

namespace Umbrella.Infrastructure.EmailHelper
{
    public class EmailService
    {
        readonly ILogger _Logger;
        readonly EmailSettings _Settings;

        public EmailService(ILogger logger, EmailSettings settings)
        { 
            this._Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }
        /// <summary>
        /// <inheritdoc cref="IEmailService.Send(IFluentEmail, CancellationToken?)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public EmailServiceResponse Send(IFluentEmail message, CancellationToken? cancellationToken)
        {
            string methodName = $"{this.GetType().Name}.{nameof(Send)}";
            try
            {
                this._Logger.LogInformation("Start {methodName}", methodName);

                if (message == null)
                    throw new ArgumentNullException(nameof(message));
                if (message.Sender == null)
                {
                    this._Logger.LogInformation("Setting default sender...");
                    Email.From(this._Settings.DefaultSenderAddress);
                }

                this._Logger.LogInformation($"Sending message: {message}");
                message.Send(cancellationToken);
                this._Logger.LogInformation("Message succesfully sent");

                return EmailServiceResponse.Success();
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex, "Failed {methodName}", methodName);
                return EmailServiceResponse.Failure(ex.Message);
            }
            finally 
            {
                this._Logger.LogInformation("End {methodName}", methodName);
            }
        }
        /// <summary>
        /// <inheritdoc cref="IEmailService.Send{T}(IFluentEmail, string, T, CancellationToken?, bool)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="templateFileName"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="isHtml"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public EmailServiceResponse Send<T>(IFluentEmail message, string templateFileName, T model, CancellationToken? cancellationToken, bool isHtml = true)
        {
            string methodName = $"{this.GetType().Name}.{nameof(Send)}";
            try
            {
                this._Logger.LogInformation("Start {methodName}", methodName);

                if (message == null)
                    throw new ArgumentNullException(nameof(message));
                if (String.IsNullOrEmpty(templateFileName))
                    throw new ArgumentNullException(nameof(templateFileName));
                if (model == null)
                    throw new ArgumentNullException(nameof(model));

                this._Logger.LogInformation("Setting Template...");
                message.UsingTemplateFromFile<T>(templateFileName, model, isHtml);

                return this.Send(message, cancellationToken);
            }
            catch (Exception ex)
            {
                this._Logger.LogError(ex, "Failed {methodName}", methodName);
                return EmailServiceResponse.Failure(ex.Message);
            }
            finally
            {
                this._Logger.LogInformation("End {methodName}", methodName);
            }
        }
    }
}