using PPKBuisnessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PPKConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient myClient = new TcpClient();
            myClient.Connect("127.0.0.1", 8080);

            var tcpStream = myClient.GetStream();
            var myStreamWriter = new BinaryWriter(tcpStream);
            var myStreamReader = new BinaryReader(tcpStream);

            ThreadPool.QueueUserWorkItem(HandleWriting, myStreamWriter);
            ThreadPool.QueueUserWorkItem(HandleReading, myStreamReader);
            
        }

        private static void HandleReading(object state)
        {
            var myStreamReader = state as BinaryReader;
            do
            {
                try
                {
                    // read the string sent to the server
                    var theReply = myStreamReader.ReadString();
                    Console.WriteLine("\r\n" + theReply);
                }
                catch (Exception)
                {
                    break;
                }
            } while (true);
        }

        private static void HandleWriting(object state)
        {
            try
            {
                var myStreamWriter = state as BinaryWriter;
                var stayAlive = true;

                while (stayAlive)
                {
                    ThreadSafeConsole.WriteLine("Write commmand: ");
                    var command = ThreadSafeConsole.ReadLine();
                    if (string.IsNullOrWhiteSpace(command))
                    {
                        stayAlive = false;
                    }
                    else
                    {
                        myStreamWriter.Write(command);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public static class ThreadSafeConsole
        {
            private static object _lockObject = new object();

            public static void WriteLine(string str)
            {
                lock (_lockObject)
                {
                    Console.WriteLine(str);
                }
            }

            public static string ReadLine()
            {
                lock (_lockObject)
                {
                    return Console.ReadLine(); ;
                }
            }
        }
    }
}
