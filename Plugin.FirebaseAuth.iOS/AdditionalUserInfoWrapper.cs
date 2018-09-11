using System;
using System.Collections.Generic;
using Firebase.Auth;
using System.Linq;

namespace Plugin.FirebaseAuth
{
    public class AdditionalUserInfoWrapper : IAdditionalUserInfo
    {
        internal AdditionalUserInfo AdditionalUserInfo { get; }

        private IDictionary<string, string> _profile;
        public IDictionary<string, string> Profile
        {
            get
            {
                if (_profile == null)
                {
                    _profile = new Dictionary<string, string>();
                    foreach (var (key, value) in AdditionalUserInfo.Profile)
                    {
                        _profile[key.ToString()] = value.ToString();
                    }
                }
                return _profile;
            }
        }

        public string ProviderId => AdditionalUserInfo.ProviderId;

        public string Username => AdditionalUserInfo.Username;

        public AdditionalUserInfoWrapper(AdditionalUserInfo additionalUserInfo)
        {
            AdditionalUserInfo = additionalUserInfo;
        }
    }
}
