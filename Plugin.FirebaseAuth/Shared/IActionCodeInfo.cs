using System;
namespace Plugin.FirebaseAuth
{
    public interface IActionCodeInfo
    {
        ActionCodeOperation Operation { get; }
        string? Email { get; }
        string? PreviousEmail { get; }
    }
}
