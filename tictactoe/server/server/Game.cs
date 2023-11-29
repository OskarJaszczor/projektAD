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
                    Console.WriteLine("1");
                    foreach(Client client in game_clients)
                    {
                        string message = client.readMessage();
                        Console.WriteLine(message);
                    }
                }
            });
            listeningThread.Start();
        }
        private void sendMessageToAll(Config.GameMessageType type, string data)
        {
            foreach (Client client in game_clients)
            {
                string message = type + "\0" + data;
                client.sendMessage(message);
            }
        }
    }
}
