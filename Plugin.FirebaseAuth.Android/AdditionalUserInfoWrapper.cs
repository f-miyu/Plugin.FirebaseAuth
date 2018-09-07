using System;
using System.Collections.Generic;

namespace Plugin.FirebaseAuth
{
    public class AdditionalUserInfoWrapper : IAdditionalUserInfo
    {
        internal Firebase.Auth.IAdditionalUserInfo AdditionalUserInfo { get; }

        public IDictionary<string, object> Profile => throw new NotImplementedException();

        public string ProviderId => AdditionalUserInfo.ProviderId;

        public string Username => AdditionalUserInfo.Username;

        public AdditionalUserInfoWrapper(Firebase.Auth.IAdditionalUserInfo additionalUserInfo)
        {
            AdditionalUserInfo = additionalUserInfo;
        }
    }
}
