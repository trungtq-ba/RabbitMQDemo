using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemoClient
{
    public class HelloWorldReceiver
    {
        public static void Run()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "10.1.11.120",
                UserName = "guest",
                Password = "guest"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null
                 );

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();

                    var message = UTF8Encoding.UTF8.GetString(body);

                    Console.WriteLine("[x] received {0} ",message);
                };

                channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);

            }
        }
    }
}
