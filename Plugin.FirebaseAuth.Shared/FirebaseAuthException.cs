using System;
using System.ComponentModel;
namespace Plugin.FirebaseAuth
{
    public class FirebaseAuthException : Exception
    {
        public ErrorType ErrorType { get; }
        public string Reason { get; }

        public FirebaseAuthException(string message, ErrorType errorType, string reason = null) : base(message)
        {
            ErrorType = errorType;
            Reason = reason;
        }

        public FirebaseAuthException(string message, Exception innerException, ErrorType errorType, string reason = null) : base(message, innerException)
        {
            ErrorType = errorType;
            Reason = reason;
        }
    }
}
