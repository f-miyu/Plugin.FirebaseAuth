using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public partial interface IAuthCredential
    {
        internal AuthCredential ToNative();
    }
}
