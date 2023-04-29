using System.Reflection;
using System.Security.Claims;
using Umbrella.Security.Decorators;

namespace Umbrella.Security.Validation
{
    /// <summary>
    /// Abstarction of validator
    /// </summary>
    public interface ISecurityVerifier
    {
        /// <summary>
        /// Checks if user has all required claims, defined by custom attributes
        /// </summary>
        /// <param name="user"></param>
        /// <param name="actionMethod">Method of an object covered by custom attribute ClaimsAuthorizeAttribute </param>
        /// <returns>Success if user is authorize, Failed otherwise</returns>
        ValidationResult AuthorizeUserOnMethod(ClaimsPrincipal user, MethodInfo actionMethod);
    }
    /// <summary>
    /// verifier for authorization permissions
    /// </summary>
    public class SecurityVerifier : ISecurityVerifier
    {
        /// <summary>
        /// EMpty COnstr
        /// </summary>
        public SecurityVerifier()
        {

        }
        /// <summary>
        /// Checks if user has all required claims, defined by custom attributes
        /// </summary>
        /// <param name="user"></param>
        /// <param name="actionMethod">Method of an object covered by custom attribute ClaimsAuthorizeAttribute </param>
        /// <returns>Success if user is authorize, Failed otherwise</returns>
        public ValidationResult AuthorizeUserOnMethod(ClaimsPrincipal user, MethodInfo actionMethod)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));
            if (actionMethod is null)
                throw new ArgumentNullException(nameof(actionMethod));

            //get security attributes.
            List<ClaimsAuthorizeAttribute> requiredClaimAttributes = actionMethod.GetCustomAttributes(typeof(ClaimsAuthorizeAttribute), true)
                                                                    .Select(x => x as ClaimsAuthorizeAttribute).ToList();
            foreach (var attribute in requiredClaimAttributes)
            {
                //get the target claim
                var claim = user.Claims.SingleOrDefault(x => x.Type == attribute.RequiredClaimType);
                // check if claim is there
                if (claim is null)
                {
                    var securityEx = new RequiredClaimSecurityException(new Claim(attribute.RequiredClaimType, attribute.RequiredClaimValue), "Claim type not found");
                    return ValidationResult.Failed(securityEx);
                }
                // check if the value is fine
                int userValue = ConvertClaimValueToInt(claim.Value);
                int requiredValue = ConvertClaimValueToInt(attribute.RequiredClaimValue);
                if (userValue < requiredValue)
                {
                    var securityEx = new RequiredClaimSecurityException(new Claim(attribute.RequiredClaimType, attribute.RequiredClaimValue), $"Wrong value {claim.Value} for required claim type");
                    return ValidationResult.Failed(securityEx);
                }
            }
            return ValidationResult.Success();
        }

        int ConvertClaimValueToInt(string value)
        {
            if (String.IsNullOrEmpty(value))
                return -1;

            switch (value)
            {
                case "R":
                    return 5;
                case "W":
                    return 10;
                default:
                    return 1;
            }
        }
    }

}