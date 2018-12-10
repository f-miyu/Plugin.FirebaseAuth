using System;
namespace Plugin.FirebaseAuth
{
    public interface IEmailAuthProvider
    {
        string ProviderId { get; }
        string EmailPasswordSignInMethod { get; }
        string EmailLinkSignInMethod { get; }
        IAuthCredential GetCredential(string email, string password);
        IAuthCredential GetCredentialWithLink(string email, string emailLink);
    }
}
