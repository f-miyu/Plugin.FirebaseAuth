using System;

namespace Plugin.FirebaseAuth
{
    public interface IFirebaseAuth
    {
        IInstance Instance { get; }
        IInstance GetInstance(string appName);
    }
}
