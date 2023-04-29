namespace Umbrella.Security.Decorators
{
    /// <summary>
    /// Custom decorator for Controller Actions, to mark them wth required claims
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ClaimsAuthorizeAttribute : Attribute
    {
        /// <summary>
        /// The type of required Claim
        /// </summary>
        /// <value></value>
        public string RequiredClaimType {get; private set;}
        /// <summary>
        /// The value of required Claim
        /// </summary>
        /// <value></value>
        public string RequiredClaimValue { get; private set; }

        /// <summary>
        /// Default COnstr
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ClaimsAuthorizeAttribute(string type, string value) : base()
        {
            if(String.IsNullOrEmpty(type))
                throw new ArgumentNullException(nameof(type));

            if (String.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            this.RequiredClaimType = type;
            this.RequiredClaimValue = value;
        }
    }
}