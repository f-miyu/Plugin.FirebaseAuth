using System;
using System.Threading.Tasks;

namespace Plugin.FirebaseAuth.Sample.Services
{
    public interface IGoogleService
    {
        Task<(string IdToken, string AccessToken)> GetCredentialAsync();
    }
}
