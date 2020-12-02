using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class ActionCodeInfoWrapper : IActionCodeInfo, IEquatable<ActionCodeInfoWrapper>
    {
        private readonly ActionCodeInfo _actionCodeInfo;

        public ActionCodeInfoWrapper(ActionCodeInfo actionCodeInfo)
        {
            _actionCodeInfo = actionCodeInfo ?? throw new ArgumentNullException(nameof(actionCodeInfo));
        }

        public ActionCodeOperation Operation => _actionCodeInfo.Operation switch
        {
            Firebase.Auth.ActionCodeOperation.Unknown => ActionCodeOperation.Unkonwn,
            Firebase.Auth.ActionCodeOperation.PasswordReset => ActionCodeOperation.PasswordReset,
            Firebase.Auth.ActionCodeOperation.VerifyEmail => ActionCodeOperation.VerifyEmail,
            Firebase.Auth.ActionCodeOperation.RecoverEmail => ActionCodeOperation.RecoverEmail,
            Firebase.Auth.ActionCodeOperation.EmailLink => ActionCodeOperation.EmailLink,
            Firebase.Auth.ActionCodeOperation.VerifyAndChangeEmail => ActionCodeOperation.VerifyAndChangeEmail,
            Firebase.Auth.ActionCodeOperation.RevertSecondFactorAddition => ActionCodeOperation.RevertSecondFactorAddition,
            _ => ActionCodeOperation.Unkonwn,
        };

        public string? Email => _actionCodeInfo.Email;

        public string? PreviousEmail => _actionCodeInfo.PreviousEmail;

        public override bool Equals(object? obj)
        {
            return Equals(obj as ActionCodeInfoWrapper);
        }

        public bool Equals(ActionCodeInfoWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_actionCodeInfo, other._actionCodeInfo)) return true;
            return _actionCodeInfo.Equals(other._actionCodeInfo);
        }

        public override int GetHashCode()
        {
            return _actionCodeInfo.GetHashCode();
        }
    }
}
