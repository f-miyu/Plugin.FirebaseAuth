using System;

namespace Plugin.FirebaseAuth
{
    public interface IFirebaseAuth
    {
        IAuth Instance { get; }
        IAuth GetInstance(string appName);
    }
}
