using System;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.FirebaseAuth.Sample.Services
{
    public class MultiFactorService
    {
        private readonly IVerificationCodeGettable _verificationCodeGetter;

        public MultiFactorService(IVerificationCodeGettable verificationCodeGetter)
        {
            _verificationCodeGetter = verificationCodeGetter;
        }

        public async Task<IAuthResult> ResolveAsync(IMultiFactorResolver multiFactorResolver)
        {
            var hint = multiFactorResolver.Hints.First() as IPhoneMultiFactorInfo;

            var verificationResult = await CrossFirebaseAuth.Current.PhoneAuthProvider.VerifyPhoneNumberAsync(hint, multiFactorResolver.Session);

            var credential = verificationResult.Credential;

            if (credential == null)
            {
                var verificationCode = await _verificationCodeGetter.GetVerificationCodeAsync();

                if (verificationCode != null)
                {
                    credential = CrossFirebaseAuth.Current.PhoneAuthProvider
                        .GetCredential(verificationResult.VerificationId, verificationCode);
                }
                else
                {
                    throw new OperationCanceledException();
                }
            }

            var assertion = CrossFirebaseAuth.Current.PhoneMultiFactorGenerator.GetAssertion(credential);

            return await multiFactorResolver.ResolveSignInAsync(assertion);
        }
    }
}
