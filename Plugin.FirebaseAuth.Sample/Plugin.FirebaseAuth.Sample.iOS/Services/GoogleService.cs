using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Foundation;
using Google.SignIn;
using Plugin.FirebaseAuth.Sample.Services;

namespace Plugin.FirebaseAuth.Sample.iOS.Services
{
    public class GoogleService : SignInDelegate, IGoogleService
    {
        private TaskCompletionSource<GoogleUser> _tcs;

        public GoogleService()
        {
            SignIn.SharedInstance.ClientId = Firebase.Core.App.DefaultInstance.Options.ClientId;
            SignIn.SharedInstance.Delegate = this;
        }

        public override void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
        {
            if (error != null)
            {
                _tcs.TrySetException(new NSErrorException(error));
            }
            else
            {
                _tcs.TrySetResult(user);
            }
        }

        public async Task<(string IdToken, string AccessToken)> GetCredentialAsync()
        {
            if (_tcs != null && !_tcs.Task.IsCompleted)
            {
                _tcs.TrySetCanceled();
            }

            if (SignIn.SharedInstance.HasPreviousSignIn)
            {
                try
                {
                    _tcs = new TaskCompletionSource<GoogleUser>();

                    SignIn.SharedInstance.RestorePreviousSignIn();

                    var restoredUser = await _tcs.Task.ConfigureAwait(false);

                    return (restoredUser.Authentication.IdToken, restoredUser.Authentication.AccessToken);
                }
                catch (NSErrorException)
                {
                }
            }

            _tcs = new TaskCompletionSource<GoogleUser>();

            SignIn.SharedInstance.PresentingViewController = Utils.GetTopViewController();
            SignIn.SharedInstance.SignInUser();

            var user = await _tcs.Task.ConfigureAwait(false);

            return (user.Authentication.IdToken, user.Authentication.AccessToken);
        }
    }
}
