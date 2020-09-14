using System;
namespace Plugin.FirebaseAuth
{
    public partial interface IMultiFactorInfo
    {
        string Uid { get; }
        string? DisplayName { get; }
        DateTimeOffset EnrollmentDate { get; }
        string FactorId { get; }
    }
}
