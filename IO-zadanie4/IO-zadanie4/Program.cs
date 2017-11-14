using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Drawing;

namespace IO_zadanie4
{
    class Program
    {
        static private Object thisLock = new Object();
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(TCPServer);
            ThreadPool.QueueUserWorkItem(TCPClient);
            ThreadPool.QueueUserWorkItem(TCPClient);
            ThreadPool.QueueUserWorkItem(TCPClient);
            ThreadPool.QueueUserWorkItem(TCPClient);
            ThreadPool.QueueUserWorkItem(TCPClient);
            ThreadPool.QueueUserWorkItem(TCPClient);
            Thread.Sleep(7000);
        }

        
            static void writeConsoleMessage(string message, ConsoleColor color)
            {
            lock (Program.thisLock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine("Otrzymalem wiadomosc: " + message);
                Console.ResetColor();
            }
            }


        static void TCPServer(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);

            server.Start();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(Clint, client);
            }

        }

        static void Clint(object stateInfo)
        {
            byte[] buffer = new byte[1024];
            ConsoleColor color = ConsoleColor.Green;
            var client = (TcpClient)stateInfo;

            client.GetStream().Read(buffer, 0, 1024);
            string s = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            writeConsoleMessage(s, color);
            byte[] message = new ASCIIEncoding().GetBytes("Server");
            client.GetStream().Write(message, 0, message.Length);
            client.Close();
        }

        static void TCPClient(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] message = new ASCIIEncoding().GetBytes("wiadomosc");
            client.GetStream().Write(message, 0, message.Length);
            ConsoleColor color = ConsoleColor.Red;
            byte[] buffer = new byte[1024];
            client.GetStream().Read(buffer, 0, 1024);
            string s = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            writeConsoleMessage(s, color);

        }

    }
}
//Problem związany z zmianą kolorów w metodzie writeConsoleMessage został rozwiązany za pomocą mechanizmu Lock.
//Mechanizm ten moze powodować wydłużenie czasu pracy wątków czekających na wejście do tej metody.