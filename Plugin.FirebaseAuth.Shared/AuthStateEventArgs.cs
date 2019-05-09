using System;
namespace Plugin.FirebaseAuth
{
    public class AuthStateEventArgs : EventArgs
    {
        public IAuth Auth { get; }

        public AuthStateEventArgs(IAuth auth)
        {
            Auth = auth;
        }
    }
}
