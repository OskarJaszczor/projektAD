using SharedData;
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
        public TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        public Client(TcpClient client)
        {
            this.client = client;
            InitializeStreams();
            //startListeningThread();
            
        }
        private void InitializeStreams()
        {
            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
        }
        public void sendMessage(string data)
        {
            Console.WriteLine(data);
            writer.WriteLine(data);
        }
        public string readMessage()
        {
            return reader.ReadLine();
        }

    }
}
