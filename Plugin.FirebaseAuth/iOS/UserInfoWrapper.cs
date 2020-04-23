using System;

namespace Plugin.FirebaseAuth
{
    public class UserInfoWrapper : IUserInfo
    {
        private readonly Firebase.Auth.IUserInfo _userInfo;

        public string DisplayName => _userInfo.DisplayName;

        public string Email => _userInfo.Email;

        public string PhoneNumber => _userInfo.PhoneNumber;

        public Uri PhotoUrl => _userInfo.PhotoUrl != null ? new Uri(_userInfo.PhotoUrl.AbsoluteString) : null;

        public string ProviderId => _userInfo.ProviderId;

        public string Uid => _userInfo.Uid;

        public UserInfoWrapper(Firebase.Auth.IUserInfo userInfo)
        {
            _userInfo = userInfo;
        }
    }
}
