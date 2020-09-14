using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Runtime;
using Plugin.FirebaseAuth.Sample.Services;

namespace Plugin.FirebaseAuth.Sample.Droid.Services
{
    public class GoogleService : IGoogleService
    {
        private const int SignInRequestCode = 9001;

        private TaskCompletionSource<string> _tcs;

        private readonly Activity _activity;
        private readonly GoogleSignInClient _client;

        public GoogleService(Activity activity)
        {
            _activity = activity;

            var gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken(activity.GetString(Resource.String.default_web_client_id))
                .RequestEmail()
                .Build();

            _client = GoogleSignIn.GetClient(activity, gso);
        }

        public async Task<(string IdToken, string AccessToken)> GetCredentialAsync()
        {
            if (_tcs != null && !_tcs.Task.IsCompleted)
            {
                _tcs.TrySetCanceled();
            }
            _tcs = new TaskCompletionSource<string>();

            var account = GoogleSignIn.GetLastSignedInAccount(_activity);
            if (account != null && !account.IsExpired)
            {
                return (account.IdToken, null);
            }

            try
            {
                account = await _client.SilentSignInAsync().ConfigureAwait(false);
                return (account.IdToken, null);
            }
            catch
            {
                _activity.StartActivityForResult(_client.SignInIntent, SignInRequestCode);

                var idToken = await _tcs.Task.ConfigureAwait(false);

                return (idToken, null);
            }
        }

        public void OnActivetyResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == SignInRequestCode)
            {
                var task = GoogleSignIn.GetSignedInAccountFromIntent(data);

                if (task.IsSuccessful)
                {
                    var account = task.Result.JavaCast<GoogleSignInAccount>();
                    _tcs.TrySetResult(account.IdToken);
                }
                else
                {
                    _tcs.TrySetException(task.Exception);
                }
            }
        }
    }
}
