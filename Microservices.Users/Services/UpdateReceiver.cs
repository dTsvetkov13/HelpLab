using Microservices.Models;
using Microsoft.AspNetCore.Identity;
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
    public class UpdateReceiver : Receiver
    {
        private readonly IServiceScopeFactory _serviceProvider;
        public UpdateReceiver(IServiceScopeFactory serviceScopeFactory) : base()
        {
            _serviceProvider = serviceScopeFactory;
            QueueName = "users.update";
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                Console.WriteLine(content);

                UpdateUser input = JsonConvert.DeserializeObject<UpdateUser>(content);

                Console.WriteLine("Received: " + input);

                IEnumerable<IdentityResult> result = new List<IdentityResult>();

                var props = ea.BasicProperties;
                var replyProps = _channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetService<IUserService>();
                        result = await service.Update(input.CurrentUserName,
                                                      input.NewUserName,
                                                      input.CurrentPassword,
                                                      input.CurrentPassword,
                                                      input.CurrentEmail,
                                                      input.NewEmail);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e.Message);
                }
                finally
                {
                    List<UpdateResponse> updateResponses = new List<UpdateResponse>();

                    foreach (var identityResult in result)
                    {
                        updateResponses.Add(new UpdateResponse
                        {
                            Succeeded = identityResult.Succeeded,
                            Errors = identityResult.Errors
                        });
                    }

                    var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(updateResponses));

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
