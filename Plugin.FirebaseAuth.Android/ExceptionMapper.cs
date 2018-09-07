using System;
using Firebase;
using System.Security.Cryptography;
namespace Plugin.FirebaseAuth
{
    public static class ExceptionMapper
    {
        public static Exception Map(FirebaseException exception)
        {
            return exception;
        }
    }
}
