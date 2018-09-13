using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;
using Android.Widget;
using Android.Content;
using System;
using Plugin.FirebaseAuth.Sample.Auth;
using Xamarin.Auth;

namespace Plugin.FirebaseAuth.Sample.Droid
{
    [Activity(Label = "Plugin.FirebaseAuth.Sample", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true,
              LaunchMode = LaunchMode.SingleTop,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataSchemes = new[] { Constants.GoogleAndroidUrlScheme, Constants.FacebookUrlScheme, Constants.UrlScheme },
        DataPaths = new[] { "/oauth2redirect" })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            FirebaseAuth.Init(this);

            Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);
            CustomTabsConfiguration.CustomTabsClosingMessage = null;

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            if (intent.Data != null)
            {
                var uri = new Uri(intent.Data.ToString());
                AuthenticationState.Authenticator.OnPageLoaded(uri);
            }
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

