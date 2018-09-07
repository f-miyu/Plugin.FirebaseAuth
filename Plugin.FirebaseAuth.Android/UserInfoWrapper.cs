using System;
namespace Plugin.FirebaseAuth
{
    public class UserInfoWrapper : IUserInfo
    {
        internal Firebase.Auth.IUserInfo UserInfo { get; }

        public string DisplayName => UserInfo.DisplayName;

        public string Email => UserInfo.Email;

        public string PhoneNumber => UserInfo.PhoneNumber;

        private Uri _photoUrl;
        public Uri PhotoUrl => _photoUrl ?? (_photoUrl = new Uri(UserInfo.ToString()));

        public string ProviderId => UserInfo.ProviderId;

        public string Uid => UserInfo.Uid;

        public UserInfoWrapper(Firebase.Auth.IUserInfo userInfo)
        {
            UserInfo = userInfo;
        }
    }
}
