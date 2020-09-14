using System;
namespace Plugin.FirebaseAuth
{
    public partial interface IAuth
    {
        internal Firebase.Auth.FirebaseAuth ToNative();
    }
}
