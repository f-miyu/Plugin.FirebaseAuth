using System;
using System.Threading.Tasks;

namespace Plugin.FirebaseAuth
{
    public interface IGameCenterAuthProvider
    {
        Task<IAuthCredential> GetCredentialAsync();
    }
}
