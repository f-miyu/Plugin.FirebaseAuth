using System;
namespace Plugin.FirebaseAuth
{
    public class AuthResultWrapper : IAuthResult
    {
        internal Firebase.Auth.IAuthResult AuthResult { get; }

        private IAdditionalUserInfo _additionalUserInfo;
        public IAdditionalUserInfo AdditionalUserInfo => _additionalUserInfo ?? (_additionalUserInfo = new AdditionalUserInfoWrapper(AuthResult.AdditionalUserInfo));

        private IUser _user;
        public IUser User => _user ?? (_user = new UserWrapper(AuthResult.User));

        public AuthResultWrapper(Firebase.Auth.IAuthResult authResult)
        {
            AuthResult = authResult;
        }
    }
}
