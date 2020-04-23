using System;
namespace Plugin.FirebaseAuth
{
    public interface IUserInfo
    {
        string DisplayName { get; }
        string Email { get; }
        string PhoneNumber { get; }
        Uri PhotoUrl { get; }
        string ProviderId { get; }
        string Uid { get; }
    }
}
