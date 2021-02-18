using EasyNetQ;
using Microservices.EventBus.Interfaces;
using System;

namespace Microservices.EventBus
{
    public class EventBus : IEventuBus
    {
        private readonly IBus Bus;
        public EventBus()
        {
            Bus = RabbitHutch.CreateBus("host=localhost");
        }
        public void Publish<T>(T message, string route)
        {
            Bus.PubSub.PublishAsync(message, route);
        }

        public void Subscribe<ReturnType>(string id, Action<ReturnType> handle)
        {
            Bus.PubSub.SubscribeAsync<ReturnType>(id, handle);
        }

        public void Subscribe<ReturnType>(string id, Action<ReturnType> handle, Action<ISubscriptionConfiguration> action)
        {
            Bus.PubSub.SubscribeAsync<ReturnType>(id, handle, action);
        }
    }
}