using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NetMqClient.Model;
using NetMQ;
using NetMQ.Sockets;
using Console = System.Console;

namespace NetMqServer
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Creating socket..");
            using (var pullSocket = new PullSocket("@tcp://localhost:5556"))
            {
                Console.WriteLine("Receiving message..\n\n");
                var serializedMessage = pullSocket.ReceiveFrameBytes();
                using (var memoryStream = new MemoryStream(serializedMessage))
                {
                    var binaryFormatter = new BinaryFormatter();
                    var message = binaryFormatter.Deserialize(memoryStream) as Message;
                    Console.WriteLine($"Message contents: \n" +
                                      $"\tText: {message?.Text}\n" +
                                      $"\tType: {message?.Type}\n" +
                                      $"\tTimestamp: {message?.TimeStamp}\n" +
                                      $"\tValueCode: {message?.ValueCode}\n");
                }
            }
            Console.WriteLine("Performing cleanup..");
            NetMQConfig.Cleanup();
            Console.WriteLine("Press enter to terminate execution!");
            Console.ReadLine();
        }
    }
}
