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

        private Uri _photoUrl;
        public Uri PhotoUrl
        {
            get
            {
                if (_photoUrl == null && User.PhotoUrl != null)
                {
                    _photoUrl = new Uri(User.PhotoUrl.AbsoluteString);
                }
                return _photoUrl;
            }
        }

        private IEnumerable<IUserInfo> _providerData;
        public IEnumerable<IUserInfo> ProviderData
        {
            get
            {
                if (_providerData == null)
                {
                    _providerData = User.ProviderData.Select(userInfo => new UserInfoWrapper(userInfo));
                }
                return _providerData;
            }
        }

        public string ProviderId => User.ProviderId;

        public string Uid => User.Uid;

        public bool IsEmailVerified => User.IsEmailVerified;

        public Task DeleteAsync()
        {
            try
            {
                return User.DeleteAsync();
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task<string> GetIdTokenAsync(bool forceRefresh)
        {
            try
            {
                return User.GetIdTokenAsync(forceRefresh);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task LinkWithCredentialAsync(IAuthCredential credential)
        {
            try
            {
                var wrapper = (AuthCredentialWrapper)credential;
                return User.LinkAndRetrieveDataAsync(wrapper.AuthCredential);
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

        public Task ReloadAsync()
        {
            try
            {
                return User.ReloadAsync();
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

        public Task UpdateEmailAsync(string email)
        {
            try
            {
                return User.UpdateEmailAsync(email);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task UpdatePasswordAsync(string password)
        {
            try
            {
                return User.UpdatePasswordAsync(password);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task UpdatePhoneNumberAsync(IPhoneAuthCredential credential)
        {
            try
            {
                var wrapper = (PhoneAuthCredentialWrapper)credential;
                return User.UpdatePhoneNumberCredentialAsync(wrapper.PhoneAuthCredential);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task UpdateProfileAsync(UserProfileChangeRequest request)
        {
            try
            {
                var userProfileChangeRequest = User.ProfileChangeRequest();
                if (request.IsDisplayNameChanged)
                {
                    userProfileChangeRequest.DisplayName = request.DisplayName;
                }
                if (request.IsPhtoUrlChanged)
                {
                    userProfileChangeRequest.PhotoUrl = request.PhtoUrl;
                }

                return userProfileChangeRequest.CommitChangesAsync();
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }
    }
}
