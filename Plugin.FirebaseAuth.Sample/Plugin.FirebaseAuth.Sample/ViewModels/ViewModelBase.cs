using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Prism.AppModel;
using System.Threading.Tasks;

namespace Plugin.FirebaseAuth.Sample.ViewModels
{
    public abstract class ViewModelBase : BindableBase, INavigationAware, IDestructible, IPageLifecycleAware
    {
        public static readonly string ParameterKey = "parameter";
        public static readonly string TaskCompletionSourceKey = "taskCompletionSourceKey";

        protected INavigationService NavigationService { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        protected ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }
    }

    public abstract class ViewModelBase<TParameer> : ViewModelBase
    {
        protected ViewModelBase(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            if (parameters != null && parameters.ContainsKey(ParameterKey))
            {
                var parameter = (TParameer)parameters[ParameterKey];

                Prepare(parameter);
            }
        }

        public abstract void Prepare(TParameer parameer);
    }

    public abstract class ViewModelBaseResult<TResult> : ViewModelBase
    {
        private TaskCompletionSource<TResult> _tcs;

        protected ViewModelBaseResult(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            if (parameters != null && parameters.ContainsKey(TaskCompletionSourceKey))
            {
                _tcs = (TaskCompletionSource<TResult>)parameters[TaskCompletionSourceKey];
            }
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            if (_tcs == null || parameters.GetNavigationMode() == NavigationMode.New)
                return;

            if (parameters != null && parameters.ContainsKey(ParameterKey))
            {
                var parameter = (TResult)parameters[ParameterKey];

                _tcs.TrySetResult(parameter);
            }
            else
            {
                _tcs.TrySetResult(default(TResult));
            }
        }

        public override void Destroy()
        {
            base.Destroy();

            _tcs?.TrySetResult(default(TResult));
        }
    }

    public abstract class ViewModelBase<TParameer, TResult> : ViewModelBaseResult<TResult>
    {
        protected ViewModelBase(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            var parameter = (TParameer)parameters[ParameterKey];

            Prepare(parameter);
        }

        public abstract void Prepare(TParameer parameer);
    }
}
