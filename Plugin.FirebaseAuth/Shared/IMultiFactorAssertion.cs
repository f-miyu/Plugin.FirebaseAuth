using System;
namespace Plugin.FirebaseAuth
{
    public partial interface IMultiFactorAssertion
    {
        string FactorId { get; }
    }
}
