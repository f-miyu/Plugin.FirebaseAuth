using System;

namespace Plugin.FirebaseAuth
{
    public partial interface IFederatedAuthProvider
    {
        internal Firebase.Auth.IFederatedAuthProvider ToNative();
    }
}
