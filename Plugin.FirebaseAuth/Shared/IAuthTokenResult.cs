using System;
using System.Collections.Generic;

namespace Plugin.FirebaseAuth
{
    public interface IAuthTokenResult
    {
        string? Token { get; }
        DateTimeOffset ExpirationDate { get; }
        DateTimeOffset AuthDate { get; }
        DateTimeOffset IssuedAtDate { get; }
        string? SignInProvider { get; }
        string? SignInSecondFactor { get; }
        IDictionary<string, object?> Claims { get; }
    }
}
