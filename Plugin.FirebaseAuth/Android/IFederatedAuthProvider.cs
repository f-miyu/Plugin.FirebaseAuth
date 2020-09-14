using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public partial interface IFederatedAuthProvider
    {
        internal FederatedAuthProvider ToNative();
    }
}
