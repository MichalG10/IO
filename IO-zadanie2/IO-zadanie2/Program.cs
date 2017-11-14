using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace IO_zadanie2
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(TCPServer);
            ThreadPool.QueueUserWorkItem(TCPClient);
            ThreadPool.QueueUserWorkItem(TCPClient);
            Thread.Sleep(7000);
        }

        static void TCPServer(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, 1024);
                Console.WriteLine("ok");
                client.GetStream().Write(buffer, 0, buffer.Length);
                client.Close();
            }

        }

        static void TCPClient(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] message = new ASCIIEncoding().GetBytes("wiadomosc");
            client.GetStream().Write(message, 0, message.Length);

        }

    }
}


//Wątek serwera nasłuchuje na porcie 2048. Gdy połączy się jakiś klient, serwer przechodzi najpierw do nasłuchiwania wiadomości, a nastepnie sam wysyła inną wiadomość.
//Problem może wystąpić z typem i wielkością wysyłanych danych.