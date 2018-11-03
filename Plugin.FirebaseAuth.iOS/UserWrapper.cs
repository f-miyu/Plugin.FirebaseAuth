using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using System.Linq;
using Foundation;

namespace Plugin.FirebaseAuth
{
    public class UserWrapper : IUser
    {
        private readonly User _user;

        public UserWrapper(User user)
        {
            _user = user;
        }

        public string DisplayName => _user.DisplayName;

        public string Email => _user.Email;

        public bool IsAnonymous => _user.IsAnonymous;

        public string PhoneNumber => _user.PhoneNumber;

        public Uri PhotoUrl => _user.PhotoUrl != null ? new Uri(_user.PhotoUrl.AbsoluteString) : null;

        public IEnumerable<IUserInfo> ProviderData => _user.ProviderData.Select(userInfo => new UserInfoWrapper(userInfo));

        public string ProviderId => _user.ProviderId;

        public string Uid => _user.Uid;

        public bool IsEmailVerified => _user.IsEmailVerified;

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

        public async Task<string> GetIdTokenAsync(bool forceRefresh)
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

        public async Task<IAuthResult> LinkWithCredentialAsync(IAuthCredential credential)
        {
            try
            {
                var wrapper = (AuthCredentialWrapper)credential;
                var result = await _user.LinkAndRetrieveDataAsync((AuthCredential)wrapper).ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IAuthResult> ReauthenticateAndRetrieveDataAsync(IAuthCredential credential)
        {
            try
            {
                var wrapper = (AuthCredentialWrapper)credential;
                var result = await _user.ReauthenticateAndRetrieveDataAsync((AuthCredential)wrapper).ConfigureAwait(false);
                return new AuthResultWrapper(result);
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
                await _user.SendEmailVerificationAsync(actionCodeSettings.ToNative()).ConfigureAwait(false);
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
                var wrapper = (PhoneAuthCredentialWrapper)credential;
                await _user.UpdatePhoneNumberCredentialAsync((PhoneAuthCredential)wrapper).ConfigureAwait(false);
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
    }
}
