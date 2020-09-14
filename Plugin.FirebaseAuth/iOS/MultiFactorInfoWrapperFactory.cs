using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    internal static class MultiFactorInfoWrapperFactory
    {
        public static IMultiFactorInfo Create(MultiFactorInfo multiFactorInfo)
        {
            return multiFactorInfo switch
            {
                PhoneMultiFactorInfo phoneMultiFactorInfo => new PhoneMultiFactorInfoWrapper(phoneMultiFactorInfo),
                _ => new MultiFactorInfoWrapper(multiFactorInfo)
            };
        }
    }
}
