namespace Umbrella.Security.Validation
{
    /// <summary>
    /// Simple DTO to map a result of validation process
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Result { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string Message { get; private set; }
        /// <summary>
        /// Occurred error
        /// </summary>
        /// <value></value>
        public Exception OccurredError { get; private set; }
        /// <summary>
        /// TRUE IF validation is succesful, FLASE otherwise
        /// </summary>
        public bool IsValid { get { return !String.IsNullOrEmpty(Result) && Result.ToUpperInvariant() == "OK"; } }

        /// <summary>
        /// Default Constr
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public ValidationResult(string result, string message, Exception ex = null)
        {
            this.Result = result;
            this.Message = message;
            this.OccurredError = ex;
        }
        /// <summary>
        /// Gets the success result
        /// </summary>
        /// <returns></returns>
        public static ValidationResult Success()
        {
            return new ValidationResult("OK", "");
        }
        /// <summary>
        /// Gets the failed result
        /// </summary>
        /// <param name="securityEx"></param>
        /// <returns></returns>
        public static ValidationResult Failed(RequiredClaimSecurityException securityEx)
        {
            var message = $"{securityEx.Reason}: Expecting {securityEx.MissingClaim.Type} with value {securityEx.MissingClaim.Value}";
            return new ValidationResult("KO", message, securityEx);
        }
    }
}