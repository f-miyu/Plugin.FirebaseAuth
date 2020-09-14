using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class UserMetadataWrapper : IUserMetadata, IEquatable<UserMetadataWrapper>
    {
        private readonly UserMetadata _userMetadata;

        public UserMetadataWrapper(UserMetadata userMetadata)
        {
            _userMetadata = userMetadata ?? throw new ArgumentNullException(nameof(userMetadata));
        }

        public DateTimeOffset CreationDate => new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(_userMetadata.CreationDate.SecondsSinceReferenceDate);

        public DateTimeOffset LastSignInDate => new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(_userMetadata.LastSignInDate.SecondsSinceReferenceDate);

        public override bool Equals(object? obj)
        {
            return Equals(obj as UserMetadataWrapper);
        }

        public bool Equals(UserMetadataWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_userMetadata, other._userMetadata)) return true;
            return _userMetadata.Equals(other._userMetadata);
        }

        public override int GetHashCode()
        {
            return _userMetadata.GetHashCode();
        }
    }
}
