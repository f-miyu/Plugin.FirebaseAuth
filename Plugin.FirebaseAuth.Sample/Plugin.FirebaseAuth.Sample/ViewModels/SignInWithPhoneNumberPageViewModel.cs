using System;
using Reactive.Bindings;
using System.Reactive.Linq;
using Prism.Navigation;
using Plugin.FirebaseAuth.Sample.Extensins;
using Prism.Services;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Plugin.FirebaseAuth.Sample.ViewModels
{
    public class SignInWithPhoneNumberPageViewModel : ViewModelBaseResult<IAuthResult>
    {
        public ReactivePropertySlim<string> PhoneNumber { get; } = new ReactivePropertySlim<string>();
        public AsyncReactiveCommand SignInCommand { get; }

        private readonly IPageDialogService _pageDialogService;

        public SignInWithPhoneNumberPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Phone Numbe";

            _pageDialogService = pageDialogService;

            SignInCommand = PhoneNumber.Select(s => !string.IsNullOrEmpty(s)).ToAsyncReactiveCommand();

            SignInCommand.Subscribe(async () =>
            {
                try
                {
                    var verificationResult = await CrossFirebaseAuth.Current.PhoneAuthProvider
                                                                    .VerifyPhoneNumberAsync(CrossFirebaseAuth.Current.Instance, PhoneNumber.Value);

                    if (verificationResult.Credential != null)
                    {
                        var result = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(verificationResult.Credential);

                        await NavigationService.GoBackAsync(result);
                    }
                    else
                    {
                        var verificationCode = await NavigationService.NavigateAsync<VerificationCodePageViewModel, string>();

                        if (verificationCode != null)
                        {
                            var credential = CrossFirebaseAuth.Current.PhoneAuthProvider.GetCredential(CrossFirebaseAuth.Current.Instance, verificationResult.VerificationId, verificationCode);

                            var result = await CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);

                            await NavigationService.GoBackAsync(result);
                        }
                    }
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
