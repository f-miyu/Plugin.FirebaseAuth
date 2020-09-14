using System;
using System.Collections.Generic;
using System.Globalization;
using Firebase.Auth;
using Foundation;
namespace Plugin.FirebaseAuth
{
    internal static class ExceptionMapper
    {
        public static Exception Map(NSErrorException exception)
        {
            var authErrorCode = (AuthErrorCode)(long)exception.Error.Code;

            var userInfo = exception.Error.UserInfo;
            var errorCode = userInfo[Auth.ErrorUserInfoNameKey] as NSString;
            var message = userInfo[NSError.LocalizedDescriptionKey] as NSString;

            switch (authErrorCode)
            {
                case AuthErrorCode.NetworkError:
                case AuthErrorCode.WebNetworkRequestFailed:
                    return new FirebaseAuthException(message, exception, ErrorType.NetWork, errorCode);
                case AuthErrorCode.InvalidMessagePayload:
                case AuthErrorCode.InvalidSender:
                case AuthErrorCode.InvalidRecipientEmail:
                    return new FirebaseAuthException(message, exception, ErrorType.Email, errorCode);
                case AuthErrorCode.ExpiredActionCode:
                case AuthErrorCode.InvalidActionCode:
                    return new FirebaseAuthException(message, exception, ErrorType.ActionCode, errorCode);
                case AuthErrorCode.UserDisabled:
                case AuthErrorCode.UserNotFound:
                case AuthErrorCode.InvalidUserToken:
                case AuthErrorCode.UserTokenExpired:
                    return new FirebaseAuthException(message, exception, ErrorType.InvalidUser, errorCode);
                case AuthErrorCode.TooManyRequests:
                case AuthErrorCode.QuotaExceeded:
                    return new FirebaseAuthException(message, exception, ErrorType.TooManyRequests, errorCode);
                case AuthErrorCode.WeakPassword:
                    var reason = userInfo[NSError.LocalizedFailureReasonErrorKey] as NSString;
                    return new FirebaseAuthException(message, exception, ErrorType.WeakPassword, errorCode, reason);
                case AuthErrorCode.EmailAlreadyInUse:
                case AuthErrorCode.AccountExistsWithDifferentCredential:
                case AuthErrorCode.CredentialAlreadyInUse:
                    var email = userInfo[Auth.ErrorUserInfoEmailKey] as NSString;
                    var updateCredential = userInfo[Auth.ErrorUserInfoUpdatedCredentialKey] is AuthCredential credential
                        ? AuthCredentialWrapperFactory.Create(credential) : null;
                    return new FirebaseAuthException(message, exception, ErrorType.UserCollision, errorCode, email, updateCredential);
                case AuthErrorCode.InvalidCustomToken:
                case AuthErrorCode.CustomTokenMismatch:
                case AuthErrorCode.InvalidCredential:
                case AuthErrorCode.InvalidEmail:
                case AuthErrorCode.WrongPassword:
                case AuthErrorCode.UserMismatch:
                case AuthErrorCode.MissingEmail:
                case (AuthErrorCode)17035:
                case AuthErrorCode.MissingPhoneNumber:
                case AuthErrorCode.InvalidPhoneNumber:
                case AuthErrorCode.MissingVerificationCode:
                case AuthErrorCode.InvalidVerificationCode:
                case AuthErrorCode.MissingVerificationID:
                case AuthErrorCode.InvalidVerificationID:
                case (AuthErrorCode)17049:
                case AuthErrorCode.SessionExpired:
                case AuthErrorCode.RejectedCredential:
                case (AuthErrorCode)17077:
                case AuthErrorCode.MissingMultiFactorSession:
                case AuthErrorCode.MissingMultiFactorInfo:
                case AuthErrorCode.InvalidMultiFactorSession:
                case AuthErrorCode.MultiFactorInfoNotFound:
                case AuthErrorCode.MissingOrInvalidNonce:
                    return new FirebaseAuthException(message, exception, ErrorType.InvalidCredentials, errorCode);
                case AuthErrorCode.RequiresRecentLogin:
                    return new FirebaseAuthException(message, exception, ErrorType.RecentLoginRequired, errorCode);
                case AuthErrorCode.SecondFactorRequired:
                    var resolver = userInfo[Auth.ErrorUserInfoMultiFactorResolverKey] is MultiFactorResolver multiFactorResolver
                        ? new MultiFactorResolverWrapper(multiFactorResolver) : null;
                    return new FirebaseAuthException(message, exception, ErrorType.MultiFactor, errorCode, resolver);
                case AuthErrorCode.WebContextAlreadyPresented:
                case AuthErrorCode.WebContextCancelled:
                case AuthErrorCode.WebInternalError:
                case (AuthErrorCode)17065:
                    return new FirebaseAuthException(message, exception, ErrorType.Web, errorCode);
                case AuthErrorCode.WebSignInUserInteractionFailure:
                case (AuthErrorCode)17080:
                    return new FirebaseAuthException(message, exception, ErrorType.ApiNotAvailable, errorCode);
                default:
                    return new FirebaseAuthException(message, exception, ErrorType.Other, errorCode);
            }
        }

        public static Exception Map(NSError error)
        {
            return Map(new NSErrorException(error));
        }
    }
}
