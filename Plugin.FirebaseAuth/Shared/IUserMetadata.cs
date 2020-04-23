using System;
namespace Plugin.FirebaseAuth
{
    public interface IUserMetadata
    {
        DateTimeOffset CreationDate { get; }
        DateTimeOffset LastSignInDate { get; }
    }
}