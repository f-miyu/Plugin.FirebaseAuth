using System;
using Reactive.Bindings;
using System.Reactive.Linq;
using Prism.Navigation;
using Plugin.FirebaseAuth.Sample.Extensins;
using Prism.Services;
using Xamarin.Forms;

namespace Plugin.FirebaseAuth.Sample.ViewModels
{
    public class LoginWithPhoneNumberPageViewModel : ViewModelBaseResult<(IAuthResult Result, Exception Exception)>
    {
        public ReactivePropertySlim<string> PhoneNumber { get; } = new ReactivePropertySlim<string>();
        public AsyncReactiveCommand LoginCommand { get; }

        private readonly IPageDialogService _pageDialogService;

        public LoginWithPhoneNumberPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Phone Numbe";

            _pageDialogService = pageDialogService;

            LoginCommand = PhoneNumber.Select(s => !string.IsNullOrEmpty(s)).ToAsyncReactiveCommand();

            LoginCommand.Subscribe(async () =>
            {
                try
                {
                    var (credential, verificationId) = await CrossFirebaseAuth.Current
                                                                              .PhoneAuthProvider
                                                                              .VerifyPhoneNumberAsync(PhoneNumber.Value);

                    if (credential != null)
                    {
                        var result = await CrossFirebaseAuth.Current.SignInWithCredentialAsync(credential);

                        await NavigationService.GoBackAsync<(IAuthResult Result, Exception Exception)>((result, null));
                    }
                    else
                    {
                        var verificationCode = await NavigationService.NavigateAsync<VerificationCodePageViewModel, string>();

                        if (verificationCode != null)
                        {
                            credential = CrossFirebaseAuth.Current.PhoneAuthProvider.GetCredential(verificationId, verificationCode);

                            var result = await CrossFirebaseAuth.Current.SignInWithCredentialAsync(credential);

                            await NavigationService.GoBackAsync<(IAuthResult Result, Exception Exception)>((result, null));
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);

                    await NavigationService.GoBackAsync<(IAuthResult Result, Exception Exception)>((null, e));
                }
            });
        }
    }
}
