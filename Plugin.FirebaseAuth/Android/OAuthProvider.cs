using System;
using System.Linq;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public partial class OAuthProvider
    {
        FederatedAuthProvider IFederatedAuthProvider.ToNative()
        {
            Firebase.Auth.OAuthProvider.Builder builder;
            if (_auth != null)
            {
                builder = Firebase.Auth.OAuthProvider.NewBuilder(_providerId, _auth.ToNative());
            }
            else
            {
                builder = Firebase.Auth.OAuthProvider.NewBuilder(_providerId);
            }

            if (CustomParameters != null)
            {
                builder.AddCustomParameters(CustomParameters);
            }

            if (Scopes != null)
            {
                builder.SetScopes(Scopes.ToList());
            }

            return builder.Build();
        }
    }
}
