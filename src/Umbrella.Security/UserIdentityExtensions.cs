using System;
using System.Linq;
using System.Security.Claims;

namespace Umbrella.Security
{
    /// <summary>
    /// Helper for identity
    /// </summary>
    public static class UserIdentityExtensions
    {
        /// <summary>
        /// Checks if user is ADmin or not, based on his roles
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static bool IsAdministrator(this ClaimsPrincipal principal)
        {
            if (principal.Identity == null)
                return false;

            if (!principal.Identity.IsAuthenticated)
                return false;

            var roleClaims = principal.Claims.Where(x => x.Type == ClaimTypes.Role).ToList();
            if (roleClaims.Any(x => x.Value == "ADMIN"))
                return true;
            return false;
        }

        /// <summary>
        /// Checks if user is ADmin or not, based on his roles
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static bool HasRoleOf(this ClaimsPrincipal principal, string role)
        {
            if (String.IsNullOrEmpty(role))
                return false;

            if (principal.Identity == null)
                return false;

            if (!principal.Identity.IsAuthenticated)
                return false;

            var roleClaims = principal.Claims.Where(x => x.Type == ClaimTypes.Role).ToList();
            if (roleClaims.Any(x => x.Value == role))
                return true;

            return false;
        }
    }
}