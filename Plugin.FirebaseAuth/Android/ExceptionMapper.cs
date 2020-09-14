using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Java.Util;

namespace Plugin.FirebaseAuth
{
    internal static class ExceptionMapper
    {
        public static Exception Map(FirebaseException exception)
        {
            var message = exception.Message;

            switch (exception)
            {
                case FirebaseNetworkException _:
                    return new FirebaseAuthException(message, exception, ErrorType.NetWork, null);
                case FirebaseTooManyRequestsException _:
                    return new FirebaseAuthException(message, exception, ErrorType.TooManyRequests, null);
                case FirebaseApiNotAvailableException _:
                    return new FirebaseAuthException(message, exception, ErrorType.ApiNotAvailable, null);
                case FirebaseAuthEmailException emailException:
                    return new FirebaseAuthException(message, exception, ErrorType.Email, emailException.ErrorCode);
                case FirebaseAuthActionCodeException actionCodeException:
                    return new FirebaseAuthException(message, exception, ErrorType.ActionCode, actionCodeException.ErrorCode);
                case FirebaseAuthInvalidUserException invalidUserException:
                    return new FirebaseAuthException(message, exception, ErrorType.InvalidUser, invalidUserException.ErrorCode);
                case FirebaseAuthWeakPasswordException weakPasswordException:
                    return new FirebaseAuthException(message, exception, ErrorType.WeakPassword, weakPasswordException.ErrorCode, weakPasswordException.Reason);
                case FirebaseAuthUserCollisionException userCollisionException:
                    return new FirebaseAuthException(message, exception, ErrorType.UserCollision, userCollisionException.ErrorCode,
                        userCollisionException.Email,
                        userCollisionException.UpdatedCredential != null ? AuthCredentialWrapperFactory.Create(userCollisionException.UpdatedCredential) : null);
                case FirebaseAuthRecentLoginRequiredException recentLoginRequiredException:
                    return new FirebaseAuthException(message, exception, ErrorType.RecentLoginRequired, recentLoginRequiredException.ErrorCode);
                case FirebaseAuthInvalidCredentialsException invalidCredentialsException:
                    return new FirebaseAuthException(message, exception, ErrorType.InvalidCredentials, invalidCredentialsException.ErrorCode);
                case FirebaseAuthMultiFactorException multiFactorException:
                    return new FirebaseAuthException(message, exception, ErrorType.MultiFactor, multiFactorException.ErrorCode,
                        multiFactorException.Resolver != null ? new MultiFactorResolverWrapper(multiFactorException.Resolver) : null);
                case FirebaseAuthWebException webException:
                    return new FirebaseAuthException(message, exception, ErrorType.Web, webException.ErrorCode);
                default:
                    return new FirebaseAuthException(message, exception, ErrorType.Other, null);
            }
        }
    }
}
