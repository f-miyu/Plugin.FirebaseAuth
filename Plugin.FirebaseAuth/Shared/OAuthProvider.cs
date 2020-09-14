using System;
using System.Collections;
using System.Collections.Generic;

namespace Plugin.FirebaseAuth
{
    public partial class OAuthProvider : IFederatedAuthProvider
    {
        private readonly string _providerId;
        private readonly IAuth? _auth;

        public OAuthProvider(string providerId)
        {
            _providerId = providerId;
        }

        public OAuthProvider(string providerId, IAuth auth)
        {
            _providerId = providerId;
            _auth = auth;
        }

        public IDictionary<string, string>? CustomParameters { get; set; }

        public IEnumerable<string>? Scopes { get; set; }
    }
}
