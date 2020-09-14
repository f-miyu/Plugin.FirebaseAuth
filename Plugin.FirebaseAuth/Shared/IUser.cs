using System;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
namespace Plugin.FirebaseAuth
{
    public partial interface IUser
    {
        string? DisplayName { get; }
        string? Email { get; }
        bool IsAnonymous { get; }
        string? PhoneNumber { get; }
        Uri? PhotoUrl { get; }
        IEnumerable<IUserInfo> ProviderData { get; }
        string ProviderId { get; }
        string Uid { get; }
        bool IsEmailVerified { get; }
        IUserMetadata? Metadata { get; }
        IMultiFactor MultiFactor { get; }
        Task DeleteAsync();
        Task<string?> GetIdTokenAsync(bool forceRefresh);
        Task<IAuthTokenResult> GetIdTokenResultAsync(bool forceRefresh);
        Task<IAuthResult> LinkWithCredentialAsync(IAuthCredential credential);
        Task<IAuthResult> LinkWithProviderAsync(IFederatedAuthProvider federatedAuthProvider);
        Task<IAuthResult> ReauthenticateAndRetrieveDataAsync(IAuthCredential credential);
        Task<IAuthResult> ReauthenticateWithProviderAsync(IFederatedAuthProvider federatedAuthProvider);
        Task ReauthenticateAsync(IAuthCredential credential);
        Task ReloadAsync();
        Task SendEmailVerificationAsync();
        Task SendEmailVerificationAsync(ActionCodeSettings actionCodeSettings);
        Task<IUser> UnlinkAsync(string providerId);
        Task UpdateEmailAsync(string email);
        Task UpdatePasswordAsync(string password);
        Task UpdatePhoneNumberAsync(IPhoneAuthCredential credential);
        Task UpdateProfileAsync(UserProfileChangeRequest request);
        Task VerifyBeforeUpdateEmail(string newEmail);
        Task VerifyBeforeUpdateEmail(string newEmail, ActionCodeSettings actionCodeSettings);
    }
}
