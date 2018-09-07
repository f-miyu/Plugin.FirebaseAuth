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

        private Uri _phtoUrl;
        public Uri PhtoUrl
        {
            get => _phtoUrl;
            set
            {
                _phtoUrl = value;
                IsPhtoUrlChanged = true;
            }
        }

        internal bool IsDisplayNameChanged { get; private set; }
        internal bool IsPhtoUrlChanged { get; private set; }
    }
}
