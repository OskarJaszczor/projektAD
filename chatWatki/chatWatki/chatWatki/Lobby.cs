// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;

namespace chatWatki
{
    class Lobby
    {
        public List<TcpClient> waiting_player = new List<TcpClient>();

        public void addClient(TcpClient client)
        {
            waiting_player.Add(client);
        }

        public void Start()
        {
            new Thread(() =>
            {
                while(true)
                {
                    if(waiting_player.Count >=2)
                    {
                        //Create Game
                        Game gra = new Game();
                        gra.Start();

                        gra.addClient(waiting_player[0]);
                        gra.addClient(waiting_player[1]);
                        waiting_player.RemoveAt(0);
                        waiting_player.RemoveAt(0);
                    }
                }
            }).Start();
        }

    }
}
