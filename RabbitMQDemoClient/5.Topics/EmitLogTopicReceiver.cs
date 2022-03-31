using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemoClient
{
    /// <summary>
    /// https://www.rabbitmq.com/tutorials/tutorial-five-dotnet.html
    /// </summary>
    public class EmitLogTopicReceiver
    {
        public static string[] BindingKeys = new string[] { "kern.*", "*.critical", "#" };


        protected static string ExchangeName { get; } = "topic_logs";


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
                channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Topic);

                foreach (var bindingKey in BindingKeys)
                {
                    // declare a server-named queue
                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue: queueName,
                                      exchange: ExchangeName,
                                      routingKey: bindingKey);

                    Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();

                        var message = Encoding.UTF8.GetString(body);

                        var routingKey = ea.RoutingKey;

                        Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, message);
                    };

                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                }


                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
