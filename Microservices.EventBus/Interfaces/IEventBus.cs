using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.EventBus.Interfaces
{
    public interface IEventBus
    {
        void Publish<T>(T message, string route);

        void Subscribe<ReturnType>(string id, Action<ReturnType> handle);

        void Subscribe<ReturnType>(string id, Action<ReturnType> handle, Action<ISubscriptionConfiguration> action);
    }
}
