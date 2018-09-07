using System;
namespace Plugin.FirebaseAuth
{
    public partial class ActionCodeSettings
    {
        public string AndroidPackageName { get; private set; }
        public bool AndroidInstallIfNotAvailable { get; private set; }
        public string AndroidMinimumVersion { get; private set; }

        private bool _handleCodeInApp;
        public bool HandleCodeInApp
        {
            get => _handleCodeInApp;
            set
            {
                _handleCodeInApp = value;
                IsHandleCodeInAppChanged = true;
            }
        }

        private string _iosBundleId;
        public string IosBundleId
        {
            get => _iosBundleId;
            set
            {
                _iosBundleId = value;
                IsIosBundleIdChanged = true;
            }
        }

        private string _url;
        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                IsUrlChanged = true;
            }
        }

        internal bool IsAndroidPackageChanged { get; private set; }
        internal bool IsHandleCodeInAppChanged { get; private set; }
        internal bool IsIosBundleIdChanged { get; private set; }
        internal bool IsUrlChanged { get; private set; }

        public void SetAndroidPackageName(string androidPackageName, bool installIfNotAvailable, string minimumVersion)
        {
            AndroidPackageName = androidPackageName;
            AndroidInstallIfNotAvailable = installIfNotAvailable;
            AndroidMinimumVersion = minimumVersion;
            IsAndroidPackageChanged = true;
        }
    }
}
