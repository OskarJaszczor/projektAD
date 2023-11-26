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
using SharedData;
namespace server
{
    class Lobby
    {
         public List<Client> lobby_clients = new List<Client>();
         public List<Client> waiting_players = new List<Client>();
         private StreamReader reader;
         private StreamWriter writer;

        public Lobby()
        {
            startListeningThread();
            arePlayersReady();
        }
         
        private void startListeningThread()
        {
            Thread listeningThread = new Thread(() =>
            {
                while (true)
                {
                    for (int i = 0; i < lobby_clients.Count; i++)
                    {
                        string message = lobby_clients[i].readMessage();
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
            });
            listeningThread.Start();           
        }

        public void arePlayersReady()
        {
            Thread arePlayersReadyThread = new Thread(() =>
            {
                int? position = null;
                while(true)
                {
                    for(int i = 0;i < lobby_clients.Count;i++)
                    {
                        if (lobby_clients[i].readyToPlay == true)
                        {
                            waiting_players.Add(lobby_clients[i]);
                            lobby_clients.Remove(lobby_clients[i]);
                            break;
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
                    if (waiting_players.Count >= 2)
                    {
                        Game gra = new Game();
                        gra.game_clients.Add(waiting_players[0]);
                        gra.game_clients.Add(waiting_players[1]);
                        waiting_players.Remove(waiting_players[0]);
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
