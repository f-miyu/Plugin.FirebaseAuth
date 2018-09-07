using System;
namespace Plugin.FirebaseAuth
{
    public interface IEmailAuthProvider
    {
        string ProviderId { get; }
        IAuthCredential GetCredential(string email, string password);
    }
}
