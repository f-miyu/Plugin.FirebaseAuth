using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Foundation;

namespace Plugin.FirebaseAuth
{
    public class FirebaseAuthImplementation : IFirebaseAuth
    {
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

        private readonly Auth _instance = Auth.DefaultInstance;


        public async Task<IAuthResult> CreateUserWithEmailAndPasswordAsync(string email, string password)
        {
            try
            {
                var result = await _instance.CreateUserAsync(email, password).ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (NSErrorException e)
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
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthResult> SignInWithCredentialAsync(IAuthCredential credential)
        {
            try
            {
                var wrapper = (AuthCredentialWrapper)credential;
                var result = await _instance.SignInAndRetrieveDataWithCredentialAsync(wrapper.AuthCredential).ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IUser> SignInWithCustomTokenAsync(string token)
        {
            try
            {
                var result = await _instance.SignInWithCustomTokenAsync(token).ConfigureAwait(false);
                return new UserWrapper(result);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthResult> SignInWithEmailAndPasswordAsync(string email, string password)
        {
            try
            {
                var result = await _instance.SignInWithPasswordAsync(email, password).ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task FetchProvidersForEmailAsync(string email)
        {
            try
            {
                return _instance.FetchProvidersAsync(email);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task SendPasswordResetEmailAsync(string email)
        {
            try
            {
                return _instance.SendPasswordResetAsync(email);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task SendPasswordResetEmailAsync(string email, ActionCodeSettings settings)
        {
            try
            {
                var actionCodeSettings = new Firebase.Auth.ActionCodeSettings();

                if (settings.IsUrlChanged)
                {
                    actionCodeSettings.Url = new NSUrl(settings.Url);
                }
                if (settings.IsIosBundleIdChanged)
                {
                    actionCodeSettings.IOSBundleId = settings.IosBundleId;
                }
                if (settings.IsAndroidPackageChanged)
                {
                    actionCodeSettings.SetAndroidPackageName(settings.AndroidPackageName,
                                                             settings.AndroidInstallIfNotAvailable,
                                                             settings.AndroidMinimumVersion);
                }
                if (settings.IsHandleCodeInAppChanged)
                {
                    actionCodeSettings.HandleCodeInApp = settings.HandleCodeInApp;
                }

                return _instance.SendPasswordResetAsync(email, actionCodeSettings);
            }
            catch (NSErrorException e)
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
            catch (NSErrorException e)
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
            catch (NSErrorException e)
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
            catch (NSErrorException e)
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
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public void SignOut()
        {
            _instance.SignOut(out var error);

            if (error != null)
            {
                throw ExceptionMapper.Map(new NSErrorException(error));
            }
        }

        public void UseAppLanguage()
        {
            _instance.UseAppLanguage();
        }

        public IListenerRegistration AddAuthStateChangedListener(AuthStateChangedHandler listener)
        {
            return new AuthStateChangedListenerRegistration(_instance, listener);
        }

        public IListenerRegistration AddIdTokenChangedListener(IdTokenChangedHandler listener)
        {
            return new IdTokenChangedListenerRegistration(_instance, listener);
        }

        private class AuthStateChangedListenerRegistration : IListenerRegistration
        {
            private readonly Auth _instance;
            private NSObject _listner;

            public AuthStateChangedListenerRegistration(Auth instance, AuthStateChangedHandler handler)
            {
                _instance = instance;
                _listner = _instance.AddAuthStateDidChangeListener((Auth auth, User user) =>
                {
                    IUser userWrapper = null;
                    if (user != null)
                    {
                        userWrapper = new UserWrapper(user);
                    }
                    handler?.Invoke(userWrapper);
                });
            }

            public void Remove()
            {
                if (_listner != null)
                {
                    _instance.RemoveAuthStateDidChangeListener(_listner);
                    _listner = null;
                }
            }
        }

        private class IdTokenChangedListenerRegistration : IListenerRegistration
        {
            private readonly Auth _instance;
            private NSObject _listner;

            public IdTokenChangedListenerRegistration(Auth instance, IdTokenChangedHandler handler)
            {
                _instance = instance;
                _listner = _instance.AddIdTokenDidChangeListener((Auth auth, User user) =>
                {
                    IUser userWrapper = null;
                    if (user != null)
                    {
                        userWrapper = new UserWrapper(user);
                    }
                    handler?.Invoke(userWrapper);
                });
            }

            public void Remove()
            {
                if (_listner != null)
                {
                    _instance.RemoveIdTokenDidChangeListener(_listner);
                    _listner = null;
                }
            }
        }
    }
}