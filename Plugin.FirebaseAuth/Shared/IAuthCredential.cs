using System;
namespace Plugin.FirebaseAuth
{
    public interface IAuthCredential
    {
        string Provider { get; }
    }
}
