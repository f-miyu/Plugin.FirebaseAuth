using System;
using System.Threading.Tasks;
using Firebase;
namespace Plugin.FirebaseAuth
{
    public class FirebaseAuthImplementation : IFirebaseAuth
    {
        public event EventHandler<UserEventArgs> AuthStateChanged;
        public event EventHandler<UserEventArgs> IdTokenChanged;

        public IEmailAuthProvider EmailAuthProvider { get; } = new EmailAuthProviderWrapper();

        public IGoogleAuthProvider GoogleAuthProvider { get; } = new GoogleAuthProviderWrapper();

        public IFacebookAuthProvider FacebookAuthProvider { get; } = new FacebookAuthProviderWrapper();

        public ITwitterAuthProvider TwitterAuthProvider { get; } = new TwitterAuthProviderWrapper();

        public IGitHubAuthProvider GitHubAuthProvider { get; } = new GitHubAuthProviderWrapper();

        public IPhoneAuthProvider PhoneAuthProvider { get; } = new PhoneAuthProviderWrapper();

        public IUser CurrentUser
        {
            get
            {
                if (_instance.CurrentUser != null)
                {
                    return new UserWrapper(_instance.CurrentUser);
                }
                return null;
            }
        }

        public string LanguageCode
        {
            get => _instance.LanguageCode;
            set => _instance.LanguageCode = value;
        }

        private readonly Firebase.Auth.FirebaseAuth _instance = Firebase.Auth.FirebaseAuth.Instance;

        public FirebaseAuthImplementation()
        {
            _instance.AuthState += (object sender, Firebase.Auth.FirebaseAuth.AuthStateEventArgs e) =>
            {
                IUser user = null;
                if (e.Auth.CurrentUser != null)
                {
                    user = new UserWrapper(e.Auth.CurrentUser);
                }

                AuthStateChanged?.Invoke(this, new UserEventArgs(user));
            };

            _instance.IdToken += (object sender, Firebase.Auth.FirebaseAuth.IdTokenEventArgs e) =>
            {
                IUser user = null;
                if (e.Auth.CurrentUser != null)
                {
                    user = new UserWrapper(e.Auth.CurrentUser);
                }
                IdTokenChanged?.Invoke(this, new UserEventArgs(user));
            };
        }

        public async Task<IAuthResult> CreateUserWithEmailAndPasswordAsync(string email, string password)
        {
            try
            {
                var result = await _instance.CreateUserWithEmailAndPasswordAsync(email, password).ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthResult> SignInAnonymouslyAsync()
        {
            try
            {
                var result = await _instance.SignInAnonymouslyAsync().ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthResult> SignInWithCredentialAsync(IAuthCredential credential)
        {
            try
            {
                var wrapper = (AuthCredentialWrapper)credential;
                var result = await _instance.SignInWithCredentialAsync(wrapper.AuthCredential).ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IUser> SignInWithCustomTokenAsync(string token)
        {
            try
            {
                var result = await _instance.SignInWithCustomTokenAsync(token).ConfigureAwait(false);
                return new UserWrapper(result.User);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthResult> SignInWithEmailAndPasswordAsync(string email, string password)
        {
            try
            {
                var result = await _instance.SignInWithEmailAndPasswordAsync(email, password).ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task FetchProvidersForEmailAsync(string email)
        {
            try
            {
                return _instance.FetchProvidersForEmailAsync(email);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task SendPasswordResetEmailAsync(string email)
        {
            try
            {
                return _instance.SendPasswordResetEmailAsync(email);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task SendPasswordResetEmailAsync(string email, ActionCodeSettings settings)
        {
            try
            {
                var builder = Firebase.Auth.ActionCodeSettings.NewBuilder();

                if (settings.IsUrlChanged)
                {
                    builder.SetUrl(settings.Url);
                }
                if (settings.IsIosBundleIdChanged)
                {
                    builder.SetIOSBundleId(settings.IosBundleId);
                }
                if (settings.IsAndroidPackageChanged)
                {
                    builder.SetAndroidPackageName(settings.AndroidPackageName,
                                                             settings.AndroidInstallIfNotAvailable,
                                                             settings.AndroidMinimumVersion);
                }
                if (settings.IsHandleCodeInAppChanged)
                {
                    builder.SetHandleCodeInApp(settings.HandleCodeInApp);
                }

                return _instance.SendPasswordResetEmailAsync(email, builder.Build());
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task ApplyActionCodeAsync(string code)
        {
            try
            {
                return _instance.ApplyActionCodeAsync(code);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task CheckActionCodeAsync(string code)
        {
            try
            {
                return _instance.CheckActionCodeAsync(code);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task ConfirmPasswordResetAsync(string email, string newPassword)
        {
            try
            {
                return _instance.ConfirmPasswordResetAsync(email, newPassword);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task VerifyPasswordResetCodeAsync(string code)
        {
            try
            {
                return _instance.VerifyPasswordResetCodeAsync(code);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public void SignOut()
        {
            try
            {
                _instance.SignOut();
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public void UseAppLanguage()
        {
            _instance.UseAppLanguage();
        }
    }
}
