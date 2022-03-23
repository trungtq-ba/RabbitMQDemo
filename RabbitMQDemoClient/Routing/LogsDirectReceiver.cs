using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemoClient
{
    public class LogsDirectReceiver
    {
        private static string[] Serverity = new string[] { "info", "warning", "error" };

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
                foreach (var severity in Serverity)
                {
                    channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue: queueName,
                                      exchange: "direct_logs",
                                      routingKey: severity);

                    Console.WriteLine(" [*] Waiting for messages.");

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, message);
                    };

                    channel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);
                }

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
