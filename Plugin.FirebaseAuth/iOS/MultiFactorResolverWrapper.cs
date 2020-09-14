using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class MultiFactorResolverWrapper : IMultiFactorResolver, IEquatable<MultiFactorResolverWrapper>
    {
        private readonly MultiFactorResolver _multiFactorResolver;

        public MultiFactorResolverWrapper(MultiFactorResolver multiFactorResolver)
        {
            _multiFactorResolver = multiFactorResolver ?? throw new ArgumentNullException(nameof(multiFactorResolver));
        }

        public IMultiFactorSession Session => new MultiFactorSessionWrapper(_multiFactorResolver.Session);

        public IEnumerable<IMultiFactorInfo> Hints => _multiFactorResolver.Hints.Select(info => MultiFactorInfoWrapperFactory.Create(info));

        public IAuth Auth => AuthProvider.GetAuth(_multiFactorResolver.Auth);

        public Task<IAuthResult> ResolveSignInAsync(IMultiFactorAssertion multiFactorAssertion)
        {
            var tcs = new TaskCompletionSource<IAuthResult>();

            _multiFactorResolver.ResolveSignIn(multiFactorAssertion.ToNative(), (authResult, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(error));
                }
                else
                {
                    tcs.SetResult(new AuthResultWrapper(authResult));
                }
            });

            return tcs.Task;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MultiFactorResolverWrapper);
        }

        public bool Equals(MultiFactorResolverWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_multiFactorResolver, other._multiFactorResolver)) return true;
            return _multiFactorResolver.Equals(other._multiFactorResolver);
        }

        public override int GetHashCode()
        {
            return _multiFactorResolver.GetHashCode();
        }
    }
}
