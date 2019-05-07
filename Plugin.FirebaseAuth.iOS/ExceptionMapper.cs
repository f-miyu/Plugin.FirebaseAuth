using System;
using Firebase.Auth;
using Foundation;
namespace Plugin.FirebaseAuth
{
    internal static class ExceptionMapper
    {
        public static Exception Map(NSErrorException exception)
        {
            var errorType = ErrorType.Other;
            string reason = null;
            var errorCode = (AuthErrorCode)(long)exception.Error.Code;
            switch (errorCode)
            {
                case AuthErrorCode.NetworkError:
                    errorType = ErrorType.NetWork;
                    break;
                case AuthErrorCode.TooManyRequests:
                    errorType = ErrorType.TooManyRequests;
                    break;
                case AuthErrorCode.InvalidRecipientEmail:
                case AuthErrorCode.InvalidSender:
                case AuthErrorCode.InvalidMessagePayload:
                    errorType = ErrorType.Email;
                    break;
                case AuthErrorCode.InvalidActionCode:
                case AuthErrorCode.ExpiredActionCode:
                    errorType = ErrorType.ActionCode;
                    break;
                case AuthErrorCode.UserDisabled:
                case AuthErrorCode.UserNotFound:
                case AuthErrorCode.UserTokenExpired:
                case AuthErrorCode.InvalidUserToken:
                    errorType = ErrorType.InvalidUser;
                    break;
                case AuthErrorCode.WeakPassword:
                    errorType = ErrorType.WeakPassword;
                    reason = exception.Error.UserInfo[NSError.LocalizedFailureReasonErrorKey] as NSString;
                    break;
                case AuthErrorCode.EmailAlreadyInUse:
                case AuthErrorCode.AccountExistsWithDifferentCredential:
                case AuthErrorCode.CredentialAlreadyInUse:
                    errorType = ErrorType.UserCollision;
                    break;
                case AuthErrorCode.RequiresRecentLogin:
                    errorType = ErrorType.RecentLoginRequired;
                    break;
                case AuthErrorCode.InvalidCredential:
                case AuthErrorCode.InvalidEmail:
                case AuthErrorCode.WrongPassword:
                case AuthErrorCode.InvalidCustomToken:
                case AuthErrorCode.CustomTokenMismatch:
                case AuthErrorCode.InvalidPhoneNumber:
                case AuthErrorCode.MissingPhoneNumber:
                case AuthErrorCode.InvalidVerificationID:
                case AuthErrorCode.MissingVerificationID:
                case AuthErrorCode.InvalidVerificationCode:
                case AuthErrorCode.MissingVerificationCode:
                    errorType = ErrorType.InvalidCredentials;
                    break;
            }

            return new FirebaseAuthException(exception.Error.LocalizedDescription, exception, errorType, reason);
        }
    }
}
