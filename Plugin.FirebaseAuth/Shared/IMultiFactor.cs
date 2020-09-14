using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.FirebaseAuth
{
    public interface IMultiFactor
    {
        IEnumerable<IMultiFactorInfo> EnrolledFactors { get; }
        Task<IMultiFactorSession> GetSessionAsync();
        Task EnrollAsync(IMultiFactorAssertion multiFactorAssertion, string? displayName);
        Task UnenrollAsync(IMultiFactorInfo multiFactorInfo);
        Task UnenrollAsync(string factorUid);
    }
}
