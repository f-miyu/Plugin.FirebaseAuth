using System;
using Android.App;
using Android.OS;

namespace Plugin.FirebaseAuth
{
    public static class FirebaseAuth
    {
        private static ActivityLifecycleCallbacks _callbacks;

        internal static Activity CurrentActivity => _callbacks.CurrentActivit;

        public static long VerifyingPhoneNumberTimeout { get; set; } = 60;


        public static void Init(Application application)
        {
            if (_callbacks != null)
                return;

            _callbacks = new ActivityLifecycleCallbacks();
            application.RegisterActivityLifecycleCallbacks(_callbacks);
        }

        public static void Init(Activity activity)
        {
            if (_callbacks != null)
                return;

            _callbacks = new ActivityLifecycleCallbacks(activity);
            activity.Application.RegisterActivityLifecycleCallbacks(_callbacks);
        }

        private class ActivityLifecycleCallbacks : Java.Lang.Object, Application.IActivityLifecycleCallbacks
        {
            private WeakReference<Activity> _currentActivity = new WeakReference<Activity>(null);
            public Activity CurrentActivit
            {
                get => _currentActivity.TryGetTarget(out var activity) ? activity : null;
                set => _currentActivity.SetTarget(value);
            }

            public ActivityLifecycleCallbacks()
            {
            }

            public ActivityLifecycleCallbacks(Activity activity)
            {
                CurrentActivit = activity;
            }

            public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
            {
                CurrentActivit = activity;
            }

            public void OnActivityDestroyed(Activity activity)
            {
            }

            public void OnActivityPaused(Activity activity)
            {
            }

            public void OnActivityResumed(Activity activity)
            {
                CurrentActivit = activity;
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
