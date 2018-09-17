using System;
using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;
using Plugin.FirebaseAuth.Sample.Auth;

namespace Plugin.FirebaseAuth.Sample.Droid
{
    [Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataSchemes = new[] { Constants.FacebookUrlScheme, Constants.UrlScheme },
        DataHost = "authorize")]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = Constants.GoogleAndroidUrlScheme,
        DataPath = "/oauth2redirect")]
    public class CustomUrlSchemeInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var uri = new Uri(Intent.Data.ToString());

            AuthenticationState.Authenticator.OnPageLoaded(uri);

            var intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
            StartActivity(intent);

            Finish();
        }
    }
}
