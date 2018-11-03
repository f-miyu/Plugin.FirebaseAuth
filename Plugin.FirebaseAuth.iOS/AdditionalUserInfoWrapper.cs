using System;
using System.Collections.Generic;
using Firebase.Auth;
using System.Linq;
using Foundation;

namespace Plugin.FirebaseAuth
{
    public class AdditionalUserInfoWrapper : IAdditionalUserInfo
    {
        private readonly AdditionalUserInfo _additionalUserInfo;

        private IDictionary<string, object> _profile;
        public IDictionary<string, object> Profile
        {
            get
            {
                if (_additionalUserInfo.Profile != null && _profile == null)
                {
                    _profile = new Dictionary<string, Object>();
                    foreach (var (key, value) in _additionalUserInfo.Profile)
                    {
                        _profile[key.ToString()] = ConvertProfileValue(value);
                    }
                }
                return _profile;
            }
        }

        public string ProviderId => _additionalUserInfo.ProviderId;

        public string Username => _additionalUserInfo.Username;

        public AdditionalUserInfoWrapper(AdditionalUserInfo additionalUserInfo)
        {
            _additionalUserInfo = additionalUserInfo;
        }

        private object ConvertProfileValue(NSObject profileValue)
        {
            if (profileValue == null)
                return null;

            switch (profileValue)
            {
                case NSNumber number:
                    return number.DoubleValue;
                case NSString @string:
                    return @string.ToString();
                case NSArray array:
                    {
                        var list = new List<object>();
                        for (nuint i = 0; i < array.Count; i++)
                        {
                            list.Add(ConvertProfileValue(array.GetItem<NSObject>(i)));
                        }
                        return list;
                    }
                case NSDictionary dictionary:
                    {
                        var dict = new Dictionary<string, object>();
                        foreach (var (key, value) in dictionary)
                        {
                            dict.Add(key.ToString(), ConvertProfileValue(value));
                        }
                        return dict;
                    }
                case NSNull @null:
                    return null;
                default:
                    return profileValue;
            }
        }
    }
}
