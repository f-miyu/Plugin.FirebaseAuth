using System;
using System.Threading.Tasks;

namespace Plugin.FirebaseAuth.Sample.Auth
{
    public interface IAuthService
    {
        Task<(string IdToken, string AccessToken)> LoginWithGoogle();
        Task<(string OauthToken, string OatuhTokenSecret)> LoginWithTwitter();
        Task<string> LoginWithFacebook();
        Task<string> LoginWithGitHub();
    }
}
