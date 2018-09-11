using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.FirebaseAuth
{
    public class AdditionalUserInfoWrapper : IAdditionalUserInfo
    {
        internal Firebase.Auth.IAdditionalUserInfo AdditionalUserInfo { get; }

        private IDictionary<string, string> _profile;
        public IDictionary<string, string> Profile
        {
            get
            {
                if (_profile == null)
                {
                    _profile = AdditionalUserInfo.Profile.ToDictionary(pair => pair.Key, pair => pair.Value.ToString());
                }
                return _profile;
            }
        }

        public string ProviderId => AdditionalUserInfo.ProviderId;

        public string Username => AdditionalUserInfo.Username;

        public AdditionalUserInfoWrapper(Firebase.Auth.IAdditionalUserInfo additionalUserInfo)
        {
            AdditionalUserInfo = additionalUserInfo;
        }
    }
}
