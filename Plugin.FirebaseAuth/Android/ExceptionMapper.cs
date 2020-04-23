using System;
using Firebase;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    internal static class ExceptionMapper
    {
        public static Exception Map(FirebaseException exception)
        {
            var errorType = ErrorType.Other;
            string reason = null;
            switch (exception)
            {
                case FirebaseNetworkException firebaseNetworkException:
                    errorType = ErrorType.NetWork;
                    break;
                case FirebaseTooManyRequestsException firebaseTooManyRequestsException:
                    errorType = ErrorType.TooManyRequests;
                    break;
                case FirebaseAuthEmailException firebaseAuthEmailException:
                    errorType = ErrorType.Email;
                    break;
                case FirebaseAuthActionCodeException firebaseAuthActionCodeException:
                    errorType = ErrorType.ActionCode;
                    break;
                case FirebaseAuthInvalidUserException firebaseAuthInvalidUserException:
                    errorType = ErrorType.InvalidUser;
                    break;
                case FirebaseAuthWeakPasswordException firebaseAuthWeakPasswordException:
                    errorType = ErrorType.WeakPassword;
                    reason = firebaseAuthWeakPasswordException.Reason;
                    break;
                case FirebaseAuthUserCollisionException firebaseAuthUserCollisionException:
                    errorType = ErrorType.UserCollision;
                    break;
                case FirebaseAuthRecentLoginRequiredException firebaseAuthRecentLoginRequiredException:
                    errorType = ErrorType.RecentLoginRequired;
                    break;
                case FirebaseAuthInvalidCredentialsException firebaseAuthInvalidCredentialsException:
                    errorType = ErrorType.InvalidCredentials;
                    break;
            }

            return new FirebaseAuthException(exception.Message, exception, errorType, reason);
        }
    }
}
