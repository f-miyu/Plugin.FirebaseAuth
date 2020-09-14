using System;
using Prism.Navigation;
using Reactive.Bindings;
using Prism.Services;
using System.Threading.Tasks;
using Plugin.FirebaseAuth.Sample.Extensins;
using System.Reactive.Linq;
using System.Linq;
using Plugin.FirebaseAuth.Sample.Services;

namespace Plugin.FirebaseAuth.Sample.ViewModels
{
    public class UserPageViewModel : ViewModelBase, IVerificationCodeGettable
    {
        public ReactivePropertySlim<string> Name { get; }
        public ReactivePropertySlim<string> Email { get; }
        public ReactivePropertySlim<string> PhoneNumber { get; }

        private readonly ReactivePropertySlim<bool> _isLinkedWithGoogle = new ReactivePropertySlim<bool>();
        public IReadOnlyReactiveProperty<bool> IsLinkedWithGoogle => _isLinkedWithGoogle;

        private readonly ReactivePropertySlim<bool> _isLinkedWithTwitter = new ReactivePropertySlim<bool>();
        public IReadOnlyReactiveProperty<bool> IsLinkedWithTwitter => _isLinkedWithTwitter;

        private readonly ReactivePropertySlim<bool> _isLinkedWithFacebook = new ReactivePropertySlim<bool>();
        public IReadOnlyReactiveProperty<bool> IsLinkedWithFacebook => _isLinkedWithFacebook;

        private readonly ReactivePropertySlim<bool> _isLinkedWithGitHub = new ReactivePropertySlim<bool>();
        public IReadOnlyReactiveProperty<bool> IsLinkedWithGitHub => _isLinkedWithGitHub;

        private readonly ReactivePropertySlim<bool> _isLinkedWithYahoo = new ReactivePropertySlim<bool>();
        public IReadOnlyReactiveProperty<bool> IsLinkedWithYahoo => _isLinkedWithYahoo;

        private readonly ReactivePropertySlim<bool> _isLinkedWithMicrosoft = new ReactivePropertySlim<bool>();
        public IReadOnlyReactiveProperty<bool> IsLinkedWithMicrosoft => _isLinkedWithMicrosoft;

        private readonly ReactivePropertySlim<bool> _isLinkedWithApple = new ReactivePropertySlim<bool>();
        public IReadOnlyReactiveProperty<bool> IsLinkedWithApple => _isLinkedWithApple;

        private readonly ReactivePropertySlim<bool> _isEnrolledMultiFactor = new ReactivePropertySlim<bool>();
        public IReadOnlyReactiveProperty<bool> IsEnrolledMultiFactor => _isEnrolledMultiFactor;

        public AsyncReactiveCommand UpdateNameCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand UpdateEmailCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand UpdatePhoneNumberCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignOutCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand LinkOrUnlinkWithGoogleCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand LinkOrUnlinkWithTwitterCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand LinkOrUnlinkWithFacebookCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand LinkOrUnlinkWithGitHubCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand LinkOrUnlinkWithYahooCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand LinkOrUnlinkWithMicrosoftCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand LinkOrUnlinkWithAppleCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand EnrollOrUnenrollMultiFactorCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand DeleteCommand { get; } = new AsyncReactiveCommand();

        private readonly IPageDialogService _pageDialogService;
        private readonly IGoogleService _googleService;
        private readonly IFacebookService _facebookService;
        private readonly IAppleService _appleService;

        private readonly MultiFactorService _multiFactorService;

        private readonly IAuth _auth = CrossFirebaseAuth.Current.Instance;

        public UserPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IGoogleService googleService, IFacebookService facebookService, IAppleService appleService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _googleService = googleService;
            _facebookService = facebookService;
            _appleService = appleService;

            _multiFactorService = new MultiFactorService(this);

            Title = "User";

            var user = _auth.CurrentUser;

            Update(user);

            Name = new ReactivePropertySlim<string>(user?.DisplayName);
            Email = new ReactivePropertySlim<string>(user?.Email);
            PhoneNumber = new ReactivePropertySlim<string>(user?.PhoneNumber);

            _isEnrolledMultiFactor.Value = user.MultiFactor.EnrolledFactors.Any();

            UpdateNameCommand.Subscribe(UpdateName);
            UpdateEmailCommand.Subscribe(UpdateEmail);
            UpdatePhoneNumberCommand.Subscribe(UpdatePhoneNumber);
            SignOutCommand.Subscribe(SignOut);
            LinkOrUnlinkWithGoogleCommand.Subscribe(() => IsLinkedWithGoogle.Value ? UnlinkWithProvider("google.com") : LinkWithGoogle());
            LinkOrUnlinkWithTwitterCommand.Subscribe(() => IsLinkedWithTwitter.Value ? UnlinkWithProvider("twitter.com") : LinkWithProvider("twitter.com"));
            LinkOrUnlinkWithFacebookCommand.Subscribe(() => IsLinkedWithFacebook.Value ? UnlinkWithProvider("facebook.com") : LinkWithFacebook());
            LinkOrUnlinkWithGitHubCommand.Subscribe(() => IsLinkedWithGitHub.Value ? UnlinkWithProvider("github.com") : LinkWithProvider("github.com"));
            LinkOrUnlinkWithYahooCommand.Subscribe(() => IsLinkedWithYahoo.Value ? UnlinkWithProvider("yahoo.com") : LinkWithProvider("yahoo.com"));
            LinkOrUnlinkWithMicrosoftCommand.Subscribe(() => IsLinkedWithMicrosoft.Value ? UnlinkWithProvider("microsoft.com") : LinkWithProvider("microsoft.com"));
            LinkOrUnlinkWithAppleCommand.Subscribe(() => IsLinkedWithApple.Value ? UnlinkWithProvider("apple.com") : LinkWithApple());
            EnrollOrUnenrollMultiFactorCommand.Subscribe(() => IsEnrolledMultiFactor.Value ? UnenrollMultiFactor() : EnrollMultiFactor());
            DeleteCommand.Subscribe(Delete);
        }

        private void Update(IUser user)
        {
            if (user != null)
            {
                var isLinkedWithGoogle = false;
                var isLinkedWithTwitter = false;
                var isLinkedWithFacebook = false;
                var isLinkedWithGitHub = false;
                var isLinkedWithYahoo = false;
                var isLinkedWithMicrosoft = false;
                var isLindedWithApple = false;

                foreach (var info in user.ProviderData)
                {
                    switch (info.ProviderId)
                    {
                        case "google.com":
                            isLinkedWithGoogle = true;
                            break;
                        case "twitter.com":
                            isLinkedWithTwitter = true;
                            break;
                        case "facebook.com":
                            isLinkedWithFacebook = true;
                            break;
                        case "github.com":
                            isLinkedWithGitHub = true;
                            break;
                        case "yahoo.com":
                            isLinkedWithYahoo = true;
                            break;
                        case "microsoft.com":
                            isLinkedWithMicrosoft = true;
                            break;
                        case "apple.com":
                            isLindedWithApple = true;
                            break;
                    }
                }

                _isLinkedWithGoogle.Value = isLinkedWithGoogle;
                _isLinkedWithTwitter.Value = isLinkedWithTwitter;
                _isLinkedWithFacebook.Value = isLinkedWithFacebook;
                _isLinkedWithGitHub.Value = isLinkedWithGitHub;
                _isLinkedWithYahoo.Value = isLinkedWithYahoo;
                _isLinkedWithMicrosoft.Value = isLinkedWithMicrosoft;
                _isLinkedWithApple.Value = isLindedWithApple;
            }
        }

        private async Task UpdateName()
        {
            var user = _auth.CurrentUser;
            if (user == null) return;

            try
            {
                await user.UpdateProfileAsync(new UserProfileChangeRequest { DisplayName = Name.Value });

                await _pageDialogService.DisplayAlertAsync("Success", null, "OK");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task UpdateEmail()
        {
            var user = CrossFirebaseAuth.Current.Instance.CurrentUser;
            if (user == null) return;

            try
            {
                await user.UpdateEmailAsync(Email.Value);

                await _pageDialogService.DisplayAlertAsync("Success", null, "OK");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task UpdatePhoneNumber()
        {
            var user = _auth.CurrentUser;
            if (user == null) return;

            try
            {
                var verificationResult = await CrossFirebaseAuth.Current.PhoneAuthProvider
                    .VerifyPhoneNumberAsync(PhoneNumber.Value);

                if (verificationResult.Credential != null)
                {
                    await user.UpdatePhoneNumberAsync(verificationResult.Credential);

                    await _pageDialogService.DisplayAlertAsync("Success", null, "OK");
                }
                else
                {
                    var verificationCode = await NavigationService.NavigateAsync<VerificationCodePageViewModel, string>();

                    if (verificationCode != null)
                    {
                        var credential = CrossFirebaseAuth.Current.PhoneAuthProvider.GetCredential(CrossFirebaseAuth.Current.Instance, verificationResult.VerificationId, verificationCode);

                        await user.UpdatePhoneNumberAsync(credential);

                        await _pageDialogService.DisplayAlertAsync("Success", null, "OK");
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task SignOut()
        {
            try
            {
                CrossFirebaseAuth.Current.Instance.SignOut();

                await NavigationService.GoBackAsync();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task LinkWithGoogle()
        {
            var user = _auth.CurrentUser;
            if (user == null) return;

            try
            {
                var (idToken, accessToken) = await _googleService.GetCredentialAsync();

                var credential = CrossFirebaseAuth.Current
                    .GoogleAuthProvider
                    .GetCredential(idToken, accessToken);

                var result = await user.LinkWithCredentialAsync(credential);

                Update(user);

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

        private async Task LinkWithFacebook()
        {
            var user = _auth.CurrentUser;
            if (user == null) return;

            try
            {
                var accessToken = await _facebookService.GetCredentialAsync();

                var credential = CrossFirebaseAuth.Current
                    .FacebookAuthProvider
                    .GetCredential(accessToken);

                var result = await user.LinkWithCredentialAsync(credential);

                Update(user);

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

        private async Task LinkWithApple()
        {
            var user = _auth.CurrentUser;
            if (user == null) return;

            try
            {
                IAuthResult result;

                var (idToken, rawNonce) = await _appleService.GetCredentialAsync();
                if (idToken != null)
                {
                    var credential = CrossFirebaseAuth.Current
                        .OAuthProvider
                        .GetCredential("apple.com", idToken, rawNonce: rawNonce);

                    result = await user.LinkWithCredentialAsync(credential);
                }
                else
                {
                    var porvider = new OAuthProvider("apple.com")
                    {
                        Scopes = new[] { "email", "name" }
                    };

                    result = await user.LinkWithProviderAsync(porvider);
                }

                Update(user);

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

        private async Task LinkWithProvider(string providerId)
        {
            var user = _auth.CurrentUser;
            if (user == null) return;

            try
            {
                var porvider = new OAuthProvider(providerId);

                var result = await user.LinkWithProviderAsync(porvider);

                Update(user);

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

        private async Task UnlinkWithProvider(string providerId)
        {
            var user = _auth.CurrentUser;
            if (user == null) return;

            try
            {
                var result = await user.UnlinkAsync(providerId);

                Update(user);

                await _pageDialogService.DisplayAlertAsync("Success", result.DisplayName, "OK");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task Delete()
        {
            var user = _auth.CurrentUser;
            if (user == null) return;

            try
            {
                await user.DeleteAsync();

                await _pageDialogService.DisplayAlertAsync("Success", null, "OK");

                await NavigationService.GoBackAsync();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task EnrollMultiFactor()
        {
            var user = _auth.CurrentUser;
            if (user == null) return;

            try
            {
                var session = await user.MultiFactor.GetSessionAsync();

                var credential = await NavigationService.NavigateAsync<SignInWithPhoneNumberPageViewModel, IMultiFactorSession, IPhoneAuthCredential>(session);

                if (credential == null)
                {
                    await _pageDialogService.DisplayAlertAsync("Failure", "Cancelled", "OK");
                    return;
                }

                var assertion = CrossFirebaseAuth.Current.PhoneMultiFactorGenerator.GetAssertion(credential);

                await user.MultiFactor.EnrollAsync(assertion, null);

                _isEnrolledMultiFactor.Value = true;

                await _pageDialogService.DisplayAlertAsync("Success", null, "OK");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task UnenrollMultiFactor()
        {
            var user = _auth.CurrentUser;
            if (user == null) return;

            try
            {
                var multiFactor = user.MultiFactor;

                await Task.WhenAll(multiFactor.EnrolledFactors.Select(info => multiFactor.UnenrollAsync(info)));

                _isEnrolledMultiFactor.Value = false;

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
