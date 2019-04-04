using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class UserMetadataWrapper : IUserMetadata
    {
        private readonly IFirebaseUserMetadata _userMetadata;

        public DateTimeOffset CreationDate => new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMilliseconds(_userMetadata.CreationTimestamp);

        public DateTimeOffset LastSignInDate => new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMilliseconds(_userMetadata.LastSignInTimestamp);

        public UserMetadataWrapper(IFirebaseUserMetadata userMetadata)
        {
            _userMetadata = userMetadata;
        }
    }
}
