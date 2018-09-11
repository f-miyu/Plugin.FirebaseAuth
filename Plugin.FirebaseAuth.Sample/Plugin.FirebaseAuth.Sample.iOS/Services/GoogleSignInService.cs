using System;
using Plugin.FirebaseAuth.Sample.Services;
using Google.SignIn;
using Foundation;
using System.Threading.Tasks;
using UIKit;

namespace Plugin.FirebaseAuth.Sample.iOS.Services
{
    public class GoogleSignInService : IGoogleSignInService
    {
        private static TaskCompletionSource<(string IdToken, string AccessToken)> _tcs;

        public static GoogleSignInService Instance { get; } = new GoogleSignInService();

        protected GoogleSignInService()
        {
            Google.SignIn.SignIn.SharedInstance.SignedIn += (sender, e) =>
            {
                if (_tcs == null) return;

                string idToken = null;
                string accessToken = null;

                if (e.Error == null)
                {
                    idToken = e.User.Authentication.IdToken;
                    accessToken = e.User.Authentication.AccessToken;
                }

                _tcs.TrySetResult((idToken, accessToken));
            };

            Google.SignIn.SignIn.SharedInstance.UIDelegate = new MySignInUIDelegate();
        }

        public Task<(string IdToken, string AccessToken)> SignIn()
        {
            if (_tcs != null && !_tcs.Task.IsCompleted)
                throw new InvalidOperationException();

            _tcs = new TaskCompletionSource<(string IdToken, string AccessToken)>();

            Google.SignIn.SignIn.SharedInstance.SignInUser();

            return _tcs.Task;
        }

        private class MySignInUIDelegate : SignInUIDelegate
        {
            public override void PresentViewController(SignIn signIn, UIViewController viewController)
            {
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(viewController, true, null);
            }

            public override void DismissViewController(SignIn signIn, UIViewController viewController)
            {
                UIApplication.SharedApplication.KeyWindow.RootViewController.DismissViewController(true, null);

                _tcs?.TrySetCanceled();
            }
        }
    }
}
