using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemoServer
{
    /// <summary>
    /// https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html
    /// </summary>
    public class HelloWorldSender
    {
        public static void Run()
        {
            var factory = new ConnectionFactory()
            {
                HostName="10.1.11.120",
                UserName="guest",
                Password="guest"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete:false,
                                    arguments:null
                 );

                string message = "Hello RabbitMQ!";

                var body = UTF8Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange:"",
                    routingKey:"hello",
                    basicProperties:null,
                    body: body
                    );

                Console.WriteLine("[x] Sent {0} ", message);
            }
        }
    }
}
