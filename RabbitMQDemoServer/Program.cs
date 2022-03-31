using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemoServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Hello World
            // HelloWorldSender.Run();

            // 2. Work Queue
            // WorkQueuesSender.Run();

            // 3. Publish/Subcribe
            // EmitLogSender.RunLoop(1000);

            // 4. Routeing
            // EmitLogDirectSender.RunLoop(1000);

            // 5. Topics
            // EmitLogTopicSender.RunLoop(1000);

            // 6. RPC
            // RPCServer.Run();

            // 7. PublisherConfirms

            PublisherConfirms.Run();

            Console.WriteLine("Press [Enter] to Exit.");
            Console.ReadLine();
        }
    }
}
