using System;
using System.Threading.Tasks;

namespace Plugin.FirebaseAuth
{
    public delegate void AuthStateChangedHandler(IUser user);
    public delegate void IdTokenChangedHandler(IUser user);

    public interface IAuth
    {
        IUser CurrentUser { get; }
        string LanguageCode { get; set; }
        Task<IAuthResult> CreateUserWithEmailAndPasswordAsync(string email, string password);
        Task<IAuthResult> SignInAnonymouslyAsync();
        Task<IUser> SignInWithCustomTokenAsync(string token);
        Task<IAuthResult> SignInWithCredentialAsync(IAuthCredential credential);
        Task<IAuthResult> SignInWithEmailAndPasswordAsync(string email, string password);
        Task<IAuthResult> SignInWithEmailLinkAsync(string email, string link);
        Task<string[]> FetchProvidersForEmailAsync(string email);
        Task<string[]> FetchSignInMethodsForEmailAsync(string email);
        Task SendPasswordResetEmailAsync(string email);
        Task SendPasswordResetEmailAsync(string email, ActionCodeSettings actionCodeSettings);
        Task SendSignInLinkToEmailAsync(string email, ActionCodeSettings actionCodeSettings);
        Task ApplyActionCodeAsync(string code);
        Task CheckActionCodeAsync(string code);
        Task ConfirmPasswordResetAsync(string code, string newPassword);
        Task<string> VerifyPasswordResetCodeAsync(string code);
        Task UpdateCurrentUserAsync(IUser user);
        void SignOut();
        void UseAppLanguage();
        bool IsSignInWithEmailLink(string link);
        IListenerRegistration AddAuthStateChangedListener(AuthStateChangedHandler listener);
        IListenerRegistration AddIdTokenChangedListener(IdTokenChangedHandler listener);
    }
}
