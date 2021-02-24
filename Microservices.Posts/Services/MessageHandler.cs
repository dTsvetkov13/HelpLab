using Microservices.EventBus.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Posts.Services
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
            _eventBus.Subscribe<string>(Guid.NewGuid().ToString(), CatchAnswerAdded, x => x.WithTopic(MessagesEnum.AnswersCreatedRoute));
            _eventBus.Subscribe<string>(Guid.NewGuid().ToString(), CatchAnswerDeleted, x => x.WithTopic(MessagesEnum.AnswersDeletedRoute));

            return Task.CompletedTask;
        }

        private void CatchAnswerAdded(string userId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<PostService>();
                service.IncreaseAnswersCount(userId);
            }
        }

        private void CatchAnswerDeleted(string userId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<PostService>();
                service.DecreaseAnswersCount(userId);
            }
        }
    }
}
