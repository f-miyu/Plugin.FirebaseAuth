using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Java.Util.Concurrent;
using Plugin.CurrentActivity;

namespace Plugin.FirebaseAuth
{
    public class PhoneAuthProviderWrapper : IPhoneAuthProvider
    {
        public string ProviderId => PhoneAuthProvider.ProviderId;

        public string PhoneSignInMethod => PhoneAuthProvider.PhoneSignInMethod;

        public IPhoneAuthCredential GetCredential(IAuth auth, string verificationId, string verificationCode)
        {
            var credential = PhoneAuthProvider.GetCredential(verificationId, verificationCode);
            return new PhoneAuthCredentialWrapper(credential);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, string phoneNumber)
        {
            return VerifyPhoneNumberAsync(auth, phoneNumber, TimeSpan.FromSeconds(60));
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, string phoneNumber, TimeSpan timeout)
        {
            var activity = CrossCurrentActivity.Current.Activity ?? throw new NullReferenceException("current activity is null");

            var tcs = new TaskCompletionSource<PhoneNumberVerificationResult>();
            var callbacks = new Callbacks(tcs);

            var wrapper = (AuthWrapper)auth;
            var firebaseAuth = (Firebase.Auth.FirebaseAuth)wrapper;
            firebaseAuth.FirebaseAuthSettings.SetAutoRetrievedSmsCodeForPhoneNumber(null, null);

            PhoneAuthProvider.GetInstance(firebaseAuth).VerifyPhoneNumber(phoneNumber, (long)timeout.TotalMilliseconds, TimeUnit.Milliseconds, activity, callbacks);

            return tcs.Task;
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberForTestingAsync(IAuth auth, string phoneNumber, string verificationCode)
        {
            return VerifyPhoneNumberForTestingAsync(auth, phoneNumber, verificationCode, TimeSpan.FromSeconds(60));
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberForTestingAsync(IAuth auth, string phoneNumber, string verificationCode, TimeSpan timeout)
        {
            var activity = CrossCurrentActivity.Current.Activity ?? throw new NullReferenceException("current activity is null");

            var tcs = new TaskCompletionSource<PhoneNumberVerificationResult>();
            var callbacks = new Callbacks(tcs);

            var wrapper = (AuthWrapper)auth;
            var firebaseAuth = (Firebase.Auth.FirebaseAuth)wrapper;
            firebaseAuth.FirebaseAuthSettings.SetAutoRetrievedSmsCodeForPhoneNumber(phoneNumber, verificationCode);

            PhoneAuthProvider.GetInstance(firebaseAuth).VerifyPhoneNumber(phoneNumber, (long)timeout.TotalMilliseconds, TimeUnit.Milliseconds, activity, callbacks);

            return tcs.Task;
        }

        private class Callbacks : PhoneAuthProvider.OnVerificationStateChangedCallbacks
        {
            private readonly TaskCompletionSource<PhoneNumberVerificationResult> _tcs;

            public Callbacks(TaskCompletionSource<PhoneNumberVerificationResult> tcs)
            {
                _tcs = tcs;
            }

            public override void OnVerificationCompleted(PhoneAuthCredential credential)
            {
                _tcs.TrySetResult(new PhoneNumberVerificationResult(new PhoneAuthCredentialWrapper(credential), null));
            }

            public override void OnVerificationFailed(FirebaseException exception)
            {
                _tcs.SetException(ExceptionMapper.Map(exception));
            }

            public override void OnCodeSent(string verificationId, PhoneAuthProvider.ForceResendingToken forceResendingToken)
            {
                base.OnCodeSent(verificationId, forceResendingToken);

                _tcs.TrySetResult(new PhoneNumberVerificationResult(null, verificationId));
            }

            public override void OnCodeAutoRetrievalTimeOut(string verificationId)
            {
                base.OnCodeAutoRetrievalTimeOut(verificationId);

                _tcs.TrySetResult(new PhoneNumberVerificationResult(null, verificationId));
            }
        }
    }
}
