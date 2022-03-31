using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQDemoClient
{
    public class RPCClient
    {
        private const string QUEUE_NAME = "rpc_queue";

        private readonly IConnection connection;

        private readonly IModel channel;
        
        private readonly string replyQueueName;
        
        private readonly EventingBasicConsumer consumer;
        
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

        public RPCClient()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "10.1.11.120",
                UserName = "guest",
                Password = "guest"
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            // declare a server-named queue
            replyQueueName = channel.QueueDeclare().QueueName;

            consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                if (!callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out TaskCompletionSource<string> tcs))
                    return;
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                tcs.TrySetResult(response);
            };

            channel.BasicConsume(
              consumer: consumer,
              queue: replyQueueName,
              autoAck: true);
        }

        public Task<string> CallAsync(string message, CancellationToken cancellationToken = default(CancellationToken))
        {
            IBasicProperties props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var tcs = new TaskCompletionSource<string>();
            callbackMapper.TryAdd(correlationId, tcs);

            channel.BasicPublish(
                exchange: "",
                routingKey: QUEUE_NAME,
                basicProperties: props,
                body: messageBytes);

            cancellationToken.Register(() => callbackMapper.TryRemove(correlationId, out var tmp));
            return tcs.Task;
        }

        public void Close()
        {
            connection.Close();
        }
    }


    public class Rpc
    {
        private static Random rnd = new Random();

        public static void RunLoop(int times)
        {
            for (int i = 0; i < times; i++)
            {
                Run();

                Thread.Sleep(1000);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public static void Run()
        {
            Console.WriteLine("RPC Client");
            string n = rnd.Next(1, 100).ToString();
            Task t = InvokeAsync(n);
            t.Wait();
        }

        private static async Task InvokeAsync(string n)
        {
            var rpcClient = new RPCClient();

            Console.WriteLine(" [x] Requesting fib({0})", n);

            var response = await rpcClient.CallAsync(n.ToString());
            
            Console.WriteLine(" [.] Got '{0}'", response);

            rpcClient.Close();
        }
    }
}


