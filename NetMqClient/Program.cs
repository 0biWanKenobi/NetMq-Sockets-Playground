using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NetMqClient.Model;
using NetMQ;
using NetMQ.Sockets;

namespace NetMqClient
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Creating socket..");
            using (var pushSocket = new PushSocket(">tcp://localhost:5556")
            {
                /* So apparently this is needed to prevent the socket from being immediately disposed
                   after the last instruction of the using block.
                   With this setting, the socket shall be disposed only after the message has been sent.
                   This is equivalent to say "wait forever", but someone may arguably want to set a finite 
                   amount of time when in production.
                */
                Options = { Linger = TimeSpan.FromSeconds(-1)}
            })
            {
                Console.WriteLine("Creating message..");
                var message = new Message
                {
                    Text = "test message",
                    TimeStamp = DateTime.Now,
                    Type = "test",
                    ValueCode = 0
                };
                Console.WriteLine("Serializing message..");
                using (var memoryStream = new MemoryStream())
                {
                    var binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(memoryStream, message);
                    var serializedMessage = memoryStream.ToArray();
                    Console.WriteLine("Sending message..");
                    pushSocket.SendFrame(serializedMessage);
                }
            }
            Console.WriteLine("Performing cleanup..");
            NetMQConfig.Cleanup();
        }
    }
}
