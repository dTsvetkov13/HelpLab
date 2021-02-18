using EasyNetQ;
using Microservices.EventBus.Interfaces;
using System;

namespace Microservices.EventBus
{
    public class AmqpEventBus : Interfaces.IEventBus
    {
        private readonly IBus Bus;
        public AmqpEventBus()
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