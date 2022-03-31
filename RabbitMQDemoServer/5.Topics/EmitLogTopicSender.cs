using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQDemoServer
{
    /// <summary>
    /// https://www.rabbitmq.com/tutorials/tutorial-five-dotnet.html
    /// </summary>
    public class EmitLogTopicSender
    {
        public static string[] Agrs = new string[] { "kern.critical" };

        protected static string ExchangeName { get; } = "topic_logs";

        protected static string DefaultRoutingKey  { get; } = "anonymous.info";

        public static void Run(int index)
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
                channel.ExchangeDeclare(exchange: ExchangeName,  type: ExchangeType.Topic);

                var routingKey = (Agrs.Length > 0) ? Agrs[0] : DefaultRoutingKey;

                var message = $"Hello World {index} times";

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: ExchangeName,
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine(" [x] Sent Routingkey: '{0}', Message: '{1}'", routingKey, message);
            }
        }

        public static void RunLoop(int times)
        {
            Random rnd = new Random();

            for (int i = 0; i < times; i++)
            {
                Run(i);

                Thread.Sleep(rnd.Next(500, 10000));
            }
        }
    }
}
