using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared
{
    public class Config
    {
        public enum GameMessageType
        {
            Chat,
            InGameChat,
            Play,
            Game,
            Move,
            InvalidMove,
            Win,
            Draw,
            LeaderBoard
        }
        
    }
}
