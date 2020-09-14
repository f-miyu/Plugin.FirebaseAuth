using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public static class FirebaseAuth
    {
        public static IAuthUIDelegate? VerifyingPhoneNumberAuthUIDelegate { get; set; }
        public static IAuthUIDelegate? SignInWithProviderAuthUIDelegate { get; set; }
        public static IAuthUIDelegate? LinkWithProviderAuthUIDelegate { get; set; }
        public static IAuthUIDelegate? ReauthenticateWithProviderAuthUIDelegate { get; set; }
    }
}
