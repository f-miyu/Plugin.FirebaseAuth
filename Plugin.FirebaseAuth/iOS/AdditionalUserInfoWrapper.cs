using System;
using System.Collections.Generic;
using Firebase.Auth;
using System.Linq;
using Foundation;

namespace Plugin.FirebaseAuth
{
    public class AdditionalUserInfoWrapper : IAdditionalUserInfo, IEquatable<AdditionalUserInfoWrapper>
    {
        private readonly AdditionalUserInfo _additionalUserInfo;

        public AdditionalUserInfoWrapper(AdditionalUserInfo additionalUserInfo)
        {
            _additionalUserInfo = additionalUserInfo ?? throw new ArgumentNullException(nameof(additionalUserInfo));
        }

        public IDictionary<string, object?>? Profile
        {
            get
            {
                if (_additionalUserInfo.Profile == null) return null;

                var profile = new Dictionary<string, object?>();
                foreach (var (key, value) in _additionalUserInfo.Profile)
                {
                    profile[key.ToString()] = ValueConverter.Convert(value);
                }
                return profile;
            }
        }

        public string? ProviderId => _additionalUserInfo.ProviderId;

        public string? Username => _additionalUserInfo.Username;

        public bool IsNewUser => _additionalUserInfo.IsNewUser;

        public override bool Equals(object? obj)
        {
            return Equals(obj as AdditionalUserInfoWrapper);
        }

        public bool Equals(AdditionalUserInfoWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_additionalUserInfo, other._additionalUserInfo)) return true;
            return _additionalUserInfo.Equals(other._additionalUserInfo);
        }

        public override int GetHashCode()
        {
            return _additionalUserInfo.GetHashCode();
        }
    }
}
