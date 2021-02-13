using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Services
{
    public class RabbitMqService
    {
        private IConnection connection;
        private IModel channel;

        public RabbitMqService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            connection = factory.CreateConnection();
        }

        public ReturnType SendWithResult<ReturnType, ParameterType>(ParameterType data, string routingKey)
        {
            channel = connection.CreateModel();

            string replyQueueName;
            EventingBasicConsumer consumer;
            BlockingCollection<ReturnType> respQueue = new BlockingCollection<ReturnType>();
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
                    respQueue.Add(JsonConvert.DeserializeObject<ReturnType>(response));
                }
            };

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            channel.BasicPublish(
                exchange: "",
                routingKey: routingKey,
                basicProperties: props,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            var result = respQueue.Take();

            return result;
        }
    }
}
