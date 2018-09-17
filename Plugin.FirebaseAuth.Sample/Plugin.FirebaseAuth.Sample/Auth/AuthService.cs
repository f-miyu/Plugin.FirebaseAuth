using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Plugin.FirebaseAuth.Sample.Auth
{
    public class AuthService : IAuthService
    {
        public Task<(string IdToken, string AccessToken)> LoginWithGoogle()
        {
            var tcs = new TaskCompletionSource<(string idToken, string accessToken)>();

            string clientId = null;
            string redirectUri = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = Constants.GoogleIosClientId;
                    redirectUri = Constants.GoogleIosRedirectUrl;
                    break;
                case Device.Android:
                    clientId = Constants.GoogleAndroidClientId;
                    redirectUri = Constants.GoogleAndroidRedirectUrl;
                    break;
            }

            var authenticator = new CustomOAuth2Authenticator(clientId,
                                                              null,
                                                              "https://www.googleapis.com/auth/userinfo.email",
                                                              new Uri("https://accounts.google.com/o/oauth2/auth"),
                                                              new Uri(redirectUri),
                                                              new Uri("https://www.googleapis.com/oauth2/v4/token"),
                                                              null,
                                                              true);

            authenticator.Completed += (sender, e) =>
            {
                if (e.IsAuthenticated && e.Account != null && e.Account.Properties != null)
                {
                    var properties = e.Account.Properties;

                    tcs.TrySetResult((properties["id_token"], properties["access_token"]));
                }
                else
                {
                    tcs.TrySetResult((null, null));
                }
            };

            authenticator.Error += (sender, e) =>
            {
                tcs.TrySetException(e.Exception ?? new Exception(e.Message));
            };

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);

            return tcs.Task;
        }

        public Task<(string OauthToken, string OatuhTokenSecret)> LoginWithTwitter()
        {
            var tcs = new TaskCompletionSource<(string Token, string Secret)>();

            var authenticator = new CustomOAuth1Authenticator(Constants.TwitterConsumerKey,
                                                              Constants.TwitterConsumerSecret,
                                                              new Uri("https://api.twitter.com/oauth/request_token"),
                                                              new Uri("https://api.twitter.com/oauth/authorize"),
                                                              new Uri("https://api.twitter.com/oauth/access_token"),
                                                              new Uri(Constants.TwitterRedirectUrl),
                                                              null,
                                                              true);

            authenticator.Completed += (sender, e) =>
            {
                if (e.IsAuthenticated && e.Account != null && e.Account.Properties != null)
                {
                    var properties = e.Account.Properties;

                    tcs.TrySetResult((properties["oauth_token"], properties["oauth_token_secret"]));
                }
                else
                {
                    tcs.TrySetResult((null, null));
                }
            };

            authenticator.Error += (sender, e) =>
            {
                tcs.TrySetException(e.Exception ?? new Exception(e.Message));
            };

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);

            return tcs.Task;
        }

        public Task<string> LoginWithFacebook()
        {
            var tcs = new TaskCompletionSource<string>();

            var authenticator = new CustomOAuth2Authenticator(Constants.FacebookClientId,
                                                              null,
                                                              new Uri("https://m.facebook.com/dialog/oauth"),
                                                              new Uri(Constants.FacebookRedirectUrl),
                                                              null,
                                                              true);

            authenticator.Completed += (sender, e) =>
            {
                if (e.IsAuthenticated && e.Account != null && e.Account.Properties != null)
                {
                    var properties = e.Account.Properties;

                    tcs.TrySetResult((properties["access_token"]));
                }
                else
                {
                    tcs.TrySetResult(null);
                }
            };

            authenticator.Error += (sender, e) =>
            {
                tcs.TrySetException(e.Exception ?? new Exception(e.Message));
            };

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);

            return tcs.Task;
        }

        public Task<string> LoginWithGitHub()
        {
            var tcs = new TaskCompletionSource<string>();

            var authenticator = new CustomOAuth2Authenticator(Constants.GitHubClientId,
                                                              Constants.GitHubClientSecret,
                                                              "user",
                                                              new Uri("https://github.com/login/oauth/authorize"),
                                                              new Uri(Constants.GitHubRedirectUrl),
                                                              new Uri("https://github.com/login/oauth/access_token"),
                                                              null,
                                                              true);

            authenticator.Completed += (sender, e) =>
            {
                if (e.IsAuthenticated && e.Account != null && e.Account.Properties != null)
                {
                    var properties = e.Account.Properties;

                    tcs.TrySetResult((properties["access_token"]));
                }
                else
                {
                    tcs.TrySetResult(null);
                }
            };

            authenticator.Error += (sender, e) =>
            {
                tcs.TrySetException(e.Exception ?? new Exception(e.Message));
            };

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);

            return tcs.Task;
        }
    }
}
