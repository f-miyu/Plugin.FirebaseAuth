using System;
using Android.App;
using Android.OS;

namespace Plugin.FirebaseAuth
{
    public static class FirebaseAuth
    {
        private static Func<Activity> _topActivityFactory;

        internal static Activity CurrentTopActivity => _topActivityFactory?.Invoke();

        public static long VerifyingPhoneNumberTimeout { get; set; } = 60;

        public static void Init(Func<Activity> topActivityFactory)
        {
            _topActivityFactory = topActivityFactory;
        }

        public static void Init(Application application)
        {
            var callbacks = new ActivityLifecycleCallbacks();
            application.RegisterActivityLifecycleCallbacks(callbacks);
            _topActivityFactory = () => callbacks.CurrentTopActivit;
        }

        public static void Init(Activity activity)
        {
            var callbacks = new ActivityLifecycleCallbacks(activity);
            activity.Application.RegisterActivityLifecycleCallbacks(callbacks);
            _topActivityFactory = () => callbacks.CurrentTopActivit;
        }

        private class ActivityLifecycleCallbacks : Java.Lang.Object, Application.IActivityLifecycleCallbacks
        {
            public Activity CurrentTopActivit { get; private set; }

            public ActivityLifecycleCallbacks()
            {
            }

            public ActivityLifecycleCallbacks(Activity activity)
            {
                CurrentTopActivit = activity;
            }

            public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
            {
                CurrentTopActivit = activity;
            }

            public void OnActivityDestroyed(Activity activity)
            {
            }

            public void OnActivityPaused(Activity activity)
            {
            }

            public void OnActivityResumed(Activity activity)
            {
                CurrentTopActivit = activity;
            }

            public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
            {
            }

            public void OnActivityStarted(Activity activity)
            {
            }

            public void OnActivityStopped(Activity activity)
            {
            }
        }
    }
}
