using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace server
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Server start!");

            Server server = new Server();
            server.startListening(8080);

            while (true)
                Thread.Sleep(999999);
        }
    }
}
