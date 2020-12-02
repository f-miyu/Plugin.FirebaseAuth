using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class MultiFactorWrapper : IMultiFactor, IEquatable<MultiFactorWrapper>
    {
        private readonly MultiFactor _multiFactor;

        public MultiFactorWrapper(MultiFactor multiFactor)
        {
            _multiFactor = multiFactor ?? throw new ArgumentNullException(nameof(multiFactor));
        }

        public IEnumerable<IMultiFactorInfo> EnrolledFactors =>
            _multiFactor.EnrolledFactors.Select(info => MultiFactorInfoWrapperFactory.Create(info));

        public Task<IMultiFactorSession> GetSessionAsync()
        {
            var tcs = new TaskCompletionSource<IMultiFactorSession>();

            _multiFactor.GetSession((session, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(error));
                }
                else
                {
                    tcs.SetResult(new MultiFactorSessionWrapper(session!));
                }
            });

            return tcs.Task;
        }

        public Task EnrollAsync(IMultiFactorAssertion multiFactorAssertion, string? displayName)
        {
            var tcs = new TaskCompletionSource<bool>();

            _multiFactor.Enroll(multiFactorAssertion.ToNative(), displayName, (error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(error));
                }
                else
                {
                    tcs.SetResult(true);
                }
            });

            return tcs.Task;
        }

        public Task UnenrollAsync(IMultiFactorInfo multiFactorInfo)
        {
            var tcs = new TaskCompletionSource<bool>();

            _multiFactor.Unenroll(multiFactorInfo.ToNative(), (error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(error));
                }
                else
                {
                    tcs.SetResult(true);
                }
            });

            return tcs.Task;
        }

        public Task UnenrollAsync(string factorUid)
        {
            var tcs = new TaskCompletionSource<bool>();

            _multiFactor.Unenroll(factorUid, (error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(error));
                }
                else
                {
                    tcs.SetResult(true);
                }
            });

            return tcs.Task;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MultiFactorWrapper);
        }

        public bool Equals(MultiFactorWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_multiFactor, other._multiFactor)) return true;
            return _multiFactor.Equals(other._multiFactor);
        }

        public override int GetHashCode()
        {
            return _multiFactor.GetHashCode();
        }
    }
}
