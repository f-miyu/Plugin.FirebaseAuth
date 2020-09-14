using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reactive.Bindings;
using Xamarin.Forms;
using System.Threading.Tasks;
using Prism.Services;
using DryIoc;
using Plugin.FirebaseAuth.Sample.Extensins;
using Plugin.FirebaseAuth.Sample.Services;
using Prism.Ioc;

namespace Plugin.FirebaseAuth.Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IVerificationCodeGettable
    {
        public AsyncReactiveCommand SignUpCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInWithEmailAndPasswordCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInWithGoogleCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInWithTwitterCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInWithFacebookCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInWithGitHubCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInWithYahooCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInWithMicrosoftCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInWithAppleCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInWithPhoneNumberCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignInAnonymouslyCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand ShowUserCommand { get; }

        private readonly IPageDialogService _pageDialogService;
        private ReactivePropertySlim<bool> _isSignedIn = new ReactivePropertySlim<bool>();
        private IListenerRegistration _registration;
        private readonly IGoogleService _googleService;
        private readonly IFacebookService _facebookService;
        private readonly IAppleService _appleService;
        private readonly MultiFactorService _multiFactorService;

        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IGoogleService googleService, IFacebookService facebookService, IAppleService appleService)
            : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _googleService = googleService;
            _facebookService = facebookService;
            _appleService = appleService;

            _multiFactorService = new MultiFactorService(this);

            Title = "Main Page";

            _registration = CrossFirebaseAuth.Current.Instance.AddAuthStateChangedListener((auth) =>
            {
                _isSignedIn.Value = auth.CurrentUser != null;
            });

            ShowUserCommand = _isSignedIn.ToAsyncReactiveCommand();

            SignUpCommand.Subscribe(SignUp);
            SignInWithEmailAndPasswordCommand.Subscribe(SignInWithEmailAndPassword);
            SignInWithGoogleCommand.Subscribe(SignInWithGoogle);
            SignInWithTwitterCommand.Subscribe(() => SignInWithProvider("twitter.com"));
            SignInWithFacebookCommand.Subscribe(SignInWithFacebook);
            SignInWithGitHubCommand.Subscribe(() => SignInWithProvider("github.com"));
            SignInWithYahooCommand.Subscribe(() => SignInWithProvider("yahoo.com"));
            SignInWithMicrosoftCommand.Subscribe(() => SignInWithProvider("microsoft.com"));
            SignInWithAppleCommand.Subscribe(SignInWithApple);
            SignInWithPhoneNumberCommand.Subscribe(SignInWithPhoneNumber);
            SignInAnonymouslyCommand.Subscribe(SignInSignInAnonymously);
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
                var (idToken, accessToken) = await _googleService.GetCredentialAsync();

                var credential = CrossFirebaseAuth.Current
                    .GoogleAuthProvider
                    .GetCredential(idToken, accessToken);

                var result = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);

                await _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
            }
            catch (FirebaseAuthException e)
            {
                await ResolveAsync(e);
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
                var accessToken = await _facebookService.GetCredentialAsync();

                var credential = CrossFirebaseAuth.Current
                    .FacebookAuthProvider
                    .GetCredential(accessToken);

                var result = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);

                await _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
            }
            catch (FirebaseAuthException e)
            {
                await ResolveAsync(e);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task SignInWithApple()
        {
            try
            {
                IAuthResult result;

                var (idToken, rawNonce) = await _appleService.GetCredentialAsync();
                if (idToken != null)
                {
                    var credential = CrossFirebaseAuth.Current.OAuthProvider
                        .GetCredential("apple.com", idToken, rawNonce: rawNonce);

                    result = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);
                }
                else
                {
                    var porvider = new OAuthProvider("apple.com")
                    {
                        Scopes = new[] { "email", "name" }
                    };

                    result = await CrossFirebaseAuth.Current.Instance.SignInWithProviderAsync(porvider);
                }

                await _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
            }
            catch (FirebaseAuthException e)
            {
                await ResolveAsync(e);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task SignInWithProvider(string providerId)
        {
            try
            {
                var porvider = new OAuthProvider(providerId);

                var result = await CrossFirebaseAuth.Current.Instance.SignInWithProviderAsync(porvider);

                await _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
            }
            catch (FirebaseAuthException e)
            {
                await ResolveAsync(e);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task SignInWithPhoneNumber()
        {
            try
            {
                var credential = await NavigationService.NavigateAsync<SignInWithPhoneNumberPageViewModel, IMultiFactorSession, IPhoneAuthCredential>(null);

                if (credential != null)
                {
                    var result = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);

                    await _pageDialogService.DisplayAlertAsync("Success", result.User.PhoneNumber, "OK");
                }
            }
            catch (FirebaseAuthException e)
            {
                await ResolveAsync(e);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task SignInSignInAnonymously()
        {
            try
            {
                var result = await CrossFirebaseAuth.Current.Instance.SignInAnonymouslyAsync();

                await _pageDialogService.DisplayAlertAsync("Success", null, "OK");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task ResolveAsync(FirebaseAuthException firebaseAuthException)
        {
            if (firebaseAuthException.Resolver == null)
            {
                System.Diagnostics.Debug.WriteLine(firebaseAuthException);

                await _pageDialogService.DisplayAlertAsync("Failure", firebaseAuthException.Message, "OK");
            }
            else
            {
                try
                {
                    var result = await _multiFactorService.ResolveAsync(firebaseAuthException.Resolver);

                    await _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);

                    await _pageDialogService.DisplayAlertAsync("Failure", ex.Message, "OK");
                }
            }
        }

        public Task<string> GetVerificationCodeAsync()
        {
            return NavigationService.NavigateAsync<VerificationCodePageViewModel, string>();
        }
    }
}
