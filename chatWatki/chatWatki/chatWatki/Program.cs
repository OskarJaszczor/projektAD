// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace chatWatki
{


    class OldGame
    {
       
    }

    class OldLobby
    {
        public List<TcpClient> waiting_player = new List<TcpClient>();
        public List<TcpClient> players = new List<TcpClient>();
        public Game[] games = new Game[5];

        public OldLobby()
        {
            for (int i = 0; i < games.Length; i++)
            {
                games[i] = new Game();
            }
        }

        public void addClient(TcpClient client)
        {
            waiting_player.Add(client);
        }

        public void Start()
        {
            new Thread(() =>
            {
                while (true)
                {
                    if (waiting_player.Count >=2)
                    {
                        Game gra = new Game();

                        gra.addClient(waiting_player[0]);
                        gra.addClient(waiting_player[1]);
                        waiting_player.RemoveAt(0);
                        waiting_player.RemoveAt(0);

                        gra.Start();
                    }
                }
            }).Start();
        }

    }
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Server start!");

            TcpListener listener = new TcpListener(IPAddress.Any, 9999);
            listener.Start();

            OldLobby lobby = new OldLobby();

            lobby.Start();
            
            while (true)
            {
                lobby.addClient(listener.AcceptTcpClient());             

            }

            //Game gra = new Game();
            //gra.Start();
            

            //gra.addClient(listener.AcceptTcpClient());
            //gra.addClient(listener.AcceptTcpClient());


        }
    }
}
