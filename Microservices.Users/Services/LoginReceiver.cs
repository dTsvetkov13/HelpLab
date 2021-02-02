using IdentityModel.Client;
using Microservices.Models;
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
    public class LoginReceiver : Receiver
    {
        private readonly IServiceScopeFactory _serviceProvider;
        public LoginReceiver(IServiceScopeFactory serviceScopeFactory) : base()
        {
            _serviceProvider = serviceScopeFactory;
            QueueName = "users.login";
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                Console.WriteLine(content);

                LoginUser input = JsonConvert.DeserializeObject<LoginUser>(content);

                Console.WriteLine("Received: " + input);

                TokenResponse result = new TokenResponse();

                var props = ea.BasicProperties;
                var replyProps = _channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetService<IUserService>();
                        result = await service.Authenticate(input.Username, input.Password);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(" [.] " + e.Message);
                }
                finally
                {
                    LoginResponse loginResponse = new LoginResponse { 
                        AccessToken = result.AccessToken,
                        Error = result.Error,
                        ErrorDescription = result.ErrorDescription,
                        ExpiresIn = result.ExpiresIn,
                        IsError = result.IsError,
                        Scope = result.Scope,
                        TokenType = result.TokenType,
                        IdentityToken = result.IdentityToken,
                        IssuedTokenType = result.IssuedTokenType,
                        RefreshToken = result.RefreshToken
                    };

                    var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(loginResponse));

                    var deserialized = JsonConvert.DeserializeObject<LoginResponse>(Encoding.UTF8.GetString(responseBytes));

                    Console.WriteLine("Is error: " + deserialized.IsError + " " + deserialized.Error + " " + deserialized.ErrorDescription);
                    Console.WriteLine("Token: " + deserialized.AccessToken);

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
