using System;
using System.ComponentModel;
namespace Plugin.FirebaseAuth
{
    public class FirebaseAuthException : Exception
    {
        public FirebaseAuthException(string? message, Exception innerException, ErrorType errorType, string? errorCode) : base(message, innerException)
        {
            ErrorType = errorType;
            ErrorCode = errorCode ?? "UNKNOWN";
        }

        public FirebaseAuthException(string? message, Exception innerException, ErrorType errorType, string? errorCode, string? reason)
            : this(message, innerException, errorType, errorCode)
        {
            Reason = reason;
        }

        public FirebaseAuthException(string? message, Exception innerException, ErrorType errorType, string? errorCode, string? email, IAuthCredential? updatedCredential)
            : this(message, innerException, errorType, errorCode)
        {
            Email = email;
            UpdatedCredential = updatedCredential;
        }

        public FirebaseAuthException(string? message, Exception innerException, ErrorType errorType, string? errorCode, IMultiFactorResolver? resolver)
            : this(message, innerException, errorType, errorCode)
        {
            Resolver = resolver;
        }

        public ErrorType ErrorType { get; }
        public string ErrorCode { get; }
        public string? Reason { get; }
        public string? Email { get; }
        public IAuthCredential? UpdatedCredential { get; }
        public IMultiFactorResolver? Resolver { get; }
    }
}
