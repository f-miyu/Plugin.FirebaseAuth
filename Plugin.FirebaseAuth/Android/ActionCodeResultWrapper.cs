using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class ActionCodeResultWrapper : IActionCodeInfo, IEquatable<ActionCodeResultWrapper>
    {
        private readonly IActionCodeResult _actionCodeResult;

        public ActionCodeResultWrapper(IActionCodeResult actionCodeResult)
        {
            _actionCodeResult = actionCodeResult ?? throw new ArgumentNullException(nameof(actionCodeResult));
        }

        public ActionCodeOperation Operation => _actionCodeResult.Operation switch
        {
            ActionCodeResult.PasswordReset => ActionCodeOperation.PasswordReset,
            ActionCodeResult.VerifyEmail => ActionCodeOperation.VerifyEmail,
            ActionCodeResult.RecoverEmail => ActionCodeOperation.RecoverEmail,
            ActionCodeResult.Error => ActionCodeOperation.Unkonwn,
            ActionCodeResult.SignInWithEmailLink => ActionCodeOperation.EmailLink,
            ActionCodeResult.VerifyBeforeChangeEmail => ActionCodeOperation.VerifyAndChangeEmail,
            ActionCodeResult.RevertSecondFactorAddition => ActionCodeOperation.RevertSecondFactorAddition,
            _ => ActionCodeOperation.Unkonwn,
        };

        public string? Email => _actionCodeResult.Info?.Email;

        public string? PreviousEmail => _actionCodeResult.Info switch
        {
            ActionCodeEmailInfo actionCodeEmailInfo => actionCodeEmailInfo.PreviousEmail,
            _ => null
        };

        public override bool Equals(object? obj)
        {
            return Equals(obj as ActionCodeResultWrapper);
        }

        public bool Equals(ActionCodeResultWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_actionCodeResult, other._actionCodeResult)) return true;
            return _actionCodeResult.Equals(other._actionCodeResult);
        }

        public override int GetHashCode()
        {
            return _actionCodeResult.GetHashCode();
        }
    }
}
