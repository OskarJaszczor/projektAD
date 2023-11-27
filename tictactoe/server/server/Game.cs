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
        StreamReader reader;
        StreamWriter writer;
        public Game() 
        {

        }
        public void startGame()
        {
            foreach (Client client in game_clients)
            {
                string message = Config.GameMessageType.Game.ToString();
                writer.WriteLine(message);
                writer.Flush();
            }
        }
    }
}
