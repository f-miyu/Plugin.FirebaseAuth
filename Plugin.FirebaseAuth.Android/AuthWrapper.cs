using System;
using System.Threading.Tasks;
using Firebase;
using System.Linq;
using Firebase.Auth;
using Android.Runtime;
using Android.Gms.Extensions;

namespace Plugin.FirebaseAuth
{
    public class AuthWrapper : IAuth
    {
        public IUser CurrentUser => _auth.CurrentUser != null ? new UserWrapper(_auth.CurrentUser) : null;

        public string LanguageCode
        {
            get => _auth.LanguageCode;
            set => _auth.LanguageCode = value;
        }

        private readonly Firebase.Auth.FirebaseAuth _auth;

        public AuthWrapper(Firebase.Auth.FirebaseAuth auth)
        {
            _auth = auth;
        }

        public static explicit operator Firebase.Auth.FirebaseAuth(AuthWrapper wrapper)
        {
            return wrapper._auth;
        }

        public async Task<IAuthResult> CreateUserWithEmailAndPasswordAsync(string email, string password)
        {
            try
            {
                var result = await _auth.CreateUserWithEmailAndPasswordAsync(email, password).ConfigureAwait(false);
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
                var result = await _auth.SignInAnonymouslyAsync().ConfigureAwait(false);
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
                var result = await _auth.SignInWithCredentialAsync((AuthCredential)wrapper).ConfigureAwait(false);
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
                var result = await _auth.SignInWithCustomTokenAsync(token).ConfigureAwait(false);
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
                var result = await _auth.SignInWithEmailAndPasswordAsync(email, password).ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthResult> SignInWithEmailLinkAsync(string email, string link)
        {
            try
            {
                var result = await TasksExtensions.AsAsync<Firebase.Auth.IAuthResult>(_auth.SignInWithEmailLink(email, link)).ConfigureAwait(false);
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
                var result = await _auth.FetchProvidersForEmailAsync(email).ConfigureAwait(false);
                return result.Providers.ToArray();
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<string[]> FetchSignInMethodsForEmailAsync(string email)
        {
            try
            {
                var result = await TasksExtensions.AsAsync<ISignInMethodQueryResult>(_auth.FetchSignInMethodsForEmail(email)).ConfigureAwait(false);
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
                await _auth.SendPasswordResetEmailAsync(email).ConfigureAwait(false);
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
                await _auth.SendPasswordResetEmailAsync(email, actionCodeSettings.ToNative()).ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task SendSignInLinkToEmailAsync(string email, ActionCodeSettings actionCodeSettings)
        {
            try
            {
                await TasksExtensions.AsAsync(_auth.SendSignInLinkToEmail(email, actionCodeSettings.ToNative())).ConfigureAwait(false);
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
                await _auth.ApplyActionCodeAsync(code).ConfigureAwait(false);
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
                await _auth.CheckActionCodeAsync(code).ConfigureAwait(false);
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
                await _auth.ConfirmPasswordResetAsync(code, newPassword).ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<string> VerifyPasswordResetCodeAsync(string code)
        {
            try
            {
                var result = await TasksExtensions.AsAsync<Java.Lang.String>(_auth.VerifyPasswordResetCode(code)).ConfigureAwait(false);
                return result.ToString();
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task UpdateCurrentUserAsync(IUser user)
        {
            try
            {
                var wrapper = (UserWrapper)user;
                await TasksExtensions.AsAsync(_auth.UpdateCurrentUser((FirebaseUser)wrapper)).ConfigureAwait(false);
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
                _auth.SignOut();
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public void UseAppLanguage()
        {
            _auth.UseAppLanguage();
        }

        public bool IsSignInWithEmailLink(string link)
        {
            return _auth.IsSignInWithEmailLink(link);
        }

        public IListenerRegistration AddAuthStateChangedListener(AuthStateChangedHandler listener)
        {
            return new AuthStateChangedListenerRegistration(_auth, listener);
        }

        public IListenerRegistration AddIdTokenChangedListener(IdTokenChangedHandler listener)
        {
            return new IdTokenChangedListenerRegistration(_auth, listener);
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
