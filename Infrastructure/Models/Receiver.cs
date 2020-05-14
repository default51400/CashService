using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Helpers;
using Infrastructure.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Models
{
    public class Receiver
    {
        private ConnectionFactory factory;
        private IConfiguration configuration;
        private ICashOrderRepository db;

        public Receiver(IConfiguration configuration, ICashOrderRepository repository)
        {
            this.configuration = configuration;
            db = repository;
            factory = new ConnectionFactory();
            SetConfigurations(factory);
        }

        public void Start()
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "CashService",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    channel.BasicQos(0, 1, false);

                    var consumer = new EventingBasicConsumer(channel);

                    channel.BasicConsume(queue: "CashService",
                                         autoAck: false,
                                         consumer: consumer);

                    //Awaiting requests
                    consumer.Received += async (model, ea) =>
                    {
                        string response = null;

                        var replyProps = channel.CreateBasicProperties();
                        replyProps.CorrelationId = ea.BasicProperties.CorrelationId;

                        try
                        {
                            response = await CreateResponse(ea);
                        }
                        catch (Exception e)
                        {
                            response = e.Message + "\n" + e.StackTrace;
                        }
                        finally
                        {
                            PublishResponse(ea, channel, response, replyProps);
                        }
                    };
                    Console.ReadLine();
                }
            }
        }

        private async Task<string> CreateResponse(BasicDeliverEventArgs ea)
        {
            var takedMessage = Encoding.UTF8.GetString(ea.Body.Span);
            List<CashOrder> ordersList = new List<CashOrder>();
            ordersList = CashOrdersSerializer.Deserialize(takedMessage);
            ordersList = await RepositoryHelper.GetOrCreateOrders(ordersList, db);

            return CashOrdersSerializer.Serialize(ordersList);
        }

        private static void PublishResponse(BasicDeliverEventArgs ea, IModel channel, string response, IBasicProperties replyProps)
        {
            channel.BasicPublish(exchange: "", routingKey: ea.BasicProperties.ReplyTo,
              basicProperties: replyProps, body: Encoding.UTF8.GetBytes(response));
            channel.BasicAck(deliveryTag: ea.DeliveryTag,
              multiple: false);
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
