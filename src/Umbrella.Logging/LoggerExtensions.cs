using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Umbrella.Logging
{
    /// <summary>
    /// Extentions for ILOGGER
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Sets the scope for a HTTP call from MVC application
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="url"></param>
        /// <param name="trxId"></param>
        /// <param name="areaName"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns>the scope as IDisposable item</returns>
        public static IDisposable BeginHttpTransactionScope(this ILogger logger, string url, Guid trxId, string areaName, string controllerName, string actionName)
        {
            if (String.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));
            if (String.IsNullOrEmpty(controllerName))
                throw new ArgumentNullException(nameof(controllerName));
            if (String.IsNullOrEmpty(actionName))
                throw new ArgumentNullException(nameof(actionName));

            return logger.BeginScope(new Dictionary<string, string>()
            {
                ["trxId"] = trxId.ToString(), 
                ["url"] = url, 
                ["areaName"] = areaName,
                ["controllerName"] = controllerName,
                ["actionName"] =  actionName
            });
        }
    }
}
