using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Plugin.FirebaseAuth.Sample.Services;
using Reactive.Bindings;

namespace Plugin.FirebaseAuth.Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public AsyncReactiveCommand LoginCommand { get; } = new AsyncReactiveCommand();

        private readonly IGoogleSignInService _googleSignInService;

        public MainPageViewModel(INavigationService navigationService, IGoogleSignInService googleSignInService)
            : base(navigationService)
        {
            _googleSignInService = googleSignInService;

            Title = "Main Page";

            LoginCommand.Subscribe(async () =>
            {
                var (idToken, accessToken) = await _googleSignInService.SignIn();

                if (idToken != null && accessToken != null)
                {
                    var credential = CrossFirebaseAuth.Current
                                                      .GoogleAuthProvider
                                                      .GetCredential(idToken, accessToken);

                    var resutl = await CrossFirebaseAuth.Current
                                                        .SignInWithCredentialAsync(credential);
                }
            });
        }

        public async override void OnAppearing()
        {
            base.OnAppearing();

        }
    }
}
