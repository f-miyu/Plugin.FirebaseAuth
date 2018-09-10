using System;
namespace Plugin.FirebaseAuth
{
    public class FirebaseAuthException : Exception
    {
        public ErrorType ErrorType { get; }

        public FirebaseAuthException(string message, ErrorType errorType) : base(message)
        {
            ErrorType = errorType;
        }

        public FirebaseAuthException(string message, Exception innerException, ErrorType errorType) : base(message, innerException)
        {
            ErrorType = errorType;
        }
    }
}
