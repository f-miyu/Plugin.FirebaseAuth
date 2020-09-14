using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public partial interface IUser
    {
        internal FirebaseUser ToNative();
    }
}
