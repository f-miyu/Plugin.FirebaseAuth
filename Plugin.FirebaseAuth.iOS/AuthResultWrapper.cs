using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class AuthResultWrapper : IAuthResult
    {
        internal AuthDataResult AuthResult { get; }

        private IAdditionalUserInfo _additionalUserInfo;
        public IAdditionalUserInfo AdditionalUserInfo
        {
            get
            {
                if (AuthResult.AdditionalUserInfo != null && _additionalUserInfo == null)
                {
                    _additionalUserInfo = new AdditionalUserInfoWrapper(AuthResult.AdditionalUserInfo);
                }
                return _additionalUserInfo;
            }
        }

        private IUser _user;
        public IUser User
        {
            get
            {
                if (AuthResult.User != null && _user == null)
                {
                    _user = new UserWrapper(AuthResult.User);
                }
                return _user;
            }
        }

        public AuthResultWrapper(AuthDataResult authResult)
        {
            AuthResult = authResult;
        }
    }
}
