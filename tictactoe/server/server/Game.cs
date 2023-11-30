using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharedData;
namespace server
{
    class Game
    {
        public List<Client> game_clients = new List<Client>();
        public Game() 
        {
            startListeningThread();
        }
        public void startGame()
        {
            foreach (Client client in game_clients)
            {
                string message = Config.GameMessageType.Game.ToString();
                client.sendMessage(message);
            }
        }
        private void startListeningThread()
        {
            Thread listeningThread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10);
                    if (game_clients.Count > 0)
                    {
                        foreach(Client client in game_clients)
                        {
                            string message = client.getPacket();
                            if (message != null)
                            {
                                Console.WriteLine(message);
                                var splitted = message.Split('\0');
                                switch (splitted[0])
                                {
                                    case "InGameChat":
                                        Console.WriteLine("dziala");
                                        sendMessageToAll(Config.GameMessageType.InGameChat, splitted[1]);
                                        break;
                                }
                            }
                        }
                        
                    }
                }
            });
            listeningThread.Start();
        }
        private void sendMessageToAll(Config.GameMessageType type, string data)
        {
            foreach (Client client in game_clients)
            {
                string message = type.ToString() + "\0" + data;
                client.sendMessage(message);
            }
        }
    }
}
