using System;
using System.Collections.Generic;
using Xamarin.Auth;
namespace Plugin.FirebaseAuth.Sample.Auth
{
    public class CustomOAuth2Authenticator : OAuth2Authenticator
    {
        public CustomOAuth2Authenticator(string clientId, string scope, Uri authorizeUrl, Uri redirectUrl, GetUsernameAsyncFunc getUsernameAsync = null, bool isUsingNativeUI = false) : base(clientId, scope, authorizeUrl, redirectUrl, getUsernameAsync, isUsingNativeUI)
        {
        }

        public CustomOAuth2Authenticator(string clientId, string clientSecret, string scope, Uri authorizeUrl, Uri redirectUrl, Uri accessTokenUrl, GetUsernameAsyncFunc getUsernameAsync = null, bool isUsingNativeUI = false) : base(clientId, clientSecret, scope, authorizeUrl, redirectUrl, accessTokenUrl, getUsernameAsync, isUsingNativeUI)
        {
        }

        protected override void OnPageEncountered(Uri url, IDictionary<string, string> query, IDictionary<string, string> fragment)
        {
            base.OnPageEncountered(url, query, fragment);

            if (query.ContainsKey("error"))
            {
                OnCancelled();
            }
        }
    }
}
