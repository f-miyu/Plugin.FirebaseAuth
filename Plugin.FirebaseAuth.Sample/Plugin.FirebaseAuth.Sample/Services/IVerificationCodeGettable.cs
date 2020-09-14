using System;
using System.Threading.Tasks;

namespace Plugin.FirebaseAuth.Sample.Services
{
    public interface IVerificationCodeGettable
    {
        Task<string> GetVerificationCodeAsync();
    }
}
