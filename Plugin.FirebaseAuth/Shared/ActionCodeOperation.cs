using System;
namespace Plugin.FirebaseAuth
{
    public enum ActionCodeOperation
    {
        Unkonwn,
        PasswordReset,
        VerifyEmail,
        RecoverEmail,
        EmailLink,
        VerifyAndChangeEmail,
        RevertSecondFactorAddition,
    }
}
