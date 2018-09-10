using System;
namespace Plugin.FirebaseAuth
{
    public enum ErrorType
    {
        Other,
        NetWork,
        Email,
        ActionCode,
        ApiNotAvailable,
        InvalidUser,
        TooManyRequests,
        WeakPassword,
        UserCollision,
        InvalidCredentials,
        RecentLoginRequired,
    }
}
