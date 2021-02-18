using Microservices.Models.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Users.Services
{
    public class MessageHandler : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceProvider;
        public MessageHandler(IServiceScopeFactory scopeFactory)
        {
            _serviceProvider = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            EventBus.EventBus messenger = new EventBus.EventBus();

            messenger.Subscribe<EventMessage>(Guid.NewGuid().ToString(), ReceiveEventMessage, x => x.WithTopic("posts.*"));

            return Task.CompletedTask;
        }

        private void ReceiveEventMessage(EventMessage eventMessage)
        {
            switch (eventMessage.Event)
            {
                case EventsEnum.PostAdded:
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetService<IUserService>();
                        service.IncreasePostsCount(eventMessage.UserId);
                    }
                    break;
                }
                default:
                    break;
            }
        }
    }
}
