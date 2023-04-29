using System.Security.Claims;

namespace Umbrella.Security.Validation
{
    /// <summary>
    /// Custom exception to identify security issue
    /// </summary>
    public class SecurityException : Exception
    {
        public SecurityException() : base()
        {

        }

        public SecurityException(string message) : base(message)
        {

        }
    }
    
    
}