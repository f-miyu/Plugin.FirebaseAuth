using System;
using System.Threading.Tasks;

namespace Plugin.FirebaseAuth
{
    public interface IFirebaseAuth
    {
        event EventHandler<UserEventArgs> AuthStateChanged;
        event EventHandler<UserEventArgs> IdTokenChanged;
        IEmailAuthProvider EmailAuthProvider { get; }
        IGoogleAuthProvider GoogleAuthProvider { get; }
        IFacebookAuthProvider FacebookAuthProvider { get; }
        ITwitterAuthProvider TwitterAuthProvider { get; }
        IGitHubAuthProvider GitHubAuthProvider { get; }
        IPhoneAuthProvider PhoneAuthProvider { get; }
        IUser CurrentUser { get; }
        string LanguageCode { get; set; }
        Task<IAuthResult> CreateUserWithEmailAndPasswordAsync(string email, string password);
        Task<IAuthResult> SignInAnonymouslyAsync();
        Task<IUser> SignInWithCustomTokenAsync(string token);
        Task<IAuthResult> SignInWithCredentialAsync(IAuthCredential credential);
        Task<IAuthResult> SignInWithEmailAndPasswordAsync(string email, String password);
        Task FetchProvidersForEmailAsync(string email);
        Task SendPasswordResetEmailAsync(string email);
        Task SendPasswordResetEmailAsync(string email, ActionCodeSettings settings);
        Task ApplyActionCodeAsync(string code);
        Task CheckActionCodeAsync(string code);
        Task ConfirmPasswordResetAsync(string email, string newPassword);
        Task VerifyPasswordResetCodeAsync(string code);
        void SignOut();
        void UseAppLanguage();
    }
}
