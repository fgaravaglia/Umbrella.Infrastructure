using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbrella.Infrastructure.EmailHelper
{
    /// <summary>
    /// Model to map dedicated section on AppSettings file
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string DefaultSenderAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DefaultSenderName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SmtpServer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SmtpServerPort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SmtpUsername { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SmtpPassword {  get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TemplateFolderPath {  get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EmailSettings()
        {
            this.DefaultSenderAddress = "";
            this.DefaultSenderName = "";
            this.SmtpServer= "localhost";
            this.SmtpServerPort = 43;
            this.SmtpUsername = "";
            this.SmtpPassword = "";
            this.TemplateFolderPath = "";
        }
    }
}
