using Microservices.EventBus;
using Microservices.EventBus.Interfaces;
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
        private readonly IEventBus _eventBus;
        public MessageHandler(IServiceScopeFactory scopeFactory,
                              IEventBus eventBus)
        {
            _serviceProvider = scopeFactory;
            _eventBus = eventBus;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _eventBus.Subscribe<string>(Guid.NewGuid().ToString(), CatchPostsAdded, x => x.WithTopic(MessagesEnum.PostsCreatedRoute));
            _eventBus.Subscribe<string>(Guid.NewGuid().ToString(), CatchPostsDeleted, x => x.WithTopic(MessagesEnum.PostsDeletedRoute));

            return Task.CompletedTask;
        }

        private void CatchPostsAdded(string userId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IUserService>();
                service.IncreasePostsCount(userId);
            }
        }

        private void CatchPostsDeleted(string userId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IUserService>();
                service.DecreasePostsCount(userId);
            }
        }
    }
}
