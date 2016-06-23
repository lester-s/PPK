using PPKBuisnessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PPKHttplistener
{
    class Program
    {
        private static UTF8Encoding encoding = new UTF8Encoding();

        private static List<ChatUser> connectedUsers = new List<ChatUser>();

        static void Main(string[] args)
        {
            TcpListener myListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);

            myListener.Start();

            while (true)
            {
                var incomingClient = myListener.AcceptSocket();
                ThreadPool.QueueUserWorkItem(HandleClient, incomingClient);
            }
        }

        private static void HandleClient(object state)
        {
            var incomingsocket = state as Socket;
            var clientStream = new NetworkStream(incomingsocket);
            var incomingUser = new ChatUser()
            {
                UserSocket = incomingsocket,
                UserReader = new BinaryReader(clientStream),
                UserWriter = new BinaryWriter(clientStream)
            };
            connectedUsers.Add(incomingUser);


            do
            {
                try
                {
                    // read the string sent to the server
                    var theReply = incomingUser.UserReader.ReadString();
                    Console.WriteLine("\r\n" + theReply);

                    foreach(var user in connectedUsers)
                    {
                        user.UserWriter.Write(theReply);
                    }
                }
                catch (Exception)
                {
                    break;
                }
            } while (true);

            Console.WriteLine("client connected");
        }
    }
}
