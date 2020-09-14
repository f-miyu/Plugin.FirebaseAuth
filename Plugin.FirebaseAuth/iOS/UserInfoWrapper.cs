using System;

namespace Plugin.FirebaseAuth
{
    public class UserInfoWrapper : IUserInfo, IEquatable<UserInfoWrapper>
    {
        private readonly Firebase.Auth.IUserInfo _userInfo;

        public UserInfoWrapper(Firebase.Auth.IUserInfo userInfo)
        {
            _userInfo = userInfo ?? throw new ArgumentNullException(nameof(userInfo));
        }

        public string? DisplayName => _userInfo.DisplayName;

        public string? Email => _userInfo.Email;

        public string? PhoneNumber => _userInfo.PhoneNumber;

        public Uri? PhotoUrl => _userInfo.PhotoUrl != null ? new Uri(_userInfo.PhotoUrl.AbsoluteString) : null;

        public string ProviderId => _userInfo.ProviderId;

        public string Uid => _userInfo.Uid;

        public override bool Equals(object? obj)
        {
            return Equals(obj as UserInfoWrapper);
        }

        public bool Equals(UserInfoWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_userInfo, other._userInfo)) return true;
            return _userInfo.Equals(other._userInfo);
        }

        public override int GetHashCode()
        {
            return _userInfo.GetHashCode();
        }
    }
}
