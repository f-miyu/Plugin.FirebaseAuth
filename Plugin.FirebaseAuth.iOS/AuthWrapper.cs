using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Foundation;

namespace Plugin.FirebaseAuth
{
    public class AuthWrapper : IAuth
    {
        public event EventHandler<AuthStateEventArgs> AuthState;

        public event EventHandler<IdTokenEventArgs> IdToken;

        public IUser CurrentUser => _auth.CurrentUser != null ? new UserWrapper(_auth.CurrentUser) : null;

        public string LanguageCode
        {
            get => _auth.LanguageCode;
            set => _auth.LanguageCode = value;
        }

        private readonly Auth _auth;

        public static explicit operator Auth(AuthWrapper wrapper)
        {
            return wrapper._auth;
        }

        public AuthWrapper(Auth auth)
        {
            _auth = auth;

            _auth.AddAuthStateDidChangeListener(OnAuthStateChanged);
            _auth.AddIdTokenDidChangeListener(OnIdTokenChanged);
        }

        public async Task<IAuthResult> CreateUserWithEmailAndPasswordAsync(string email, string password)
        {
            try
            {
                var result = await _auth.CreateUserAsync(email, password).ConfigureAwait(false);
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
                var result = await _auth.SignInAnonymouslyAsync().ConfigureAwait(false);
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
                var result = await _auth.SignInWithCredentialAsync((AuthCredential)wrapper).ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthResult> SignInWithCustomTokenAsync(string token)
        {
            try
            {
                var result = await _auth.SignInWithCustomTokenAsync(token).ConfigureAwait(false);
                return new AuthResultWrapper(result);
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
                var result = await _auth.SignInWithPasswordAsync(email, password).ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthResult> SignInWithEmailLinkAsync(string email, string link)
        {
            try
            {
                var result = await _auth.SignInWithLinkAsync(email, link).ConfigureAwait(false);
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
                return await _auth.FetchSignInMethodsAsync(email).ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<string[]> FetchSignInMethodsForEmailAsync(string email)
        {
            try
            {
                return await _auth.FetchSignInMethodsAsync(email).ConfigureAwait(false);
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
                await _auth.SendPasswordResetAsync(email).ConfigureAwait(false);
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
                await _auth.SendPasswordResetAsync(email, actionCodeSettings.ToNative()).ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task SendSignInLinkToEmailAsync(string email, ActionCodeSettings actionCodeSettings)
        {
            try
            {
                await _auth.SendSignInLinkAsync(email, actionCodeSettings.ToNative()).ConfigureAwait(false);
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
                await _auth.ApplyActionCodeAsync(code).ConfigureAwait(false);
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
                await _auth.CheckActionCodeAsync(code).ConfigureAwait(false);
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
                await _auth.ConfirmPasswordResetAsync(code, newPassword).ConfigureAwait(false);
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
                return await _auth.VerifyPasswordResetCodeAsync(code).ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task UpdateCurrentUserAsync(IUser user)
        {
            try
            {
                var wrapper = (UserWrapper)user;
                await _auth.UpdateCurrentUserAsync((User)user).ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public void SignOut()
        {
            _auth.SignOut(out var error);

            if (error != null)
            {
                throw ExceptionMapper.Map(new NSErrorException(error));
            }
        }

        public void UseAppLanguage()
        {
            _auth.UseAppLanguage();
        }

        public bool IsSignInWithEmailLink(string link)
        {
            return _auth.IsSignIn(link);
        }

        public IListenerRegistration AddAuthStateChangedListener(AuthStateChangedHandler listener)
        {
            return new AuthStateChangedListenerRegistration(_auth, listener);
        }

        public IListenerRegistration AddIdTokenChangedListener(IdTokenChangedHandler listener)
        {
            return new IdTokenChangedListenerRegistration(_auth, listener);
        }

        private void OnAuthStateChanged(Auth auth, User user)
        {
            AuthState?.Invoke(this, new AuthStateEventArgs(auth == null ? null : AuthProvider.GetAuth(auth)));
        }

        private void OnIdTokenChanged(Auth auth, User user)
        {
            IdToken?.Invoke(this, new IdTokenEventArgs(auth == null ? null : AuthProvider.GetAuth(auth)));
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
                    handler?.Invoke(auth == null ? null : AuthProvider.GetAuth(auth));
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
                    handler?.Invoke(auth == null ? null : AuthProvider.GetAuth(auth));
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
