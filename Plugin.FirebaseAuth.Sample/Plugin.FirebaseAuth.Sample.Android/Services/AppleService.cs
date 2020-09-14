using System;
using System.Threading.Tasks;
using Plugin.FirebaseAuth.Sample.Services;

namespace Plugin.FirebaseAuth.Sample.Droid.Services
{
    public class AppleService : IAppleService
    {
        public Task<(string IdToken, string RawNonce)> GetCredentialAsync()
        {
            return Task.FromResult<(string IdToken, string RawNonce)>((null, null));
        }
    }
}
