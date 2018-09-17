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

        public IOAuthProvider OAuthProvider { get; } = new OAuthProviderWrapper();

        public IUser CurrentUser => _instance.CurrentUser != null ? new UserWrapper(_instance.CurrentUser) : null;

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

        public async Task<string[]> FetchProvidersForEmailAsync(string email)
        {
            try
            {
                return await _instance.FetchProvidersAsync(email).ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task SendPasswordResetEmailAsync(string email)
        {
            try
            {
                await _instance.SendPasswordResetAsync(email).ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task SendPasswordResetEmailAsync(string email, ActionCodeSettings actionCodeSettings)
        {
            try
            {
                await _instance.SendPasswordResetAsync(email, actionCodeSettings.ToNative()).ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task ApplyActionCodeAsync(string code)
        {
            try
            {
                await _instance.ApplyActionCodeAsync(code).ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task CheckActionCodeAsync(string code)
        {
            try
            {
                await _instance.CheckActionCodeAsync(code).ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task ConfirmPasswordResetAsync(string code, string newPassword)
        {
            try
            {
                await _instance.ConfirmPasswordResetAsync(code, newPassword).ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<string> VerifyPasswordResetCodeAsync(string code)
        {
            try
            {
                return await _instance.VerifyPasswordResetCodeAsync(code).ConfigureAwait(false);
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