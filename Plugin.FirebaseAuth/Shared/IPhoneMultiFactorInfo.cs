using System;
namespace Plugin.FirebaseAuth
{
    public partial interface IPhoneMultiFactorInfo : IMultiFactorInfo
    {
        string PhoneNumber { get; }
    }
}
