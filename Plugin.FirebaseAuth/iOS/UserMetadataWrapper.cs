using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class UserMetadataWrapper : IUserMetadata
    {
        private readonly UserMetadata _userMetadata;

        public DateTimeOffset CreationDate => new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(_userMetadata.CreationDate.SecondsSinceReferenceDate);

        public DateTimeOffset LastSignInDate => new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(_userMetadata.LastSignInDate.SecondsSinceReferenceDate);

        public UserMetadataWrapper(UserMetadata userMetadata)
        {
            _userMetadata = userMetadata;
        }
    }
}
