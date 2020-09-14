using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class OAuthProviderWrapper : IOAuthProvider
    {
        public IAuthCredential GetCredential(string providerId, string idToken, string? accessToken = null, string? rawNonce = null)
        {
            var builder = Firebase.Auth.OAuthProvider.NewCredentialBuilder(providerId);

            if (rawNonce != null)
            {
                builder.SetIdTokenWithRawNonce(idToken, rawNonce);
            }
            else
            {
                builder.SetIdToken(idToken);
            }

            if (accessToken != null)
            {
                builder.SetAccessToken(accessToken);
            }

            return new OAuthCredentialWrapper((OAuthCredential)builder.Build());
        }
    }
}
