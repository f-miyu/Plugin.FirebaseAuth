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

        public Uri PhotoUrl => User.PhotoUrl != null ? new Uri(User.PhotoUrl.ToString()) : null;

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

        public async Task<IAuthResult> LinkWithCredentialAsync(IAuthCredential credential)
        {
            try
            {
                var wrapper = (AuthCredentialWrapper)credential;
                var result = await User.LinkWithCredentialAsync(wrapper.AuthCredential).ConfigureAwait(false);
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
                var wrapper = (AuthCredentialWrapper)credential;
                var result = await User.ReauthenticateAndRetrieveDataAsync(wrapper.AuthCredential).ConfigureAwait(false);
                return new AuthResultWrapper(result);
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
                await User.ReloadAsync().ConfigureAwait(false);
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
                await User.SendEmailVerificationAsync(null).ConfigureAwait(false);
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
                await User.SendEmailVerificationAsync(actionCodeSettings.ToNative()).ConfigureAwait(false);
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

        public async Task UpdateEmailAsync(string email)
        {
            try
            {
                await User.UpdateEmailAsync(email).ConfigureAwait(false);
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
                await User.UpdatePasswordAsync(password).ConfigureAwait(false);
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
                var wrapper = (PhoneAuthCredentialWrapper)credential;
                await User.UpdatePhoneNumberAsync(wrapper.PhoneAuthCredential).ConfigureAwait(false);
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

                await User.UpdateProfileAsync(builder.Build()).ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }
    }
}
