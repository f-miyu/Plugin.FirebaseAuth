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
using Plugin.FirebaseAuth.Sample.Extensins;

namespace Plugin.FirebaseAuth.Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public AsyncReactiveCommand SignUpCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInWithEmailAndPasswordCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInWithGoogleCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInWithTwitterCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInWithFacebookCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInWithGitHubCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInWithPhoneNumberCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand ShowUserCommand { get; }

        private readonly IPageDialogService _pageDialogService;
        private ReactivePropertySlim<bool> _isSignedIn = new ReactivePropertySlim<bool>();
        private IListenerRegistration _registration;
        private IAuthService _authService;

        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAuthService authService)
            : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _authService = authService;

            Title = "Main Page";

            _registration = CrossFirebaseAuth.Current.AddAuthStateChangedListener((user) =>
            {
                _isSignedIn.Value = user != null;
            });

            ShowUserCommand = _isSignedIn.ToAsyncReactiveCommand();

            SignUpCommand.Subscribe(SignUp);
            SignInWithEmailAndPasswordCommand.Subscribe(SignInWithEmailAndPassword);
            SignInWithGoogleCommand.Subscribe(SignInWithGoogle);
            SignInWithTwitterCommand.Subscribe(SignInWithTwitter);
            SignInWithFacebookCommand.Subscribe(SignInWithFacebook);
            SignInWithGitHubCommand.Subscribe(SignInWithGitHub);
            SignInWithPhoneNumberCommand.Subscribe(SignInWithPhoneNumber);
            ShowUserCommand.Subscribe(async () => await NavigationService.NavigateAsync<UserPageViewModel>());
        }

        public override void Destroy()
        {
            base.Destroy();

            _registration.Remove();
        }

        private async Task SignUp()
        {
            var user = await NavigationService.NavigateAsync<SignUpPageViewModel, IUser>();

            if (user != null)
            {
                await _pageDialogService.DisplayAlertAsync("Success", user.DisplayName, "OK");
            }
        }

        private async Task SignInWithEmailAndPassword()
        {
            var user = await NavigationService.NavigateAsync<SignInWithEmailAndPasswordPageViewModel, IUser>();

            if (user != null)
            {
                await _pageDialogService.DisplayAlertAsync("Success", user.DisplayName, "OK");
            }
        }

        private async Task SignInWithGoogle()
        {
            try
            {
                var (idToken, accessToken) = await _authService.LoginWithGoogle();

                if (idToken != null)
                {
                    var credential = CrossFirebaseAuth.Current
                                                      .GoogleAuthProvider
                                                      .GetCredential(idToken, accessToken);

                    var result = await CrossFirebaseAuth.Current.SignInWithCredentialAsync(credential);

                    await _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task SignInWithTwitter()
        {
            try
            {
                var (token, secret) = await _authService.LoginWithTwitter();

                if (token != null && secret != null)
                {
                    var credential = CrossFirebaseAuth.Current
                                                      .TwitterAuthProvider
                                                      .GetCredential(token, secret);

                    var result = await CrossFirebaseAuth.Current.SignInWithCredentialAsync(credential);

                    await _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task SignInWithFacebook()
        {
            try
            {
                var accessToken = await _authService.LoginWithFacebook();

                if (accessToken != null)
                {
                    var credential = CrossFirebaseAuth.Current
                                                      .FacebookAuthProvider
                                                      .GetCredential(accessToken);

                    var result = await CrossFirebaseAuth.Current.SignInWithCredentialAsync(credential);

                    await _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task SignInWithGitHub()
        {
            try
            {
                var accessToken = await _authService.LoginWithGitHub();

                if (accessToken != null)
                {
                    var credential = CrossFirebaseAuth.Current
                                                      .GitHubAuthProvider
                                                      .GetCredential(accessToken);

                    var result = await CrossFirebaseAuth.Current.SignInWithCredentialAsync(credential);

                    await _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task SignInWithPhoneNumber()
        {
            var result = await NavigationService.NavigateAsync<SignInWithPhoneNumberPageViewModel, IAuthResult>();

            if (result != null)
            {
                await _pageDialogService.DisplayAlertAsync("Success", result.User.PhoneNumber, "OK");
            }
        }
    }
}
