using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase;
using Android.Gms.Extensions;
using Plugin.CurrentActivity;

namespace Plugin.FirebaseAuth
{
    public class UserWrapper : IUser, IEquatable<UserWrapper>
    {
        private readonly FirebaseUser _user;

        public UserWrapper(FirebaseUser user)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
        }

        public string? DisplayName => _user.DisplayName;

        public string? Email => _user.Email;

        public bool IsAnonymous => _user.IsAnonymous;

        public string? PhoneNumber => _user.PhoneNumber;

        public Uri? PhotoUrl => _user.PhotoUrl != null ? new Uri(_user.PhotoUrl.ToString()) : null;

        public IEnumerable<IUserInfo> ProviderData => _user.ProviderData.Select(userInfo => new UserInfoWrapper(userInfo));

        public string ProviderId => _user.ProviderId;

        public string Uid => _user.Uid;

        public bool IsEmailVerified => _user.IsEmailVerified;

        public IUserMetadata? Metadata => _user.Metadata != null ? new UserMetadataWrapper(_user.Metadata) : null;

        public IMultiFactor MultiFactor => new MultiFactorWrapper(_user.MultiFactor);

        public async Task DeleteAsync()
        {
            try
            {
                await _user.DeleteAsync().ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<string?> GetIdTokenAsync(bool forceRefresh)
        {
            try
            {
                var result = await _user.GetIdToken(forceRefresh).AsAsync<GetTokenResult>().ConfigureAwait(false);
                return result.Token;
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthTokenResult> GetIdTokenResultAsync(bool forceRefresh)
        {
            try
            {
                var result = await _user.GetIdToken(forceRefresh).AsAsync<GetTokenResult>().ConfigureAwait(false);
                return new AuthTokenResultWrapper(result);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthResult> LinkWithCredentialAsync(IAuthCredential credential)
        {
            try
            {
                var result = await _user.LinkWithCredentialAsync(credential.ToNative()).ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthResult> LinkWithProviderAsync(IFederatedAuthProvider federatedAuthProvider)
        {
            var activity = CrossCurrentActivity.Current.Activity ?? throw new NullReferenceException("current activity is null");

            try
            {
                Firebase.Auth.IAuthResult result;

                var auth = Firebase.Auth.FirebaseAuth.GetInstance(_user.Zza());
                var pendingResultTask = auth.GetPendingAuthResult();

                if (pendingResultTask != null)
                {
                    result = await pendingResultTask.AsAsync<Firebase.Auth.IAuthResult>().ConfigureAwait(false);
                }
                else
                {
                    result = await _user.StartActivityForLinkWithProvider(activity, federatedAuthProvider.ToNative()).AsAsync<Firebase.Auth.IAuthResult>()
                        .ConfigureAwait(false);
                }

                return new AuthResultWrapper(result);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthResult> ReauthenticateAndRetrieveDataAsync(IAuthCredential credential)
        {
            try
            {
                var result = await _user.ReauthenticateAndRetrieveDataAsync(credential.ToNative()).ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthResult> ReauthenticateWithProviderAsync(IFederatedAuthProvider federatedAuthProvider)
        {
            var activity = CrossCurrentActivity.Current.Activity ?? throw new NullReferenceException("current activity is null");

            try
            {
                Firebase.Auth.IAuthResult result;

                var auth = Firebase.Auth.FirebaseAuth.GetInstance(_user.Zza());
                var pendingResultTask = auth.GetPendingAuthResult();

                if (pendingResultTask != null)
                {
                    result = await pendingResultTask.AsAsync<Firebase.Auth.IAuthResult>().ConfigureAwait(false);
                }
                else
                {
                    result = await _user.StartActivityForReauthenticateWithProvider(activity, federatedAuthProvider.ToNative()).AsAsync<Firebase.Auth.IAuthResult>()
                        .ConfigureAwait(false);
                }

                return new AuthResultWrapper(result);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task ReauthenticateAsync(IAuthCredential credential)
        {
            try
            {
                await _user.ReauthenticateAsync(credential.ToNative()).ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task ReloadAsync()
        {
            try
            {
                await _user.ReloadAsync().ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task SendEmailVerificationAsync()
        {
            try
            {
                await _user.SendEmailVerificationAsync(null).ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task SendEmailVerificationAsync(ActionCodeSettings actionCodeSettings)
        {
            try
            {
                await _user.SendEmailVerificationAsync(actionCodeSettings.ToNative()).ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IUser> UnlinkAsync(string providerId)
        {
            try
            {
                var result = await _user.UnlinkAsync(providerId).ConfigureAwait(false);
                return new UserWrapper(result.User);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task UpdateEmailAsync(string email)
        {
            try
            {
                await _user.UpdateEmailAsync(email).ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task UpdatePasswordAsync(string password)
        {
            try
            {
                await _user.UpdatePasswordAsync(password).ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task UpdatePhoneNumberAsync(IPhoneAuthCredential credential)
        {
            try
            {
                await _user.UpdatePhoneNumberAsync(credential.ToNative()).ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task UpdateProfileAsync(UserProfileChangeRequest request)
        {
            try
            {
                var builder = new Firebase.Auth.UserProfileChangeRequest.Builder();

                if (request.IsDisplayNameChanged)
                {
                    builder.SetDisplayName(request.DisplayName);
                }
                if (request.IsPhotoUrlChanged)
                {
                    var uri = request.PhotoUrl != null ? Android.Net.Uri.Parse(request.PhotoUrl.ToString()) : null;
                    builder.SetPhotoUri(uri);
                }

                await _user.UpdateProfileAsync(builder.Build()).ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task VerifyBeforeUpdateEmail(string newEmail)
        {
            try
            {
                await _user.VerifyBeforeUpdateEmail(newEmail).AsAsync().ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task VerifyBeforeUpdateEmail(string newEmail, ActionCodeSettings actionCodeSettings)
        {
            try
            {
                await _user.VerifyBeforeUpdateEmail(newEmail, actionCodeSettings.ToNative()).AsAsync().ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as UserWrapper);
        }

        public bool Equals(UserWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_user, other._user)) return true;
            return _user.Equals(other._user);
        }

        public override int GetHashCode()
        {
            return _user.GetHashCode();
        }

        FirebaseUser IUser.ToNative()
        {
            return _user;
        }
    }
}
