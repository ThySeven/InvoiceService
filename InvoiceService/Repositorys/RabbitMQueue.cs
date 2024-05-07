﻿using InvoiceService.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace InvoiceService.Repositorys
{
    public class RabbitMQueue : IAuctionCoreQueue
    {
        ConnectionFactory factory;
        IConnection connection;
        IModel channel;
        EventingBasicConsumer consumer;

        string queue = Environment.GetEnvironmentVariable("RabbitMQQueueName");

        public RabbitMQueue()
        {
            factory = new ConnectionFactory { HostName = Environment.GetEnvironmentVariable("RabbitMQHostName") };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(queue: this.queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            consumer = new EventingBasicConsumer(channel);
        }

        public void Add(AutoMail mail)
        {
            var message = JsonSerializer.Serialize(mail);
            var bytes = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                routingKey: queue,
                basicProperties: null,
                body: bytes);
            AuctionCoreLogger.Logger.Info("sent " + mail.ReceiverMail);
        }

        public void Update(AutoMail mail)
        {
            throw new NotImplementedException();
        }
    }
}
