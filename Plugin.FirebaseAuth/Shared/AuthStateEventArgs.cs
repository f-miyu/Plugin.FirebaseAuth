using System;
namespace Plugin.FirebaseAuth
{
    public class AuthStateEventArgs : EventArgs
    {
        public AuthStateEventArgs(IAuth auth)
        {
            Auth = auth;
        }

        public IAuth Auth { get; }
    }
}
