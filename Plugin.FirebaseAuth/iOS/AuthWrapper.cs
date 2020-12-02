using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Foundation;

namespace Plugin.FirebaseAuth
{
    public class AuthWrapper : IAuth, IEquatable<AuthWrapper>
    {
        private readonly Auth _auth;

        public AuthWrapper(Auth auth)
        {
            _auth = auth ?? throw new ArgumentNullException(nameof(auth));

            _auth.AddAuthStateDidChangeListener(OnAuthStateChanged);
            _auth.AddIdTokenDidChangeListener(OnIdTokenChanged);
        }

        public event EventHandler<AuthStateEventArgs>? AuthState;

        public event EventHandler<IdTokenEventArgs>? IdToken;

        public IUser? CurrentUser => _auth.CurrentUser != null ? new UserWrapper(_auth.CurrentUser) : null;

        public string? LanguageCode
        {
            get => _auth.LanguageCode;
            set => _auth.LanguageCode = value;
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
                var result = await _auth.SignInWithCredentialAsync(credential.ToNative()).ConfigureAwait(false);
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
                await _auth.SendPasswordResetAsync(email, actionCodeSettings.ToNative()!).ConfigureAwait(false);
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
                await _auth.SendSignInLinkAsync(email, actionCodeSettings.ToNative()!).ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task<IAuthResult> SignInWithProviderAsync(IFederatedAuthProvider federatedAuthProvider)
        {
            var tcs = new TaskCompletionSource<IAuthResult>();

            federatedAuthProvider.ToNative().Completion(FirebaseAuth.SignInWithProviderAuthUIDelegate, (credential, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(error));
                }
                else
                {
                    _auth.SignInWithCredential(credential!, (result, error) =>
                    {
                        if (error != null)
                        {
                            tcs.SetException(ExceptionMapper.Map(error));
                        }
                        else
                        {
                            tcs.SetResult(new AuthResultWrapper(result!));
                        }
                    });
                }
            });

            return tcs.Task;
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

        public async Task<IActionCodeInfo> CheckActionCodeAsync(string code)
        {
            try
            {
                var info = await _auth.CheckActionCodeAsync(code).ConfigureAwait(false);
                return new ActionCodeInfoWrapper(info);
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
                await _auth.UpdateCurrentUserAsync(user.ToNative()).ConfigureAwait(false);
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

        public void UseUserAccessGroup(string? accessGroup)
        {
            _auth.UseUserAccessGroup(accessGroup, out var error);

            if (error != null)
            {
                throw ExceptionMapper.Map(error);
            }
        }

        public IUser? GetStoredUser(string? accessGroup)
        {
            var user = _auth.GetStoredUser(accessGroup, out var error);

            if (error != null)
            {
                throw ExceptionMapper.Map(error);
            }

            if (user == null)
            {
                return null;
            }

            return new UserWrapper(user);
        }

        private void OnAuthStateChanged(Auth auth, User? user)
        {
            AuthState?.Invoke(this, new AuthStateEventArgs(AuthProvider.GetAuth(auth)));
        }

        private void OnIdTokenChanged(Auth auth, User? user)
        {
            IdToken?.Invoke(this, new IdTokenEventArgs(AuthProvider.GetAuth(auth)));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AuthWrapper);
        }

        public bool Equals(AuthWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_auth, other._auth)) return true;
            return _auth.Equals(other._auth);
        }

        public override int GetHashCode()
        {
            return _auth.GetHashCode();
        }

        Auth IAuth.ToNative()
        {
            return _auth;
        }

        private class AuthStateChangedListenerRegistration : IListenerRegistration
        {
            private readonly Auth _instance;
            private NSObject? _listner;

            public AuthStateChangedListenerRegistration(Auth instance, AuthStateChangedHandler handler)
            {
                _instance = instance;
                _listner = _instance.AddAuthStateDidChangeListener((Auth auth, User? user) =>
                {
                    handler?.Invoke(AuthProvider.GetAuth(auth));
                });
            }

            public void Dispose()
            {
                Remove();
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
            private NSObject? _listner;

            public IdTokenChangedListenerRegistration(Auth instance, IdTokenChangedHandler handler)
            {
                _instance = instance;
                _listner = _instance.AddIdTokenDidChangeListener((Auth auth, User? user) =>
                {
                    handler?.Invoke(AuthProvider.GetAuth(auth));
                });
            }

            public void Dispose()
            {
                Remove();
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
