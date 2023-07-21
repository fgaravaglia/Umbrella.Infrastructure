using FluentEmail.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Umbrella.Infrastructure.EmailHelper
{
    /// <summary>
    /// Abstraction for service dedicated to sending emails
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends the input email message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        EmailServiceResponse Send(IFluentEmail message, CancellationToken? cancellationToken);
        /// <summary>
        /// Sends the message setting the template for the message and actualizing it before sending
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="templateFileName"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="isHtml"></param>
        /// <returns></returns>
        EmailServiceResponse Send<T>(IFluentEmail message, string templateFileName, T model, CancellationToken? cancellationToken, bool isHtml = true);
    }
}
