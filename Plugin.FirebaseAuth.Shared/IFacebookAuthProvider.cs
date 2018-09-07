using System;
namespace Plugin.FirebaseAuth
{
    public interface IFacebookAuthProvider
    {
        string ProviderId { get; }
        IAuthCredential GetCredential(string accessToken);
    }
}
