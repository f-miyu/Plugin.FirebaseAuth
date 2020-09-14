using System;
namespace Plugin.FirebaseAuth
{
    public partial interface IAuthCredential
    {
        string Provider { get; }
    }
}
