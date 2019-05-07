using System;
using Foundation;

namespace Plugin.FirebaseAuth
{
    internal static class ActionCodeSettingsExtensions
    {
        public static Firebase.Auth.ActionCodeSettings ToNative(this ActionCodeSettings self)
        {
            if (self == null) return null;

            var actionCodeSettings = new Firebase.Auth.ActionCodeSettings();

            if (self.IsUrlChanged)
            {
                actionCodeSettings.Url = self.Url != null ? new NSUrl(self.Url) : null;
            }
            if (self.IsIosBundleIdChanged)
            {
                actionCodeSettings.IOSBundleId = self.IosBundleId;
            }
            if (self.IsAndroidPackageChanged)
            {
                actionCodeSettings.SetAndroidPackageName(self.AndroidPackageName,
                                                         self.AndroidInstallIfNotAvailable,
                                                         self.AndroidMinimumVersion);
            }
            if (self.IsHandleCodeInAppChanged)
            {
                actionCodeSettings.HandleCodeInApp = self.HandleCodeInApp;
            }

            return actionCodeSettings;
        }
    }
}
