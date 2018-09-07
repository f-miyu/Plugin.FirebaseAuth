using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class EmailAuthProviderWrapper : IEmailAuthProvider
    {
        public string ProviderId => EmailAuthProvider.ProviderId;

        public IAuthCredential GetCredential(string email, string password)
        {
            var credential = EmailAuthProvider.GetCredential(email, password);
            return new AuthCredentialWrapper(credential);
        }
    }
}
