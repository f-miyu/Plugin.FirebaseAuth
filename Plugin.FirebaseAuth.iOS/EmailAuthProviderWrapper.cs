using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class EmailAuthProviderWrapper : IEmailAuthProvider
    {
        public string ProviderId => EmailAuthProvider.Id;

        public string EmailPasswordSignInMethod => EmailAuthProvider.PasswordSignInMethod;

        public string EmailLinkSignInMethod => "emailLink";

        public IAuthCredential GetCredential(string email, string password)
        {
            var credential = EmailAuthProvider.GetCredentialFromPassword(email, password);
            return new AuthCredentialWrapper(credential);
        }

        public IAuthCredential GetCredentialWithLink(string email, string emailLink)
        {
            var credential = EmailAuthProvider.GetCredentialFromLink(email, emailLink);
            return new AuthCredentialWrapper(credential);
        }
    }
}
