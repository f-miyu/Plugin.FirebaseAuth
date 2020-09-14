using System;
using System.Linq;
using System.Reflection;
using Firebase.Auth;
using Foundation;

namespace Plugin.FirebaseAuth
{
    public partial class OAuthProvider
    {
        Firebase.Auth.IFederatedAuthProvider IFederatedAuthProvider.ToNative()
        {
            Firebase.Auth.OAuthProvider provider;

            if (_auth != null)
            {
                provider = Firebase.Auth.OAuthProvider.Create(_providerId, _auth.ToNative());
            }
            else
            {
                provider = Firebase.Auth.OAuthProvider.Create(_providerId);
            }

            if (CustomParameters != null)
            {
                provider.CustomParameters = new NSDictionary<NSString, NSString>(
                    CustomParameters.Keys.Select(x => new NSString(x)).ToArray(),
                    CustomParameters.Values.Select(x => new NSString(x)).ToArray());
            }

            if (Scopes != null)
            {
                provider.Scopes = Scopes.ToArray();
            }

            return provider;
        }
    }
}
