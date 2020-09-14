using System;
using UIKit;

namespace Plugin.FirebaseAuth.Sample.iOS.Services
{
    public static class Utils
    {
        public static UIViewController GetTopViewController()
        {
            var viewController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            while (viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }

            return viewController;
        }
    }
}
