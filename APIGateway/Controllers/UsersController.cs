using Microservices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebMonitoringApi.InputModels;

namespace APIGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IConnection connection;
        private IModel channel;
        public UsersController()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            //TODO: Change IdentityUser with User

            string id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            string replyQueueName;
            EventingBasicConsumer consumer;
            BlockingCollection<IdentityUser> respQueue = new BlockingCollection<IdentityUser>();
            IBasicProperties props;
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(JsonConvert.DeserializeObject<IdentityUser>(response));
                }
            };

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(id));
            channel.BasicPublish(
                exchange: "",
                routingKey: "users.get",
                basicProperties: props,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            var result = respQueue.Take();

            return result != null ? Ok(result) : BadRequest("Invalid input");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel input)
        {
            string replyQueueName;
            EventingBasicConsumer consumer;
            BlockingCollection<RegisterResponse> respQueue = new BlockingCollection<RegisterResponse>();
            IBasicProperties props;
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(JsonConvert.DeserializeObject<RegisterResponse>(response));
                }
            };

            RegisterUser registerUser = new RegisterUser { Name = input.Name, Surname = input.Surname,
                                                           Email = input.Email, Password = input.Password };

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(registerUser));
            channel.BasicPublish(
                exchange: "",
                routingKey: "users.register",
                basicProperties: props,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            var result = respQueue.Take();

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        //Denis1
        //Uw9e?123

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInputModel input)
        {
            string replyQueueName;
            EventingBasicConsumer consumer;
            BlockingCollection<LoginResponse> respQueue = new BlockingCollection<LoginResponse>();
            IBasicProperties props;

            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    var deserialized = JsonConvert.DeserializeObject<LoginResponse>(response);
                    respQueue.Add(deserialized);
                }
            };

            LoginUser registerUser = new LoginUser { Username = input.UserName, Password = input.Password };

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(registerUser));
            channel.BasicPublish(
                exchange: "",
                routingKey: "users.login",
                basicProperties: props,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            var result = respQueue.Take();

            if(!result.IsError)
            {
                return Ok(new
                {
                    result.AccessToken,
                    result.ExpiresIn,
                    result.TokenType
                });
            }
            else
            {
                var errors = new Dictionary<string, string[]>();

                if (result.ErrorDescription != null)
                {
                    errors["ErrDesc"] = new string[] { result.ErrorDescription };
                }
                else
                {
                    errors["Error"] = new string[] { result.Error };
                }

                return BadRequest(new { errors });

                    /*if (result.ErrorDescription != null)
                    {
                        return BadRequest(new
                        {
                            result.ErrorDescription,
                        });
                    }

                    return BadRequest(new
                    {
                        result.Error,
                    });*/
                }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserInputModel input)
        {
            string replyQueueName;
            EventingBasicConsumer consumer;
            BlockingCollection<List<UpdateResponse>> respQueue = new BlockingCollection<List<UpdateResponse>>();
            IBasicProperties props;

            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    var deserialized = JsonConvert.DeserializeObject<List<UpdateResponse>>(response);
                    respQueue.Add(deserialized);
                }
            };

            UpdateUser updateUser = new UpdateUser
            {
                CurrentEmail = input.CurrentEmail,
                CurrentPassword = input.CurrentPassword,
                CurrentUserName = input.CurrentUserName,
                NewEmail = input.NewEmail,
                NewPassword = input.NewPassword,
                NewUserName = input.NewUserName
            };

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(updateUser));
            channel.BasicPublish(
                exchange: "",
                routingKey: "users.update",
                basicProperties: props,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            var result = respQueue.Take();

            foreach (var identityResult in result)
                if (!identityResult.Succeeded)
                    return BadRequest(result);
            
            return Ok(result);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            string id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            string replyQueueName;
            EventingBasicConsumer consumer;
            BlockingCollection<DeleteResponse> respQueue = new BlockingCollection<DeleteResponse>();
            IBasicProperties props;
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(JsonConvert.DeserializeObject<DeleteResponse>(response));
                }
            };

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(id));
            channel.BasicPublish(
                exchange: "",
                routingKey: "users.delete",
                basicProperties: props,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            var result = respQueue.Take();

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}
