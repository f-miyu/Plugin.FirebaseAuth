using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class AuthResultWrapper : IAuthResult
    {
        private readonly AuthDataResult _authResult;

        public IAdditionalUserInfo AdditionalUserInfo => _authResult.AdditionalUserInfo != null ? new AdditionalUserInfoWrapper(_authResult.AdditionalUserInfo) : null;

        public IUser User => _authResult.User != null ? new UserWrapper(_authResult.User) : null;

        public AuthResultWrapper(AuthDataResult authResult)
        {
            _authResult = authResult;
        }
    }
}
