using System;
using Prism.Navigation;
using Reactive.Bindings;
using Prism.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.FirebaseAuth.Sample.Extensins;
using System.Reactive.Linq;
using Plugin.FirebaseAuth.Sample.Auth;
using System.Linq;
using System.Reactive.Subjects;
using System.Reactive;
using System.Xml;

namespace Plugin.FirebaseAuth.Sample.ViewModels
{
    public class UserPageViewModel : ViewModelBase
    {
        public ReactivePropertySlim<string> Name { get; }
        public ReactivePropertySlim<string> Email { get; }
        public ReactivePropertySlim<string> PhoneNumber { get; }
        public ReadOnlyReactivePropertySlim<bool> IsLinkedWithGoogle { get; }
        public ReadOnlyReactivePropertySlim<bool> IsLinkedWithTwitter { get; }
        public ReadOnlyReactivePropertySlim<bool> IsLinkedWithFacebook { get; }
        public ReadOnlyReactivePropertySlim<bool> IsLinkedWithGitHub { get; }

        public AsyncReactiveCommand UpdateNameCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand UpdateEmailCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand UpdatePhoneNumberCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand SignOutCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand LinkOrUnlinkWithGoogleCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand LinkOrUnlinkWithTwitterCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand LinkOrUnlinkWithFacebookCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand LinkOrUnlinkWithGitHubCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand DeleteCommand { get; } = new AsyncReactiveCommand();

        private readonly IPageDialogService _pageDialogService;
        private readonly IAuthService _authService;

        private ReactivePropertySlim<IUser> _user = new ReactivePropertySlim<IUser>();

        public UserPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IAuthService authService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _authService = authService;

            Title = "User";

            _user.Value = CrossFirebaseAuth.Current.Instance.CurrentUser;

            Name = new ReactivePropertySlim<string>(_user.Value?.DisplayName);
            Email = new ReactivePropertySlim<string>(_user.Value?.Email);
            PhoneNumber = new ReactivePropertySlim<string>(_user.Value?.PhoneNumber);

            IsLinkedWithGoogle = _user.Where(user => user != null)
                                      .Select(user => user.ProviderData.FirstOrDefault(data => data.ProviderId == CrossFirebaseAuth.Current.GoogleAuthProvider.ProviderId) != null)
                                      .ToReadOnlyReactivePropertySlim();

            IsLinkedWithTwitter = _user.Where(user => user != null)
                                       .Select(user => user.ProviderData.FirstOrDefault(data => data.ProviderId == CrossFirebaseAuth.Current.TwitterAuthProvider.ProviderId) != null)
                                       .ToReadOnlyReactivePropertySlim();

            IsLinkedWithFacebook = _user.Where(user => user != null)
                                        .Select(user => user.ProviderData.FirstOrDefault(data => data.ProviderId == CrossFirebaseAuth.Current.FacebookAuthProvider.ProviderId) != null)
                                        .ToReadOnlyReactivePropertySlim();

            IsLinkedWithGitHub = _user.Where(user => user != null)
                                      .Select(user => user.ProviderData.FirstOrDefault(data => data.ProviderId == CrossFirebaseAuth.Current.GitHubAuthProvider.ProviderId) != null)
                                      .ToReadOnlyReactivePropertySlim();

            _user.Where(user => user != null)
                 .Select(user => user.DisplayName)
                 .DistinctUntilChanged()
                 .Subscribe(name => Name.Value = name);

            _user.Where(user => user != null)
                 .Select(user => user.Email)
                 .DistinctUntilChanged()
                 .Subscribe(email => Email.Value = email);

            _user.Where(user => user != null)
                 .Select(user => user.PhoneNumber)
                 .DistinctUntilChanged()
                 .Subscribe(phoneNumber => PhoneNumber.Value = phoneNumber);

            UpdateNameCommand.Subscribe(UpdateName);
            UpdateEmailCommand.Subscribe(UpdateEmail);
            UpdatePhoneNumberCommand.Subscribe(UpdatePhoneNumber);
            SignOutCommand.Subscribe(SignOut);
            LinkOrUnlinkWithGoogleCommand.Subscribe(() => IsLinkedWithGoogle.Value ? UnlinkWithGoogle() : LinkWithGoogle());
            LinkOrUnlinkWithTwitterCommand.Subscribe(() => IsLinkedWithTwitter.Value ? UnlinkWithTwitter() : LinkWithTwitter());
            LinkOrUnlinkWithFacebookCommand.Subscribe(() => IsLinkedWithFacebook.Value ? UnlinkWithFacebook() : LinkWithFacebook());
            LinkOrUnlinkWithGitHubCommand.Subscribe(() => IsLinkedWithGitHub.Value ? UnlinkWithGitHub() : LinkWithGitHub());
            DeleteCommand.Subscribe(Delete);
        }

        private async Task UpdateName()
        {
            var user = _user.Value;
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
            var user = _user.Value;
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
            var user = _user.Value;
            if (user == null) return;

            try
            {
                var verificationResult = await CrossFirebaseAuth.Current.PhoneAuthProvider
                                                                .VerifyPhoneNumberAsync(CrossFirebaseAuth.Current.Instance, PhoneNumber.Value);

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
            var user = _user.Value;
            if (user == null) return;

            try
            {
                var (idToken, accessToken) = await _authService.LoginWithGoogle();

                if (idToken != null)
                {
                    var credential = CrossFirebaseAuth.Current
                                                      .GoogleAuthProvider
                                                      .GetCredential(idToken, accessToken);

                    var result = await user.LinkWithCredentialAsync(credential);

                    _user.Value = result.User;

                    await _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task UnlinkWithGoogle()
        {
            var user = _user.Value;
            if (user == null) return;

            try
            {
                var result = await user.UnlinkAsync(CrossFirebaseAuth.Current.GoogleAuthProvider.ProviderId);

                _user.Value = result;

                await _pageDialogService.DisplayAlertAsync("Success", result.DisplayName, "OK");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task LinkWithTwitter()
        {
            var user = _user.Value;
            if (user == null) return;

            try
            {
                var (token, secret) = await _authService.LoginWithTwitter();

                if (token != null && secret != null)
                {
                    var credential = CrossFirebaseAuth.Current
                                                      .TwitterAuthProvider
                                                      .GetCredential(token, secret);

                    var result = await user.LinkWithCredentialAsync(credential);

                    _user.Value = result.User;

                    await _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task UnlinkWithTwitter()
        {
            var user = _user.Value;
            if (user == null) return;

            try
            {
                var result = await user.UnlinkAsync(CrossFirebaseAuth.Current.TwitterAuthProvider.ProviderId);

                _user.Value = result;

                await _pageDialogService.DisplayAlertAsync("Success", result.DisplayName, "OK");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task LinkWithFacebook()
        {
            var user = _user.Value;
            if (user == null) return;

            try
            {
                var accessToken = await _authService.LoginWithFacebook();

                if (accessToken != null)
                {
                    var credential = CrossFirebaseAuth.Current
                                                      .FacebookAuthProvider
                                                      .GetCredential(accessToken);

                    var result = await user.LinkWithCredentialAsync(credential);

                    _user.Value = result.User;

                    await _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task UnlinkWithFacebook()
        {
            var user = _user.Value;
            if (user == null) return;

            try
            {
                var result = await user.UnlinkAsync(CrossFirebaseAuth.Current.FacebookAuthProvider.ProviderId);

                _user.Value = result;

                await _pageDialogService.DisplayAlertAsync("Success", result.DisplayName, "OK");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task LinkWithGitHub()
        {
            var user = _user.Value;
            if (user == null) return;

            try
            {
                var accessToken = await _authService.LoginWithGitHub();

                if (accessToken != null)
                {
                    var credential = CrossFirebaseAuth.Current
                                                      .GitHubAuthProvider
                                                      .GetCredential(accessToken);

                    var result = await user.LinkWithCredentialAsync(credential);

                    _user.Value = result.User;

                    await _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
            }
        }

        private async Task UnlinkWithGitHub()
        {
            var user = _user.Value;
            if (user == null) return;

            try
            {
                var result = await user.UnlinkAsync(CrossFirebaseAuth.Current.GitHubAuthProvider.ProviderId);

                _user.Value = result;

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
            var user = _user.Value;
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
    }
}
