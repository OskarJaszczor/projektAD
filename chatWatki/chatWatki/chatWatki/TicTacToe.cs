using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace chatWatki
{
    class TicTacToe
    {
        public int Clicked { set; get; }
        public bool Character { set; get; } // x - false, o - true

        public TicTacToe(int clicked, bool character) { 
            
            Clicked = clicked;
            Character = character;
        }
    }
}
