using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
         private StreamReader reader;
         private StreamWriter writer;

        public Lobby()
        {
            startListeningThread();
        }
         
        private void startListeningThread()
        {

                Thread t = new Thread(() =>
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
                                }
                            }
                        }
                    }
                });
                t.Start();
            
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
