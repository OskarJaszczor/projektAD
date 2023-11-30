﻿using SharedData;
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
        public bool readyToPlay;
        private List<String> packetQueue = new List<String>();
        public Object packetQueueLock = new Object();
        public Client(TcpClient client)
        {
            this.client = client;
            InitializeStreams();
            readyToPlay = false;
            //startListeningThread();
            Thread DataReader = new(() =>
           {
               while(true)
               {
                   string packet = reader.ReadLine();
                   lock (packetQueueLock)
                   {
                       packetQueue.Add(packet);
                   }
               }
           });
            DataReader.Start();
        }
        private void InitializeStreams()
        {
            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
        }
        public string getPacket()
        {
            lock (packetQueueLock)
            {
                if (packetQueue.Count >= 1)
                {
                    string data = packetQueue[0];
                    packetQueue.RemoveAt(0);
                    return data;
                }
            }
            return null;
        }
        public void sendMessage(string data)
        {
            writer.WriteLine(data);
        }
        public void setReadyToPlay()
        {
            readyToPlay = true;
        }
        public bool isReadyToPlay()
        {
            return readyToPlay;
        }
    }
}
