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
            EmitLogDirectSender.RunLoop(1000);


            Console.WriteLine("Press [Enter] to Exit.");
            Console.ReadLine();
        }
    }
}
