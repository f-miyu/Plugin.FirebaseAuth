using System;
using Prism.Navigation;
using Reactive.Bindings;
using System.Reactive.Linq;
using System.Globalization;
using System.Linq;
using Prism.Services;
using Plugin.FirebaseAuth.Sample.Extensins;
namespace Plugin.FirebaseAuth.Sample.ViewModels
{
    public class SignUpPageViewModel : ViewModelBaseResult<IUser>
    {
        public ReactivePropertySlim<string> Name { get; } = new ReactivePropertySlim<string>();
        public ReactivePropertySlim<string> Email { get; } = new ReactivePropertySlim<string>();
        public ReactivePropertySlim<string> Password { get; } = new ReactivePropertySlim<string>();
        public AsyncReactiveCommand SignUpCommand { get; }

        private readonly IPageDialogService _pageDialogService;

        public SignUpPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;

            Title = "Sign Up";

            SignUpCommand = new[] {
                Name.Select(s=> string.IsNullOrEmpty(s)),
                Email.Select(s => string.IsNullOrEmpty(s)),
                Password.Select(s => string.IsNullOrEmpty(s))
            }.CombineLatest(x => x.All(y => !y))
             .ToAsyncReactiveCommand();

            SignUpCommand.Subscribe(async () =>
            {
                try
                {
                    var result = await CrossFirebaseAuth.Current
                                                        .Instance
                                                        .CreateUserWithEmailAndPasswordAsync(Email.Value, Password.Value);

                    await result.User.UpdateProfileAsync(new UserProfileChangeRequest { DisplayName = Name.Value });

                    var user = CrossFirebaseAuth.Current.Instance.CurrentUser;

                    await user.SendEmailVerificationAsync();

                    await NavigationService.GoBackAsync(user);

                }
                catch (FirebaseAuthException e)
                {
                    System.Diagnostics.Debug.WriteLine(e);

                    var message = e.Reason ?? e.Message;

                    await _pageDialogService.DisplayAlertAsync("Failure", message, "OK");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);

                    await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
                }
            });
        }
    }
}