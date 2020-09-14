using System;
using System.Collections.Generic;
using System.Linq;
using Android.Runtime;
using AndroidX.Collection;

namespace Plugin.FirebaseAuth
{
    public class AdditionalUserInfoWrapper : IAdditionalUserInfo, IEquatable<AdditionalUserInfoWrapper>
    {
        private readonly Firebase.Auth.IAdditionalUserInfo _additionalUserInfo;

        public AdditionalUserInfoWrapper(Firebase.Auth.IAdditionalUserInfo additionalUserInfo)
        {
            _additionalUserInfo = additionalUserInfo ?? throw new ArgumentNullException(nameof(additionalUserInfo));
        }

        public IDictionary<string, object?>? Profile => _additionalUserInfo.Profile?.ToDictionary(pair => pair.Key, pair => ValueConverter.Convert(pair.Value));

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
