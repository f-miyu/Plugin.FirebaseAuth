using System;
namespace Plugin.FirebaseAuth
{
    public class UserEventArgs : EventArgs
    {
        public UserEventArgs(IUser user)
        {
            User = user;
        }

        public IUser User { get; }
    }
}
