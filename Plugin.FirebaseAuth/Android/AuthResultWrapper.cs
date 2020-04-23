using System;
namespace Plugin.FirebaseAuth
{
    public class AuthResultWrapper : IAuthResult
    {
        private readonly Firebase.Auth.IAuthResult _authResult;

        public IAdditionalUserInfo AdditionalUserInfo => _authResult.AdditionalUserInfo != null ? new AdditionalUserInfoWrapper(_authResult.AdditionalUserInfo) : null;

        public IUser User => _authResult.User != null ? new UserWrapper(_authResult.User) : null;

        public AuthResultWrapper(Firebase.Auth.IAuthResult authResult)
        {
            _authResult = authResult;
        }
    }
}
