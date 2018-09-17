using System;
using Prism.Navigation;
using Reactive.Bindings;
using System.Reactive.Linq;
using Plugin.FirebaseAuth.Sample.Extensins;
using System.Threading.Tasks;

namespace Plugin.FirebaseAuth.Sample.ViewModels
{
    public class VerificationCodePageViewModel : ViewModelBaseResult<string>
    {
        public ReactivePropertySlim<string> VerificationCode { get; } = new ReactivePropertySlim<string>();
        public ReactiveCommand OKCommand { get; }
        public ReactiveCommand CancelCommand { get; }

        public VerificationCodePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Verification Code";

            OKCommand = VerificationCode.Select(s => !string.IsNullOrEmpty(s)).ToReactiveCommand();
            OKCommand.Subscribe(() => navigationService.GoBackAsync(VerificationCode.Value));

            CancelCommand = new ReactiveCommand();
            CancelCommand.Subscribe(() => navigationService.GoBackAsync());
        }
    }
}
