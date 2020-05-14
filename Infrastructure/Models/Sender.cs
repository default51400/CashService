using System;
using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Models
{
    public class Sender
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<string> respQueue;
        private readonly IBasicProperties props;
        private IConfiguration configuration;

        public Sender(IConfiguration configuration)
        {
            this.configuration = configuration;
            var factory = new ConnectionFactory();
            SetConfigurations(factory);

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);
            respQueue = new BlockingCollection<string>();

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            consumer.Received += (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == correlationId)
                    respQueue.TryAdd(Encoding.UTF8.GetString(ea.Body.Span));
            };
        }

        public string Call(string message)
        {
            channel.BasicPublish(
                exchange: "",
                routingKey: "CashService",
                basicProperties: props,
                body: Encoding.UTF8.GetBytes(message));

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            return respQueue.Take();
        }

        public void Close()
        {
            connection.Close();
        }

        private void SetConfigurations(ConnectionFactory connectionFactory)
        {
            connectionFactory.HostName = configuration.GetValue<string>("RabbitMqConnection:HostName");
            connectionFactory.Port = configuration.GetValue<int>("RabbitMqConnection:Port");
            connectionFactory.UserName = configuration.GetValue<string>("RabbitMqConnection:Username");
            connectionFactory.Password = configuration.GetValue<string>("RabbitMqConnection:Password");
        }
    }
}
