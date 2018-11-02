using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public static class FirebaseAuth
    {
        public static string DefaultAppName { get; set; }

        public static IAuthUIDelegate VerifyingPhoneNumberAuthUIDelegate { get; set; }
    }
}
