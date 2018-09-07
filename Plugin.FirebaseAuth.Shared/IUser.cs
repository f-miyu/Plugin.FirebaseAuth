using System;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
namespace Plugin.FirebaseAuth
{
    public interface IUser
    {
        string DisplayName { get; }
        string Email { get; }
        bool IsAnonymous { get; }
        string PhoneNumber { get; }
        Uri PhotoUrl { get; }
        IEnumerable<IUserInfo> ProviderData { get; }
        string ProviderId { get; }
        string Uid { get; }
        bool IsEmailVerified { get; }
        Task DeleteAsync();
        Task<string> GetIdTokenAsync(bool forceRefresh);
        Task LinkWithCredentialAsync(IAuthCredential credential);
        Task<IAuthResult> ReauthenticateAndRetrieveDataAsync(IAuthCredential credential);
        Task ReloadAsync();
        Task<IUser> UnlinkAsync(string providerId);
        Task UpdateEmailAsync(string email);
        Task UpdatePasswordAsync(string password);
        Task UpdatePhoneNumberAsync(IPhoneAuthCredential credential);
        Task UpdateProfileAsync(UserProfileChangeRequest request);
    }
}
