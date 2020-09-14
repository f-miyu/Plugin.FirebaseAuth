using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public partial interface IMultiFactorInfo
    {
        internal MultiFactorInfo ToNative();
    }
}
