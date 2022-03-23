using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemoClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Hello World
            // HelloWorldReceiver.Run();

            // 2. Work Queue
            // WorkQueueReceiver.Run();

            // 3. Publish/Subcribe
            // LogsReceiver.Run();

            // 4. Routing
            LogsDirectReceiver.Run();

            //
            Console.WriteLine("Press [Enter] to Exit.");
            Console.ReadLine();
        }
    }
}
