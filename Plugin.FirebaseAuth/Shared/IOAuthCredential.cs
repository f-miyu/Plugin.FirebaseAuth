using System;
namespace Plugin.FirebaseAuth
{
    public interface IOAuthCredential : IAuthCredential
    {
        string AccessToken { get; }
        string IdToken { get; }
        string Secret { get; }
    }
}
