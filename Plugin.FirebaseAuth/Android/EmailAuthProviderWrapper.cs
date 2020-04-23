using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class EmailAuthProviderWrapper : IEmailAuthProvider
    {
        public string ProviderId => EmailAuthProvider.ProviderId;

        public string EmailPasswordSignInMethod => EmailAuthProvider.EmailPasswordSignInMethod;

        public string EmailLinkSignInMethod => EmailAuthProvider.EmailLinkSignInMethod;

        public IAuthCredential GetCredential(string email, string password)
        {
            var credential = EmailAuthProvider.GetCredential(email, password);
            return new AuthCredentialWrapper(credential);
        }

        public IAuthCredential GetCredentialWithLink(string email, string emailLink)
        {
            var credential = EmailAuthProvider.GetCredentialWithLink(email, emailLink);
            return new AuthCredentialWrapper(credential);
        }
    }
}
