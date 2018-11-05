using System;
using Android.App;
using Android.OS;
using Firebase.Auth;
using Android.Content;

namespace Plugin.FirebaseAuth
{
    public static class FirebaseAuth
    {
        public static readonly string DefaultAppName = "[FirebasePlugin]";

        private static Func<Activity> _currentActivityFactory;
        private static ActivityLifecycleCallbacks _callbacks;

        internal static Activity CurrentActivity => _currentActivityFactory?.Invoke();

        public static long VerifyingPhoneNumberTimeout { get; set; } = 60;

        public static void Init(Context context, Func<Activity> currentActivityFactory)
        {
            _currentActivityFactory = currentActivityFactory;

            try
            {
                Firebase.FirebaseApp.GetInstance(DefaultAppName);
            }
            catch (Exception)
            {
                var baseOptions = Firebase.FirebaseOptions.FromResource(context);
                var options = new Firebase.FirebaseOptions.Builder(baseOptions).SetProjectId(baseOptions.StorageBucket.Split('.')[0]).Build();

                Firebase.FirebaseApp.InitializeApp(context, options, DefaultAppName);
            }
        }

        public static void Init(Application application)
        {
            if (_callbacks != null)
                return;

            _callbacks = new ActivityLifecycleCallbacks();
            application.RegisterActivityLifecycleCallbacks(_callbacks);

            Init(application, () => _callbacks.CurrentActivit);
        }

        public static void Init(Activity activity)
        {
            if (_callbacks != null)
                return;

            _callbacks = new ActivityLifecycleCallbacks(activity);
            activity.Application.RegisterActivityLifecycleCallbacks(_callbacks);

            Init(activity, () => _callbacks.CurrentActivit);
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
