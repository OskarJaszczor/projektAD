using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using shared;
namespace server
{
    class Lobby
    {
        public static List<Client> lobby_clients = new List<Client>();
        public List<Client> waiting_players = new List<Client>();
         
        private object lobbyLock = new object();
        public Lobby()
        {
            startListeningThread();
            arePlayersReady();
            sendPlayersToGame();
        }
         
        private void startListeningThread()
        {
            Thread listeningThread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10);
                    if (lobby_clients.Count > 0)
                    {
                        lock (lobbyLock)
                        {
                            for (int i = lobby_clients.Count - 1; i >= 0; i--)
                            {
                                string message = lobby_clients[i].getPacket();
                                if (message != null)
                                {
                                    var splitted = message.Split('\0');
                                    switch (splitted[0])
                                    {
                                        case "Chat":
                                            sendMessageToAll(Config.GameMessageType.Chat, splitted[1]);
                                            break;
                                        case "Play":
                                            lobby_clients[i].readyToPlay = true; // jak usuwa kleintow z lobby_clients to petle wywala po sie zmienjsz ilosc indexow
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            });
            listeningThread.Start();
                         
        }
        public void arePlayersReady()
        {
            Thread arePlayersReadyThread = new Thread(() =>
            {
                int? position = null;
                while (true)
                {
                    Thread.Sleep(10);
                    lock (lobbyLock)
                    {

                        for (int i = 0; i < lobby_clients.Count; i++)
                        {
                            if (lobby_clients[i].readyToPlay == true)
                            {
                                waiting_players.Add(lobby_clients[i]);
                                lobby_clients.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }
            });
            arePlayersReadyThread.Start();
                 
        }
        public void sendPlayersToGame()
        {
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10);
                    if (waiting_players.Count >= 2)
                    {
                        lock (lobbyLock)
                        {
                            Game gra = new Game();
                            Console.WriteLine("Utworzono gre");

                            for(int i = 0; i < 2; i++)
                            {
                                Client client = waiting_players[0];
                                gra.game_clients.Add(client);
                                waiting_players.RemoveAt(0);
                                lobby_clients.Remove(client);

                            }
                            
                            gra.startGame();

                        }
                        //gra.Start();
                    }
                }
            }).Start();
            
        }
        private void sendMessageToAll(Config.GameMessageType type, string data)
        {
            foreach(Client client in lobby_clients)
            {
                string message = type + "\0" + data;
                client.sendMessage(message);
            }
        }

    }

}
