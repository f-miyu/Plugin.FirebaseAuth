using System;
using System.Threading.Tasks;
using Firebase;
using System.Linq;
using Android.Runtime;

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

        private readonly Firebase.Auth.FirebaseAuth _instance = Firebase.Auth.FirebaseAuth.Instance;

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

        public async Task<string[]> FetchProvidersForEmailAsync(string email)
        {
            try
            {
                var result = await _instance.FetchProvidersForEmailAsync(email).ConfigureAwait(false);
                return result.Providers.ToArray();
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task SendPasswordResetEmailAsync(string email)
        {
            try
            {
                await _instance.SendPasswordResetEmailAsync(email).ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task SendPasswordResetEmailAsync(string email, ActionCodeSettings actionCodeSettings)
        {
            try
            {
                await _instance.SendPasswordResetEmailAsync(email, actionCodeSettings.ToNative()).ConfigureAwait(false);
            }
            catch (FirebaseException e)
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
            catch (FirebaseException e)
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
            catch (FirebaseException e)
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
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task<string> VerifyPasswordResetCodeAsync(string code)
        {
            var tcs = new TaskCompletionSource<string>();

            _instance.VerifyPasswordResetCode(code).AddOnCompleteListener(new OnCompleteHandlerListener(task =>
            {
                if (task.IsSuccessful)
                {
                    var result = task.Result.ToString();
                    tcs.SetResult(result);
                }
                else
                {
                    Exception exception = task.Exception;
                    if (exception is FirebaseException firebaseException)
                    {
                        exception = ExceptionMapper.Map(firebaseException);
                    }
                    tcs.SetException(exception);
                }
            }));

            return tcs.Task;
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
            private readonly Firebase.Auth.FirebaseAuth _instance;
            private Firebase.Auth.FirebaseAuth.IAuthStateListener _listener;

            public AuthStateChangedListenerRegistration(Firebase.Auth.FirebaseAuth instance, AuthStateChangedHandler handler)
            {
                _instance = instance;
                _listener = new AuthStateListener(handler);
                _instance.AddAuthStateListener(_listener);
            }

            public void Remove()
            {
                if (_listener != null)
                {
                    _instance.RemoveAuthStateListener(_listener);
                    _listener = null;
                }
            }

            private class AuthStateListener : Java.Lang.Object, Firebase.Auth.FirebaseAuth.IAuthStateListener
            {
                private readonly AuthStateChangedHandler _handler;

                public AuthStateListener(AuthStateChangedHandler handler)
                {
                    _handler = handler;
                }

                public void OnAuthStateChanged(Firebase.Auth.FirebaseAuth auth)
                {
                    IUser user = null;
                    if (auth.CurrentUser != null)
                    {
                        user = new UserWrapper(auth.CurrentUser);
                    }
                    _handler?.Invoke(user);
                }
            }
        }

        private class IdTokenChangedListenerRegistration : IListenerRegistration
        {
            private readonly Firebase.Auth.FirebaseAuth _instance;
            private Firebase.Auth.FirebaseAuth.IIdTokenListener _listener;

            public IdTokenChangedListenerRegistration(Firebase.Auth.FirebaseAuth instance, IdTokenChangedHandler handler)
            {
                _instance = instance;
                _listener = new IdTokenListener(handler);
                _instance.AddIdTokenListener(_listener);
            }

            public void Remove()
            {
                if (_listener != null)
                {
                    _instance.RemoveIdTokenListener(_listener);
                    _listener = null;
                }
            }

            private class IdTokenListener : Java.Lang.Object, Firebase.Auth.FirebaseAuth.IIdTokenListener
            {
                private readonly IdTokenChangedHandler _handler;

                public IdTokenListener(IdTokenChangedHandler handler)
                {
                    _handler = handler;
                }

                public void OnIdTokenChanged(Firebase.Auth.FirebaseAuth auth)
                {
                    IUser user = null;
                    if (auth.CurrentUser != null)
                    {
                        user = new UserWrapper(auth.CurrentUser);
                    }
                    _handler?.Invoke(user);
                }
            }
        }
    }
}
