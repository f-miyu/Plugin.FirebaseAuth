using System;
namespace Plugin.FirebaseAuth
{
    public enum ErrorType
    {
        Other,
        NetWork,
        Email,
        ActionCode,
        InvalidUser,
        TooManyRequests,
        WeakPassword,
        UserCollision,
        InvalidCredentials,
        RecentLoginRequired,
    }
}
