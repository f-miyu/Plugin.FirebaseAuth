using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using Android.Runtime;
using Firebase;
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

        public async Task<IMultiFactorSession> GetSessionAsync()
        {
            try
            {
                var session = await _multiFactor.GetSession().AsAsync<MultiFactorSession>().ConfigureAwait(false);
                return new MultiFactorSessionWrapper(session);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task EnrollAsync(IMultiFactorAssertion multiFactorAssertion, string? displayName)
        {
            try
            {
                await _multiFactor.Enroll(multiFactorAssertion.ToNative(), displayName).AsAsync().ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task UnenrollAsync(IMultiFactorInfo multiFactorInfo)
        {
            try
            {
                await _multiFactor.Unenroll(multiFactorInfo.ToNative()).AsAsync().ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task UnenrollAsync(string factorUid)
        {
            try
            {
                await _multiFactor.Unenroll(factorUid).AsAsync().ConfigureAwait(false);
            }
            catch (FirebaseException e)
            {
                throw ExceptionMapper.Map(e);
            }
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
