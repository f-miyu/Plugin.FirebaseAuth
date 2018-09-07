using System;
using Foundation;
namespace Plugin.FirebaseAuth
{
    public static class ExceptionMapper
    {
        public static Exception Map(NSErrorException exception)
        {
            return exception;
        }
    }
}
