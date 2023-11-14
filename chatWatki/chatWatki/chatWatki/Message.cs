
namespace chatWatki
{
    class Message
    {
        public string Msg { get; set; }
        public string User { get; set; }

        public Message(string msg, string user)
        {
            Msg = msg;
            User = user;
        }
    }
}
