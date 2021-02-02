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
    public class RegisterReceiver : Receiver
    {
        private readonly IServiceScopeFactory _serviceProvider;

        public RegisterReceiver(IServiceScopeFactory serviceProvider) : base()
        {
            _serviceProvider = serviceProvider;
            QueueName = "users.register";
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            
            var consumer = new EventingBasicConsumer(_channel);            

            consumer.Received += async (ch, ea) =>
            {
                IdentityResult result = new IdentityResult();

                var body = ea.Body.ToArray();
                var props = ea.BasicProperties;
                var replyProps = _channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var content = Encoding.UTF8.GetString(body);

                    RegisterUser input = JsonConvert.DeserializeObject<RegisterUser>(content);

                    Console.WriteLine("Received: " + input);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetService<IUserService>();

                        result = await service.Create(input.Username, input.Password, input.Email);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e.Message);
                }
                finally
                {
                    Console.WriteLine(result.Succeeded + " " + props.ReplyTo);

                    RegisterResponse registerResponse = new RegisterResponse { Succeeded = result.Succeeded, Errors = result.Errors };

                    var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(registerResponse));
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
