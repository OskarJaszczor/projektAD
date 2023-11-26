using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    class Game
    {
        public List<Client> game_clients = new List<Client>();

        public Game() 
        { 
            new Thread(() =>
            {
                while (true)
                {
                    Console.WriteLine(game_clients.Count);
                }
            }).Start();

        }
    }
}
