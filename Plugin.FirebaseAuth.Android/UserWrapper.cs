using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase;

namespace Plugin.FirebaseAuth
{
    public class UserWrapper : IUser
    {
        internal FirebaseUser User { get; }

        public UserWrapper(FirebaseUser user)
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
                    _photoUrl = new Uri(User.PhotoUrl.ToString());
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
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<string> GetIdTokenAsync(bool forceRefresh)
        {
            try
            {
                var result = await User.GetIdTokenAsync(forceRefresh).ConfigureAwait(false);
                return result.Token;
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task LinkWithCredentialAsync(IAuthCredential credential)
        {
            try
            {
                var wrapper = (AuthCredentialWrapper)credential;
                return User.LinkWithCredentialAsync(wrapper.AuthCredential);
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
                var wrapper = (AuthCredentialWrapper)credential;
                var result = await User.ReauthenticateAndRetrieveDataAsync(wrapper.AuthCredential).ConfigureAwait(false);
                return new AuthResultWrapper(result);
            }
            catch (FirebaseException e)
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
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IUser> UnlinkAsync(string providerId)
        {
            try
            {
                var result = await User.UnlinkAsync(providerId).ConfigureAwait(false);
                return new UserWrapper(result.User);
            }
            catch (FirebaseException e)
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
            catch (FirebaseException e)
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
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task UpdatePhoneNumberAsync(IPhoneAuthCredential credential)
        {
            try
            {
                var wrapper = (PhoneAuthCredentialWrapper)credential;
                return User.UpdatePhoneNumberAsync(wrapper.PhoneAuthCredential);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task UpdateProfileAsync(UserProfileChangeRequest request)
        {
            try
            {
                var builder = new Firebase.Auth.UserProfileChangeRequest.Builder();

                if (request.IsDisplayNameChanged)
                {
                    builder.SetDisplayName(request.DisplayName);
                }
                if (request.IsPhtoUrlChanged)
                {
                    builder.SetPhotoUri(Android.Net.Uri.Parse(request.PhtoUrl.ToString()));
                }

                return User.UpdateProfileAsync(builder.Build());
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }
    }
}
