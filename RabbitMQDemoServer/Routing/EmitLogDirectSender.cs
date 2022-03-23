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
    /// https://www.rabbitmq.com/tutorials/tutorial-four-dotnet.html
    /// </summary>
    public class EmitLogDirectSender
    {
        private static string[] Serverity = new string[] { "info", "warning", "error" };

        private static Random rnd = new Random();

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
                channel.ExchangeDeclare(exchange: "direct_logs",type: ExchangeType.Direct);

                var severity = Serverity[rnd.Next(Serverity.Length)];
                var message = (Serverity.Length > 1) ? string.Join(" ", Serverity.Skip(1).ToArray()) + " Hello World!" : "Hello World!";

                var body = Encoding.UTF8.GetBytes(message);
                
                channel.BasicPublish(exchange: "direct_logs",
                                     routingKey: severity,
                                     basicProperties: null,
                                     body: body);
                
                Console.WriteLine(" [x] Sent '{0}':'{1}'", severity, message);
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
