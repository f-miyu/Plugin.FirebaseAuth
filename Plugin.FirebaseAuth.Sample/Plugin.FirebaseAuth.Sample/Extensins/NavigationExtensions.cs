using System;
using System.Threading.Tasks;
using Plugin.FirebaseAuth.Sample.ViewModels;
using Prism.Mvvm;
using Prism.Navigation;

namespace Plugin.FirebaseAuth.Sample.Extensins
{
    public static class NavigationExtensions
    {
        public static Task NavigateAsync<TViewModel>(this INavigationService navigationService, NavigationParameters parameters = null, bool? useModalNavigation = null, bool animated = true, bool wrapInNavigationPage = false, bool noHistory = false) where TViewModel : BindableBase
        {
            var name = typeof(TViewModel).Name.Replace("ViewModel", "");

            if (wrapInNavigationPage)
            {
                name = "NavigationPage/" + name;
            }

            if (noHistory)
            {
                name = "/" + name;
            }

            return navigationService.NavigateAsync(name, parameters, useModalNavigation, animated);
        }

        public static Task NavigateAsync<TViewModel, TParameter>(this INavigationService navigationService, TParameter parameter, bool? useModalNavigation = null, bool animated = true, bool wrapInNavigationPage = false, bool noHistory = false) where TViewModel : ViewModelBase<TParameter>
        {
            var parameters = new NavigationParameters
            {
                {ViewModelBase.ParameterKey,  parameter}
            };

            return navigationService.NavigateAsync<TViewModel>(parameters, useModalNavigation, animated, wrapInNavigationPage, noHistory);
        }
    }
}
