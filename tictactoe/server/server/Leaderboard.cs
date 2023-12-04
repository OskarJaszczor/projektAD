using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public class Leaderboard
    {
        public string Nickname { get; set; }
        public int WinGames { get; set; }
        public Leaderboard(string nickname, int winGames)
        {
            Nickname = nickname;
            WinGames = winGames;
        }
    }
}
