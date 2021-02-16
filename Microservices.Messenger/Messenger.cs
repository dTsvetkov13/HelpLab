using Microservices.Interfaces;
using System;

namespace Microservices.Messenger
{
    public class Messenger : IMessenger
    {
        public void Publish<T>(T message, string route)
        {
            throw new NotImplementedException();
        }

        public void Subscribe()
        {
            throw new NotImplementedException();
        }
    }
}
