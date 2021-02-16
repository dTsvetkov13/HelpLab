using EasyNetQ;
using Microservices.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Messenger
{
    public class Messenger : IMessenger
    {
        public void Publish<T>(T message, string route)
        {
            using(var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                //TODO: decide whether to await this
                bus.PubSub.PublishAsync(message, route);
            }
        }

        public void Subscribe<ReturnType>(string id, Action<ReturnType> handle)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                //TODO: decide whether to await this
                bus.PubSub.SubscribeAsync<ReturnType>(id, handle);
            }
        }
    }
}
