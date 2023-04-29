using System.Security.Claims;
namespace Umbrella.Security.Validation
{
    /// <summary>
    /// Exception to identiy specif error due to missing claim
    /// </summary>
    public class RequiredClaimSecurityException : SecurityException
    {
        /// <summary>
        /// Claims required that is missing in User profile
        /// </summary>
        /// <value></value>
        public Claim MissingClaim { get; private set; }
        /// <summary>
        /// Generic reason of error
        /// </summary>
        /// <value></value>
        public string Reason { get; private set; }

        /// <summary>
        /// Defualt COnstr
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public RequiredClaimSecurityException(Claim claim, string reason) : base("Unauthorized User: Missing or Wrong Claim " + claim.Type)
        {
            if(claim == null || (claim != null && String.IsNullOrEmpty(claim.Value)))
                throw new ArgumentNullException(nameof(claim));
            if (string.IsNullOrEmpty(reason))
                throw new ArgumentNullException(nameof(reason));

            this.MissingClaim = claim;
            this.Reason = reason;
        }
    }
}