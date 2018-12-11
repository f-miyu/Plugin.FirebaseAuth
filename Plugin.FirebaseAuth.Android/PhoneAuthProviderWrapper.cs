using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Java.Util.Concurrent;
using Android.App;
using System.Diagnostics;

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
            var activity = FirebaseAuth.CurrentActivity ?? throw new NullReferenceException("current activity is null");

            var tcs = new TaskCompletionSource<PhoneNumberVerificationResult>();
            var callbacks = new Callbacks(tcs);

            var wrapper = (AuthWrapper)auth;
            var firebaseAuth = (Firebase.Auth.FirebaseAuth)wrapper;
            firebaseAuth.FirebaseAuthSettings.SetAutoRetrievedSmsCodeForPhoneNumber(null, null);

            PhoneAuthProvider.GetInstance(firebaseAuth).VerifyPhoneNumber(phoneNumber, FirebaseAuth.VerifyingPhoneNumberTimeout, TimeUnit.Seconds, activity, callbacks);

            return tcs.Task;
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberForTestingAsync(IAuth auth, string phoneNumber, string verificationCode)
        {
            var activity = FirebaseAuth.CurrentActivity ?? throw new NullReferenceException("current activity is null");

            var tcs = new TaskCompletionSource<PhoneNumberVerificationResult>();
            var callbacks = new Callbacks(tcs);

            var wrapper = (AuthWrapper)auth;
            var firebaseAuth = (Firebase.Auth.FirebaseAuth)wrapper;
            firebaseAuth.FirebaseAuthSettings.SetAutoRetrievedSmsCodeForPhoneNumber(phoneNumber, verificationCode);

            PhoneAuthProvider.GetInstance(firebaseAuth).VerifyPhoneNumber(phoneNumber, FirebaseAuth.VerifyingPhoneNumberTimeout, TimeUnit.Seconds, activity, callbacks);

            return tcs.Task;
        }

        private class Callbacks : PhoneAuthProvider.OnVerificationStateChangedCallbacks
        {
            private TaskCompletionSource<PhoneNumberVerificationResult> _tcs;

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
