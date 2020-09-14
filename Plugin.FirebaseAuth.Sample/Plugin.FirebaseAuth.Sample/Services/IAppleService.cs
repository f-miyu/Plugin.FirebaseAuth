using System;
using System.Threading.Tasks;

namespace Plugin.FirebaseAuth.Sample.Services
{
    public interface IAppleService
    {
        Task<(string IdToken, string RawNonce)> GetCredentialAsync();
    }
}
