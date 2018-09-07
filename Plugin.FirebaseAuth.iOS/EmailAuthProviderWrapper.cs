using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class EmailAuthProviderWrapper : IEmailAuthProvider
    {
        public string ProviderId => EmailAuthProvider.Id;

        public IAuthCredential GetCredential(string email, string password)
        {
            var credential = EmailAuthProvider.GetCredentialFromPassword(email, password);
            return new AuthCredentialWrapper(credential);
        }
    }
}
