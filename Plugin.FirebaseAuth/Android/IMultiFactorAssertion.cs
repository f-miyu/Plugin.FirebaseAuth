using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public partial interface IMultiFactorAssertion
    {
        internal MultiFactorAssertion ToNative();
    }
}
