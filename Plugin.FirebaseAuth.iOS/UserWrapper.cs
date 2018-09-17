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
        internal User User { get; }

        public UserWrapper(User user)
        {
            User = user;
        }

        public string DisplayName => User.DisplayName;

        public string Email => User.Email;

        public bool IsAnonymous => User.IsAnonymous;

        public string PhoneNumber => User.PhoneNumber;

        public Uri PhotoUrl => User.PhotoUrl != null ? new Uri(User.PhotoUrl.AbsoluteString) : null;

        public IEnumerable<IUserInfo> ProviderData => User.ProviderData.Select(userInfo => new UserInfoWrapper(userInfo));

        public string ProviderId => User.ProviderId;

        public string Uid => User.Uid;

        public bool IsEmailVerified => User.IsEmailVerified;

        public async Task DeleteAsync()
        {
            try
            {
                await User.DeleteAsync().ConfigureAwait(false);
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
                return await User.GetIdTokenAsync(forceRefresh).ConfigureAwait(false);
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
                var result = await User.LinkAndRetrieveDataAsync(wrapper.AuthCredential).ConfigureAwait(false);
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
                var result = await User.ReauthenticateAndRetrieveDataAsync(wrapper.AuthCredential).ConfigureAwait(false);
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
                await User.ReloadAsync().ConfigureAwait(false);
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
                await User.SendEmailVerificationAsync().ConfigureAwait(false);
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
                await User.SendEmailVerificationAsync(actionCodeSettings.ToNative()).ConfigureAwait(false);
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
                var result = await User.UnlinkAsync(providerId).ConfigureAwait(false);
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
                await User.UpdateEmailAsync(email).ConfigureAwait(false);
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
                await User.UpdatePasswordAsync(password).ConfigureAwait(false);
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
                await User.UpdatePhoneNumberCredentialAsync(wrapper.PhoneAuthCredential).ConfigureAwait(false);
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
                var userProfileChangeRequest = User.ProfileChangeRequest();
                if (request.IsDisplayNameChanged)
                {
                    userProfileChangeRequest.DisplayName = request.DisplayName;
                }
                if (request.IsPhotoUrlChanged)
                {
                    userProfileChangeRequest.PhotoUrl = request.PhotoUrl;
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
