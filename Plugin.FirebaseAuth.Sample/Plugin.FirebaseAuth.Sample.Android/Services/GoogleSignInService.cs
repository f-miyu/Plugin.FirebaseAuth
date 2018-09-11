using System;
using System.Threading.Tasks;
using Plugin.FirebaseAuth.Sample.Services;
using Android.Gms.Auth.Api.SignIn;
namespace Plugin.FirebaseAuth.Sample.Droid.Services
{
    public class GoogleSignInService : IGoogleSignInService
    {
        public GoogleSignInService()
        {
        }

        public Task<(string IdToken, string AccessToken)> SignIn()
        {
            var gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                                             .RequestIdToken("")
                                             .Build();

            throw new NotImplementedException();
        }
    }
}
