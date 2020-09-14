using System;
using System.Threading.Tasks;

namespace Plugin.FirebaseAuth.Sample.Services
{
    public interface IFacebookService
    {
        Task<string> GetCredentialAsync();
    }
}
