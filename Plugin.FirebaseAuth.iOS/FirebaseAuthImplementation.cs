using System;

namespace Plugin.FirebaseAuth
{
    public class FirebaseAuthImplementation : IFirebaseAuth
    {
        public IInstance Instance => new InstanceWrapper();

        public IInstance GetInstance(string appName)
        {
            return new InstanceWrapper(appName);
        }
    }
}