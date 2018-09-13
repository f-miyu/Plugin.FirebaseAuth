using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reactive.Bindings;
using Xamarin.Forms;
using Xamarin.Auth;
using System.Threading.Tasks;
using Plugin.FirebaseAuth.Sample.Auth;
using Prism.Services;
using DryIoc;

namespace Plugin.FirebaseAuth.Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public AsyncReactiveCommand GoogleLoginCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand TwitterLoginCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand FacebookLoginCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand GitHubLoginCommand { get; } = new AsyncReactiveCommand();

        private readonly IPageDialogService _pageDialogService;

        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService)
        {
            _pageDialogService = pageDialogService;

            Title = "Main Page";

            GoogleLoginCommand.Subscribe(LoginWithGoogle);
            TwitterLoginCommand.Subscribe(LoginWithTwitter);
            FacebookLoginCommand.Subscribe(LoginWithFacebook);
            GitHubLoginCommand.Subscribe(LoginWithGitHub);
        }

        private async Task LoginWithGoogle()
        {
            try
            {
                var tcs = new TaskCompletionSource<(string idToken, string accessToken)>();

                string clientId = null;
                string redirectUri = null;

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        clientId = Constants.GoogleIosClientId;
                        redirectUri = Constants.GoogleIosUrlScheme + ":/oauth2redirect";
                        break;
                    case Device.Android:
                        clientId = Constants.GoogleAndroidClientId;
                        redirectUri = Constants.GoogleAndroidUrlScheme + ":/oauth2redirect";
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
                        tcs.TrySetCanceled();
                    }
                };

                authenticator.Error += (sender, e) =>
                {
                    tcs.TrySetException(e.Exception ?? new Exception(e.Message));
                };

                AuthenticationState.Authenticator = authenticator;

                var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                presenter.Login(authenticator);

                var (idToken, accessToken) = await tcs.Task.ConfigureAwait(false);

                var credential = CrossFirebaseAuth.Current
                                                  .GoogleAuthProvider
                                                  .GetCredential(idToken, accessToken);

                var result = await CrossFirebaseAuth.Current.SignInWithCredentialAsync(credential).ConfigureAwait(false);

                Device.BeginInvokeOnMainThread(() =>
                {
                    _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
                });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                Device.BeginInvokeOnMainThread(() =>
                {
                    _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
                });
            }
        }

        private async Task LoginWithTwitter()
        {
            try
            {
                var tcs = new TaskCompletionSource<(string Token, string Secret)>();

                var authenticator = new CustomOAuth1Authenticator(Constants.TwitterConsumerKey,
                                                                  Constants.TwitterConsumerSecret,
                                                                  new Uri("https://api.twitter.com/oauth/request_token"),
                                                                  new Uri("https://api.twitter.com/oauth/authorize"),
                                                                  new Uri("https://api.twitter.com/oauth/access_token"),
                                                                  new Uri(Constants.UrlScheme + "://"),
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
                        tcs.TrySetCanceled();
                    }
                };

                authenticator.Error += (sender, e) =>
                {
                    tcs.TrySetException(e.Exception ?? new Exception(e.Message));
                };

                AuthenticationState.Authenticator = authenticator;

                var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                presenter.Login(authenticator);

                var (token, secret) = await tcs.Task.ConfigureAwait(false);

                var credential = CrossFirebaseAuth.Current
                                                  .TwitterAuthProvider
                                                  .GetCredential(token, secret);

                var result = await CrossFirebaseAuth.Current.SignInWithCredentialAsync(credential).ConfigureAwait(false);

                Device.BeginInvokeOnMainThread(() =>
                {
                    _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
                });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                Device.BeginInvokeOnMainThread(() =>
                {
                    _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
                });
            }
        }

        private async Task LoginWithFacebook()
        {
            try
            {
                var tcs = new TaskCompletionSource<string>();

                var authenticator = new CustomOAuth2Authenticator(Constants.FacebookClientId,

                                                                  null,
                                                                  new Uri("https://m.facebook.com/dialog/oauth/"),
                                                                  new Uri(Constants.FacebookUrlScheme + "://authorize"),
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
                        tcs.TrySetCanceled();
                    }
                };

                authenticator.Error += (sender, e) =>
                {
                    tcs.TrySetException(e.Exception ?? new Exception(e.Message));
                };

                AuthenticationState.Authenticator = authenticator;

                var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();

                presenter.Login(authenticator);

                var accessToken = await tcs.Task.ConfigureAwait(false);

                var credential = CrossFirebaseAuth.Current
                                                  .FacebookAuthProvider
                                                  .GetCredential(accessToken);

                var result = await CrossFirebaseAuth.Current.SignInWithCredentialAsync(credential).ConfigureAwait(false);

                Device.BeginInvokeOnMainThread(() =>
                {
                    _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
                });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                Device.BeginInvokeOnMainThread(() =>
                {
                    _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
                });
            }
        }

        private async Task LoginWithGitHub()
        {
            try
            {
                var tcs = new TaskCompletionSource<string>();

                var authenticator = new CustomOAuth2Authenticator(Constants.GitHubClientId,
                                                                  Constants.GitHubClientSecret,
                                                                  null,
                                                                  new Uri("https://github.com/login/oauth/authorize"),
                                                                  new Uri(Constants.UrlScheme + "://authorize"),
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
                        tcs.TrySetCanceled();
                    }
                };

                authenticator.Error += (sender, e) =>
                {
                    tcs.TrySetException(e.Exception ?? new Exception(e.Message));
                };

                AuthenticationState.Authenticator = authenticator;

                var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();

                presenter.Login(authenticator);

                var token = await tcs.Task.ConfigureAwait(false);

                var credential = CrossFirebaseAuth.Current
                                                  .GitHubAuthProvider
                                                  .GetCredential(token);

                var result = await CrossFirebaseAuth.Current.SignInWithCredentialAsync(credential).ConfigureAwait(false);

                Device.BeginInvokeOnMainThread(() =>
                {
                    _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
                });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                Device.BeginInvokeOnMainThread(() =>
                {
                    _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
                });
            }
        }
    }
}
