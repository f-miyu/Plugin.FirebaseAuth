using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using System.Linq;
using Foundation;

namespace Plugin.FirebaseAuth
{
    public class UserWrapper : IUser, IEquatable<UserWrapper>
    {
        private readonly User _user;

        public UserWrapper(User user)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
        }

        public string? DisplayName => _user.DisplayName;

        public string? Email => _user.Email;

        public bool IsAnonymous => _user.IsAnonymous;

        public string? PhoneNumber => _user.PhoneNumber;

        public Uri? PhotoUrl => _user.PhotoUrl != null ? new Uri(_user.PhotoUrl.AbsoluteString) : null;

        public IEnumerable<IUserInfo> ProviderData => _user.ProviderData.Select(userInfo => new UserInfoWrapper(userInfo));

        public string ProviderId => _user.ProviderId;

        public string Uid => _user.Uid;

        public bool IsEmailVerified => _user.IsEmailVerified;

        public IUserMetadata? Metadata =>
            _user.Metadata != null && _user.Metadata.CreationDate != null && _user.Metadata.LastSignInDate != null
            ? new UserMetadataWrapper(_user.Metadata) : null;

        public IMultiFactor MultiFactor => new MultiFactorWrapper(_user.MultiFactor);

        public async Task DeleteAsync()
        {
            try
            {
                await _user.DeleteAsync().ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<string?> GetIdTokenAsync(bool forceRefresh)
        {
            try
            {
                return await _user.GetIdTokenAsync(forceRefresh).ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthTokenResult> GetIdTokenResultAsync(bool forceRefresh)
        {
            try
            {
                var result = await _user.GetIdTokenResultAsync(forceRefresh).ConfigureAwait(false);
                return new AuthTokenResultWrapper(result);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthResult> LinkWithCredentialAsync(IAuthCredential credential)
        {
            try
            {
                var result = await _user.LinkAsync(credential.ToNative()).ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task<IAuthResult> LinkWithProviderAsync(IFederatedAuthProvider federatedAuthProvider)
        {
            var tcs = new TaskCompletionSource<IAuthResult>();

            federatedAuthProvider.ToNative().Completion(FirebaseAuth.LinkWithProviderAuthUIDelegate, (credential, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(error));
                }
                else
                {
                    _user.Link(credential!, (result, error) =>
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

        public async Task<IAuthResult> ReauthenticateAndRetrieveDataAsync(IAuthCredential credential)
        {
            try
            {
                var result = await _user.ReauthenticateAndRetrieveDataAsync(credential.ToNative()).ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task<IAuthResult> ReauthenticateWithProviderAsync(IFederatedAuthProvider federatedAuthProvider)
        {
            var tcs = new TaskCompletionSource<IAuthResult>();

            federatedAuthProvider.ToNative().Completion(FirebaseAuth.ReauthenticateWithProviderAuthUIDelegate, (credential, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(error));
                }
                else
                {
                    _user.ReauthenticateAndRetrieveData(credential!, (result, error) =>
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

        public async Task ReauthenticateAsync(IAuthCredential credential)
        {
            try
            {
                await _user.ReauthenticateAsync(credential.ToNative()).ConfigureAwait(false);
            }
            catch (NSErrorException e)
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
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task SendEmailVerificationAsync()
        {
            try
            {
                await _user.SendEmailVerificationAsync().ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task SendEmailVerificationAsync(ActionCodeSettings actionCodeSettings)
        {
            try
            {
                await _user.SendEmailVerificationAsync(actionCodeSettings.ToNative()!).ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IUser> UnlinkAsync(string providerId)
        {
            try
            {
                var result = await _user.UnlinkAsync(providerId).ConfigureAwait(false);
                return new UserWrapper(result);
            }
            catch (NSErrorException e)
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
            catch (NSErrorException e)
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
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task UpdatePhoneNumberAsync(IPhoneAuthCredential credential)
        {
            try
            {
                await _user.UpdatePhoneNumberCredentialAsync(credential.ToNative()).ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task UpdateProfileAsync(UserProfileChangeRequest request)
        {
            try
            {
                var userProfileChangeRequest = _user.ProfileChangeRequest();
                if (request.IsDisplayNameChanged)
                {
                    userProfileChangeRequest.DisplayName = request.DisplayName;
                }
                if (request.IsPhotoUrlChanged)
                {
                    userProfileChangeRequest.PhotoUrl = request.PhotoUrl != null ? new NSUrl(request.PhotoUrl.ToString()) : null;
                }

                await userProfileChangeRequest.CommitChangesAsync().ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task VerifyBeforeUpdateEmail(string newEmail)
        {
            try
            {
                await _user.SendEmailVerificationBeforeUpdatingEmailAsync(newEmail).ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task VerifyBeforeUpdateEmail(string newEmail, ActionCodeSettings actionCodeSettings)
        {
            try
            {
                await _user.SendEmailVerificationBeforeUpdatingEmailAsync(newEmail, actionCodeSettings.ToNative()!).ConfigureAwait(false);
            }
            catch (NSErrorException e)
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

        User IUser.ToNative()
        {
            return _user;
        }
    }
}
