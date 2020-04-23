using System;
namespace Plugin.FirebaseAuth
{
    public interface IFacebookAuthProvider
    {
        string ProviderId { get; }
        string FacebookSignInMethod { get; }
        IAuthCredential GetCredential(string accessToken);
    }
}
