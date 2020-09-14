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

        public IPhoneAuthCredential GetCredential(string verificationId, string verificationCode)
        {
            var credential = PhoneAuthProvider.GetCredential(verificationId, verificationCode);
            return new PhoneAuthCredentialWrapper(credential);
        }

        public IPhoneAuthCredential GetCredential(IAuth auth, string verificationId, string verificationCode)
        {
            return GetCredential(verificationId, verificationCode);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(string phoneNumber)
        {
            return VerifyPhoneNumberAsync(Firebase.Auth.FirebaseAuth.Instance, phoneNumber, TimeSpan.FromSeconds(30));
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(string phoneNumber, TimeSpan timeout)
        {
            return VerifyPhoneNumberAsync(Firebase.Auth.FirebaseAuth.Instance, phoneNumber, timeout);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, string phoneNumber)
        {
            return VerifyPhoneNumberAsync(auth.ToNative(), phoneNumber, TimeSpan.FromSeconds(30));
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, string phoneNumber, TimeSpan timeout)
        {
            return VerifyPhoneNumberAsync(auth.ToNative(), phoneNumber, timeout);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(string phoneNumber, IMultiFactorSession multiFactorSession)
        {
            return VerifyPhoneNumberAsync(Firebase.Auth.FirebaseAuth.Instance, phoneNumber, multiFactorSession, TimeSpan.FromSeconds(30));
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(string phoneNumber, IMultiFactorSession multiFactorSession, TimeSpan timeout, bool requiresSmsValidation)
        {
            return VerifyPhoneNumberAsync(Firebase.Auth.FirebaseAuth.Instance, phoneNumber, multiFactorSession, timeout, requiresSmsValidation);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IPhoneMultiFactorInfo phoneMultiFactorInfo, IMultiFactorSession multiFactorSession)
        {
            return VerifyPhoneNumberAsync(Firebase.Auth.FirebaseAuth.Instance, phoneMultiFactorInfo, multiFactorSession, TimeSpan.FromSeconds(30));
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IPhoneMultiFactorInfo phoneMultiFactorInfo, IMultiFactorSession multiFactorSession, TimeSpan timeout, bool requiresSmsValidation)
        {
            return VerifyPhoneNumberAsync(Firebase.Auth.FirebaseAuth.Instance, phoneMultiFactorInfo, multiFactorSession, timeout, requiresSmsValidation);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, string phoneNumber, IMultiFactorSession multiFactorSession)
        {
            return VerifyPhoneNumberAsync(auth.ToNative(), phoneNumber, multiFactorSession, TimeSpan.FromSeconds(30));
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, string phoneNumber, IMultiFactorSession multiFactorSession, TimeSpan timeout, bool requiresSmsValidation)
        {
            return VerifyPhoneNumberAsync(auth.ToNative(), phoneNumber, multiFactorSession, timeout, requiresSmsValidation);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, IPhoneMultiFactorInfo phoneMultiFactorInfo, IMultiFactorSession multiFactorSession)
        {
            return VerifyPhoneNumberAsync(auth.ToNative(), phoneMultiFactorInfo, multiFactorSession, TimeSpan.FromSeconds(30));
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, IPhoneMultiFactorInfo phoneMultiFactorInfo, IMultiFactorSession multiFactorSession, TimeSpan timeout, bool requiresSmsValidation)
        {
            return VerifyPhoneNumberAsync(auth.ToNative(), phoneMultiFactorInfo, multiFactorSession, timeout, requiresSmsValidation);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberForTestingAsync(string phoneNumber, string verificationCode)
        {
            return VerifyPhoneNumberForTestingAsync(Firebase.Auth.FirebaseAuth.Instance, phoneNumber, verificationCode, TimeSpan.FromSeconds(30));
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberForTestingAsync(string phoneNumber, string verificationCode, TimeSpan timeout)
        {
            return VerifyPhoneNumberForTestingAsync(Firebase.Auth.FirebaseAuth.Instance, phoneNumber, verificationCode, timeout);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberForTestingAsync(IAuth auth, string phoneNumber, string verificationCode)
        {
            return VerifyPhoneNumberForTestingAsync(auth.ToNative(), phoneNumber, verificationCode, TimeSpan.FromSeconds(30));
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberForTestingAsync(IAuth auth, string phoneNumber, string verificationCode, TimeSpan timeout)
        {
            return VerifyPhoneNumberForTestingAsync(auth.ToNative(), phoneNumber, verificationCode, timeout);
        }

        private Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(Firebase.Auth.FirebaseAuth auth, string phoneNumber, TimeSpan timeout)
        {
            var activity = CrossCurrentActivity.Current.Activity ?? throw new NullReferenceException("current activity is null");

            var tcs = new TaskCompletionSource<PhoneNumberVerificationResult>();
            var callbacks = new Callbacks(tcs);

            auth.FirebaseAuthSettings.SetAutoRetrievedSmsCodeForPhoneNumber(null, null);

            PhoneAuthProvider.GetInstance(auth).VerifyPhoneNumber(phoneNumber, (long)timeout.TotalMilliseconds, TimeUnit.Milliseconds, activity, callbacks);

            return tcs.Task;
        }

        private Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(Firebase.Auth.FirebaseAuth auth, string phoneNumber, IMultiFactorSession multiFactorSession, TimeSpan timeout, bool? requiresSmsValidation = null)
        {
            var activity = CrossCurrentActivity.Current.Activity ?? throw new NullReferenceException("current activity is null");

            var tcs = new TaskCompletionSource<PhoneNumberVerificationResult>();
            var callbacks = new Callbacks(tcs);

            var builder = PhoneAuthOptions.NewBuilder(auth)
                .SetActivity(activity)
                .SetCallbacks(callbacks)
                .SetPhoneNumber(phoneNumber)
                .SetMultiFactorSession(multiFactorSession.ToNative())
                .SetTimeout(new Java.Lang.Long((long)timeout.TotalMilliseconds), TimeUnit.Milliseconds);

            if (requiresSmsValidation.HasValue)
            {
                builder.RequireSmsValidation(requiresSmsValidation.Value);
            }

            auth.FirebaseAuthSettings.SetAutoRetrievedSmsCodeForPhoneNumber(null, null);

            PhoneAuthProvider.VerifyPhoneNumber(builder.Build());

            return tcs.Task;
        }

        private Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(Firebase.Auth.FirebaseAuth auth, IPhoneMultiFactorInfo phoneMultiFactorInfo, IMultiFactorSession multiFactorSession, TimeSpan timeout, bool? requiresSmsValidation = null)
        {
            var activity = CrossCurrentActivity.Current.Activity ?? throw new NullReferenceException("current activity is null");

            var tcs = new TaskCompletionSource<PhoneNumberVerificationResult>();
            var callbacks = new Callbacks(tcs);

            var builder = PhoneAuthOptions.NewBuilder(auth)
                .SetActivity(activity)
                .SetCallbacks(callbacks)
                .SetMultiFactorHint(phoneMultiFactorInfo.ToNative())
                .SetMultiFactorSession(multiFactorSession.ToNative())
                .SetTimeout(new Java.Lang.Long((long)timeout.TotalMilliseconds), TimeUnit.Milliseconds);

            if (requiresSmsValidation.HasValue)
            {
                builder.RequireSmsValidation(requiresSmsValidation.Value);
            }

            auth.FirebaseAuthSettings.SetAutoRetrievedSmsCodeForPhoneNumber(null, null);

            PhoneAuthProvider.VerifyPhoneNumber(builder.Build());

            return tcs.Task;
        }

        private Task<PhoneNumberVerificationResult> VerifyPhoneNumberForTestingAsync(Firebase.Auth.FirebaseAuth auth, string phoneNumber, string verificationCode, TimeSpan timeout)
        {
            var activity = CrossCurrentActivity.Current.Activity ?? throw new NullReferenceException("current activity is null");

            var tcs = new TaskCompletionSource<PhoneNumberVerificationResult>();
            var callbacks = new Callbacks(tcs);

            auth.FirebaseAuthSettings.SetAutoRetrievedSmsCodeForPhoneNumber(phoneNumber, verificationCode);

            PhoneAuthProvider.GetInstance(auth).VerifyPhoneNumber(phoneNumber, (long)timeout.TotalMilliseconds, TimeUnit.Milliseconds, activity, callbacks);

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
