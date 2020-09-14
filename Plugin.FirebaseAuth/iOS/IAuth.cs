using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public partial interface IAuth
    {
        internal Auth ToNative();
    }
}
