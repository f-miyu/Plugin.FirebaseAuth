using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class AuthResultWrapper : IAuthResult
    {
        internal AuthDataResult AuthResult { get; }

        private IAdditionalUserInfo _additionalUserInfo;
        public IAdditionalUserInfo AdditionalUserInfo => _additionalUserInfo ?? (_additionalUserInfo = new AdditionalUserInfoWrapper(AuthResult.AdditionalUserInfo));

        private IUser _user;
        public IUser User => _user ?? (_user = new UserWrapper((AuthResult.User)));

        public AuthResultWrapper(AuthDataResult authResult)
        {
            AuthResult = authResult;
        }
    }
}
