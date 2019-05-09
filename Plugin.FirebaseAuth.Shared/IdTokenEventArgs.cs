using System;
namespace Plugin.FirebaseAuth
{
    public class IdTokenEventArgs : EventArgs
    {
        public IAuth Auth { get; }

        public IdTokenEventArgs(IAuth auth)
        {
            Auth = auth;
        }
    }
}
