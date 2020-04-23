using System;
namespace Plugin.FirebaseAuth
{
    public interface IAuthResult
    {
        IAdditionalUserInfo AdditionalUserInfo { get; }
        IUser User { get; }
    }
}
