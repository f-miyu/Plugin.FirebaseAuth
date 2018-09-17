using System;

namespace Plugin.FirebaseAuth
{
    public class UserInfoWrapper : IUserInfo
    {
        internal Firebase.Auth.IUserInfo UserInfo { get; }

        public string DisplayName => UserInfo.DisplayName;

        public string Email => UserInfo.Email;

        public string PhoneNumber => UserInfo.PhoneNumber;

        public Uri PhotoUrl => UserInfo.PhotoUrl != null ? new Uri(UserInfo.PhotoUrl.AbsoluteString) : null;

        public string ProviderId => UserInfo.ProviderId;

        public string Uid => UserInfo.Uid;

        public UserInfoWrapper(Firebase.Auth.IUserInfo userInfo)
        {
            UserInfo = userInfo;
        }
    }
}
