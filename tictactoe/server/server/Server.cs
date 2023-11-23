using System;
using System.Collections.Generic;
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
        public List<Client> clients = new List<Client>();

        private void acceptClient(Client client)
        {
            clients.Add(client);
            //lobby.clients.Add(client); dodac liste klientow w lobby i tu sb przekaze
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
                    Client client = new Client(connection);
                    acceptClient(client);
                }

            });
            clientAcceptThread.Start();
        }
    }
}
