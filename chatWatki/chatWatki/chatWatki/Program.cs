// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace chatWatki
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Server start!");

            TcpListener listener = new TcpListener(IPAddress.Any, 9999);
            listener.Start();

            Lobby lobby = new Lobby();

            lobby.Start();

            while (true)
            {
                lobby.addClient(listener.AcceptTcpClient());


            }
            /*
             *                 Game gra = new Game();
                gra.Start();

                gra.addClient(listener.AcceptTcpClient());
                gra.addClient(listener.AcceptTcpClient());
        
            */
        }
    }
}
