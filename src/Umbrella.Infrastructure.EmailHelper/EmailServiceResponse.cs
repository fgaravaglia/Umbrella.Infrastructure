using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbrella.Infrastructure.EmailHelper
{
    public class EmailServiceResponse
    {
        public int Result { get; set; }

        public string Message { get; set; }

        public static EmailServiceResponse Success()
        {
            return new EmailServiceResponse()
            {
                Result = 200
            };
        }

        public static EmailServiceResponse Failure(string message, int errorCode = 500) 
        {
            if(string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));
            if (errorCode == 200 || errorCode <= 0)
                throw new ArgumentException(nameof(errorCode));

            return new EmailServiceResponse()
            {
                Message = message.Trim(), 
                Result = errorCode
            };
        }
    }
}
