using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Foundation;

namespace Plugin.FirebaseAuth
{
    public class GameCenterAuthProviderWrapper : IGameCenterAuthProvider
    {
        public async Task<IAuthCredential> GetCredentialAsync()
        {
            try
            {
                var credential = await GameCenterAuthProvider.GetCredentialAsync().ConfigureAwait(false);
                return new AuthCredentialWrapper(credential);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }
    }
}
