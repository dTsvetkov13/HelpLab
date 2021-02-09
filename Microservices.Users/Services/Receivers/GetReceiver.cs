using Microservices.Users.Entities.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Users.Services
{
    public class GetReceiver : Receiver
    {
        private readonly IServiceScopeFactory _serviceProvider;
        public GetReceiver(IServiceScopeFactory serviceScopeFactory) : base()
        {
            _serviceProvider = serviceScopeFactory;
            QueueName = "users.get";
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                Console.WriteLine(content);

                string input = JsonConvert.DeserializeObject<string>(content);

                Console.WriteLine("Received: " + input);

                User result = new User();

                var props = ea.BasicProperties;
                var replyProps = _channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetService<IUserService>();
                        result = await service.Get(input);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e.Message);
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result));

                    _channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                          basicProperties: replyProps, body: responseBytes);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            };

            _channel.BasicConsume(QueueName, false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
