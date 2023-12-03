using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    class Server
    {
        private TcpListener listener;
        private List<Client> clients = new List<Client>();
        public Lobby lobby = new Lobby();
        private void acceptClient(Client client)
        {
            clients.Add(client);
            Lobby.lobby_clients.Add(client);
        }

        public void startListening(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Thread clientAcceptThread = new Thread(() =>
            {
                while(true)
                {
                    TcpClient connection = listener.AcceptTcpClient();
                    Console.WriteLine("Got new Client!");
                    Client client = new Client(connection);
                    acceptClient(client);
                }

            });
            clientAcceptThread.Start();
        }
    }
}
