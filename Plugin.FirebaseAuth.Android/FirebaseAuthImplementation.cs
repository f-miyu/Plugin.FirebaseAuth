using System;
using System.Threading.Tasks;
using Firebase;
using System.Linq;

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
