using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Plugin.FirebaseAuth.Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            FirebaseAuth.CrossFirebaseAuth.Current
                        .AuthStateChanged += (sender, e) =>
                        {

                        };

            LoginCommand = new DelegateCommand(async () =>
            {
            });
        }
    }
}
