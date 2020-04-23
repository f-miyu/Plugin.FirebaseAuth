using System;
namespace Plugin.FirebaseAuth
{
    public class UserProfileChangeRequest
    {
        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                IsDisplayNameChanged = true;
            }
        }

        private Uri _photoUrl;
        public Uri PhotoUrl
        {
            get => _photoUrl;
            set
            {
                _photoUrl = value;
                IsPhotoUrlChanged = true;
            }
        }

        internal bool IsDisplayNameChanged { get; private set; }
        internal bool IsPhotoUrlChanged { get; private set; }
    }
}
