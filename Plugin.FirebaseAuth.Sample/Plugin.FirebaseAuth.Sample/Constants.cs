using System;
namespace Plugin.FirebaseAuth.Sample
{
    public static class Constants
    {
        public const string GoogleIosClientId = "";
        public const string GoogleIosUrlScheme = "";
        public const string GoogleIosRedirectUrl = GoogleIosUrlScheme + ":/oauth2redirect";

        public const string GoogleAndroidClientId = "";
        public const string GoogleAndroidUrlScheme = "";
        public const string GoogleAndroidRedirectUrl = GoogleAndroidUrlScheme + ":/oauth2redirect";

        public const string TwitterConsumerKey = "";
        public const string TwitterConsumerSecret = "";
        public const string TwitterRedirectUrl = UrlScheme + "://authorize";

        public const string FacebookClientId = "";
        public const string FacebookUrlScheme = "fb" + FacebookClientId;
        public const string FacebookRedirectUrl = FacebookUrlScheme + "://authorize";

        public const string GitHubClientId = "";
        public const string GitHubClientSecret = "";
        public const string GitHubRedirectUrl = UrlScheme + "://authorize";

        public const string UrlScheme = "firebase-auth-sample";
    }
}
