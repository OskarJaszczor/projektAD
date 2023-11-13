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
