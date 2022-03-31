using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQDemoServer
{
    public class EmitLogSender
    {
        public static void RunLoop(int times)
        {
            Random rnd = new Random();

            for (int i = 0; i < times; i++)
            {
                Run(i);

                Thread.Sleep(rnd.Next(500, 10000));
            }
        }

        private static void Run(int index)
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
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                var message = GetMessage(index);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "logs",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine(" [x] Sent {0}", message);
            }
        }

        private static string GetMessage(int index)
        {
            return ($"info: Hello World {index}");

        }
    }
}
