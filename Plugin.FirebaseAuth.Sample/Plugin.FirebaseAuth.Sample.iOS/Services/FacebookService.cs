using System;
using System.Threading.Tasks;
using Facebook.CoreKit;
using Facebook.LoginKit;
using Foundation;
using Plugin.FirebaseAuth.Sample.Services;
using UIKit;

namespace Plugin.FirebaseAuth.Sample.iOS.Services
{
    public class FacebookService : IFacebookService
    {
        public Task<string> GetCredentialAsync()
        {
            var accessToken = AccessToken.CurrentAccessToken;
            if (accessToken != null && !accessToken.IsExpired)
            {
                return Task.FromResult(accessToken.TokenString);
            }

            var tcs = new TaskCompletionSource<string>();

            var login = new LoginManager();

            login.LogIn(new[] { "email", "public_profile" }, Utils.GetTopViewController(),
                (result, error) =>
                {
                    if (error != null)
                    {
                        tcs.SetException(new NSErrorException(error));
                    }
                    else if (result.IsCancelled)
                    {
                        tcs.SetException(new OperationCanceledException());
                    }
                    else
                    {
                        tcs.SetResult(result.Token.TokenString);
                    }
                });

            return tcs.Task;
        }
    }
}
