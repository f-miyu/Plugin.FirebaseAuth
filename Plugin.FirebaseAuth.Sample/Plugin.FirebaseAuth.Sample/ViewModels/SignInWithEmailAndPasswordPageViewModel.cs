using System;
using System.Linq;
using System.Reactive.Linq;
using Prism.Navigation;
using Prism.Services;
using Reactive.Bindings;
using Plugin.FirebaseAuth.Sample.Extensins;
using Plugin.FirebaseAuth.Sample.Services;
using System.Threading.Tasks;

namespace Plugin.FirebaseAuth.Sample.ViewModels
{
    public class SignInWithEmailAndPasswordPageViewModel : ViewModelBaseResult<IUser>, IVerificationCodeGettable
    {
        public ReactivePropertySlim<string> Email { get; } = new ReactivePropertySlim<string>();
        public ReactivePropertySlim<string> Password { get; } = new ReactivePropertySlim<string>();

        public AsyncReactiveCommand SignInCommand { get; }
        public AsyncReactiveCommand ResetPasswordCommand { get; }

        private readonly IPageDialogService _pageDialogService;
        private readonly MultiFactorService _multiFactorService;

        public SignInWithEmailAndPasswordPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;

            _multiFactorService = new MultiFactorService(this);

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
                                                        .Instance
                                                        .SignInWithEmailAndPasswordAsync(Email.Value, Password.Value);

                    await NavigationService.GoBackAsync(result.User);
                }
                catch (FirebaseAuthException e)
                {
                    if (e.Resolver == null)
                    {
                        System.Diagnostics.Debug.WriteLine(e);

                        await _pageDialogService.DisplayAlertAsync("Failure", e.Message, "OK");
                    }
                    else
                    {
                        try
                        {
                            var result = await _multiFactorService.ResolveAsync(e.Resolver);

                            await _pageDialogService.DisplayAlertAsync("Success", result.User.DisplayName, "OK");
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex);

                            await _pageDialogService.DisplayAlertAsync("Failure", ex.Message, "OK");
                        }
                    }
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
                                           .Instance
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

        public Task<string> GetVerificationCodeAsync()
        {
            return NavigationService.NavigateAsync<VerificationCodePageViewModel, string>();
        }
    }
}
