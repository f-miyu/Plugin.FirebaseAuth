using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class AuthResultWrapper : IAuthResult
    {
        private readonly AuthDataResult _authResult;

        private IAdditionalUserInfo _additionalUserInfo;
        public IAdditionalUserInfo AdditionalUserInfo
        {
            get
            {
                if (_authResult.AdditionalUserInfo != null && _additionalUserInfo == null)
                {
                    _additionalUserInfo = new AdditionalUserInfoWrapper(_authResult.AdditionalUserInfo);
                }
                return _additionalUserInfo;
            }
        }

        private IUser _user;
        public IUser User
        {
            get
            {
                if (_authResult.User != null && _user == null)
                {
                    _user = new UserWrapper(_authResult.User);
                }
                return _user;
            }
        }

        public AuthResultWrapper(AuthDataResult authResult)
        {
            _authResult = authResult;
        }
    }
}
