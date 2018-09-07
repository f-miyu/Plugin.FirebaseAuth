using System;
using System.Collections.Generic;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class AdditionalUserInfoWrapper : IAdditionalUserInfo
    {
        internal AdditionalUserInfo AdditionalUser { get; }

        public IDictionary<string, object> Profile => throw new NotImplementedException();

        public string ProviderId => AdditionalUser.ProviderId;

        public string Username => AdditionalUser.Username;

        public AdditionalUserInfoWrapper(AdditionalUserInfo additionalUserInfo)
        {
            AdditionalUser = additionalUserInfo;
        }
    }
}
