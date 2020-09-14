using System;
namespace Plugin.FirebaseAuth
{
    public interface IListenerRegistration : IDisposable
    {
        void Remove();
    }
}
