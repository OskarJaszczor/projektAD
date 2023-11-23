using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    class Client
    {
        private TcpClient client;
        
        public Client(TcpClient client)
        {
            this.client = client;
            startListeningThread();
        }
        private void startListeningThread()
        {
            Thread t = new Thread(() =>
            {
                StreamReader reader = new StreamReader(client.GetStream());
            });
            t.Start();
        }
    }
}
