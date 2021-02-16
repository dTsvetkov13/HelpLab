using System;

namespace Microservices.Interfaces
{
    public interface IMessenger
    {
        void Publish<T>(T message, string route);
        void Subscribe();
    }
}