using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public partial interface IMultiFactorSession
    {
        internal MultiFactorSession ToNative();
    }
}
