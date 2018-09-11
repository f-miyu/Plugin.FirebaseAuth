using System;
using System.Threading.Tasks;
namespace Plugin.FirebaseAuth.Sample.Services
{
    public interface IGoogleSignInService
    {
        Task<(string IdToken, string AccessToken)> SignIn();
    }
}
