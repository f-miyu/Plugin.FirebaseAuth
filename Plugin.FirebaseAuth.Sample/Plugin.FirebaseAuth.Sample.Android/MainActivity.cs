using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;
using Android.Content;
using System;
using Plugin.CurrentActivity;
using Plugin.FirebaseAuth.Sample.Droid.Services;
using Plugin.FirebaseAuth.Sample.Services;

namespace Plugin.FirebaseAuth.Sample.Droid
{
    [Activity(Name = "com.plugin.firebaseauth.sample.MainActivity", Label = "Plugin.FirebaseAuth.Sample", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private FacebookService _facebookService;
        private GoogleService _googleService;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            _facebookService = new FacebookService(this);
            _googleService = new GoogleService(this);

            CrossCurrentActivity.Current.Init(this, bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer(this)));
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            _facebookService.OnActivetyResult(requestCode, resultCode, data);
            _googleService.OnActivetyResult(requestCode, resultCode, data);
        }

        public class AndroidInitializer : IPlatformInitializer
        {
            private readonly MainActivity _mainActivity;

            public AndroidInitializer(MainActivity mainActivity)
            {
                _mainActivity = mainActivity;
            }

            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
                containerRegistry.RegisterInstance<IFacebookService>(_mainActivity._facebookService);
                containerRegistry.RegisterInstance<IGoogleService>(_mainActivity._googleService);
                containerRegistry.RegisterSingleton<IAppleService, AppleService>();
            }
        }
    }
}

