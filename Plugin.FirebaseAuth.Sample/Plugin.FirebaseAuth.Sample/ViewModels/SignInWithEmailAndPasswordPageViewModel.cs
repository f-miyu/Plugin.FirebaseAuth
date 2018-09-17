using System;
using System.Linq;
using System.Reactive.Linq;
using Prism.Navigation;
using Prism.Services;
using Reactive.Bindings;
using Plugin.FirebaseAuth.Sample.Extensins;

namespace Plugin.FirebaseAuth.Sample.ViewModels
{
    public class SignInWithEmailAndPasswordPageViewModel : ViewModelBaseResult<IUser>
    {
        public ReactivePropertySlim<string> Email { get; } = new ReactivePropertySlim<string>();
        public ReactivePropertySlim<string> Password { get; } = new ReactivePropertySlim<string>();

        public AsyncReactiveCommand SignInCommand { get; }
        public AsyncReactiveCommand ResetPasswordCommand { get; }

        private readonly IPageDialogService _pageDialogService;

        public SignInWithEmailAndPasswordPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;

            Title = "Sign In ";

            SignInCommand = new[] {
                Email.Select(s => string.IsNullOrEmpty(s)),
                Password.Select(s => string.IsNullOrEmpty(s))
            }.CombineLatest(x => x.All(y => !y))
             .ToAsyncReactiveCommand();

            SignInCommand.Subscribe(async () =>
            {
                try
                {
                    var result = await CrossFirebaseAuth.Current
                                                        .SignInWithEmailAndPasswordAsync(Email.Value, Password.Value);                                 

                    await NavigationService.GoBackAsync(result.User);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);

                    await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
                }
            });

            ResetPasswordCommand = new[] {
                Email.Select(s => string.IsNullOrEmpty(s)),
            }.CombineLatest(x => x.All(y => !y))
             .ToAsyncReactiveCommand();

            ResetPasswordCommand.Subscribe(async () =>
            {
                try
                {
                    await CrossFirebaseAuth.Current
                                           .SendPasswordResetEmailAsync(Email.Value);

                    await _pageDialogService.DisplayAlertAsync(null, "Email has been sent.", "OK");
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
