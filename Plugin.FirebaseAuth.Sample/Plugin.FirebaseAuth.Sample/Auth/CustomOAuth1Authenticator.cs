using System;
using System.Collections.Generic;
using Xamarin.Auth;
namespace Plugin.FirebaseAuth.Sample.Auth
{
    public class CustomOAuth1Authenticator : OAuth1Authenticator
    {
        public CustomOAuth1Authenticator(string consumerKey, string consumerSecret, Uri requestTokenUrl, Uri authorizeUrl, Uri accessTokenUrl, Uri callbackUrl, GetUsernameAsyncFunc getUsernameAsync = null, bool isUsingNativeUI = false) : base(consumerKey, consumerSecret, requestTokenUrl, authorizeUrl, accessTokenUrl, callbackUrl, getUsernameAsync, isUsingNativeUI)
        {
        }

        public override void OnPageLoaded(Uri url)
        {
            try
            {
                base.OnPageLoaded(url);
            }
            catch (Exception)
            {
                OnCancelled();
            }
        }
    }
}
