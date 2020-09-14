using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Runtime;
using Plugin.FirebaseAuth.Sample.Services;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace Plugin.FirebaseAuth.Sample.Droid.Services
{
    public class FacebookService : IFacebookService
    {
        private readonly Activity _activity;
        private readonly ICallbackManager _callbackManager;

        private TaskCompletionSource<LoginResult> _tcs;

        public FacebookService(Activity activity)
        {
            _activity = activity;

            _callbackManager = CallbackManagerFactory.Create();

            LoginManager.Instance.RegisterCallback(_callbackManager, new FacebookCallback(this));
        }

        public async Task<string> GetCredentialAsync()
        {
            if (_tcs != null && !_tcs.Task.IsCompleted)
            {
                _tcs.TrySetCanceled();
            }
            _tcs = new TaskCompletionSource<LoginResult>();

            var accessToken = AccessToken.CurrentAccessToken;
            if (accessToken != null && !accessToken.IsExpired)
            {
                return accessToken.Token;
            }

            LoginManager.Instance.LogInWithReadPermissions(_activity, new[] { "email", "public_profile" });

            var result = await _tcs.Task.ConfigureAwait(false);

            return result.AccessToken.Token;
        }

        public void OnActivetyResult(int requestCode, Result resultCode, Intent data)
        {
            _callbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }

        private class FacebookCallback : Java.Lang.Object, IFacebookCallback
        {
            private readonly FacebookService _facebookService;

            public FacebookCallback(FacebookService facebookService)
            {
                _facebookService = facebookService;
            }

            public void OnSuccess(Java.Lang.Object result)
            {
                var loginResult = result.JavaCast<LoginResult>();
                _facebookService._tcs.TrySetResult(loginResult);
            }

            public void OnCancel()
            {
                _facebookService._tcs.TrySetException(new OperationCanceledException());
            }

            public void OnError(FacebookException error)
            {
                _facebookService._tcs.TrySetException(error);
            }
        }
    }
}
