using System.Security.Claims;

namespace Umbrella.Security.Validation
{
    /// <summary>
    /// Exception to identiy specif error due to missing role
    /// </summary>
    public class RequiredRoleSecurityException : SecurityException
    {
        /// <summary>
        /// Role missing
        /// </summary>
        /// <value></value>
        public string Role { get; private set; }

        /// <summary>
        /// Default Constr
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public RequiredRoleSecurityException(string role) : base("Unauthorized User: Missing role " + role)
        {
            if (String.IsNullOrEmpty(role))
                throw new ArgumentNullException(nameof(role));
            this.Role = role;
        }
    }
}