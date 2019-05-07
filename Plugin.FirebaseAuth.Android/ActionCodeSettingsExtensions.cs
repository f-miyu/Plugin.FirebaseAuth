using System;
namespace Plugin.FirebaseAuth
{
    internal static class ActionCodeSettingsExtensions
    {
        public static Firebase.Auth.ActionCodeSettings ToNative(this ActionCodeSettings self)
        {
            if (self == null) return null;

            var builder = Firebase.Auth.ActionCodeSettings.NewBuilder();

            if (self.IsUrlChanged)
            {
                builder.SetUrl(self.Url);
            }
            if (self.IsIosBundleIdChanged)
            {
                builder.SetIOSBundleId(self.IosBundleId);
            }
            if (self.IsAndroidPackageChanged)
            {
                builder.SetAndroidPackageName(self.AndroidPackageName,
                                              self.AndroidInstallIfNotAvailable,
                                              self.AndroidMinimumVersion);
            }
            if (self.IsHandleCodeInAppChanged)
            {
                builder.SetHandleCodeInApp(self.HandleCodeInApp);
            }

            return builder.Build();
        }
    }
}
