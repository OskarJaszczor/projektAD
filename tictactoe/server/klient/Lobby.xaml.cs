using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace klient
{
    /// <summary>
    /// Logika interakcji dla klasy Lobby.xaml
    /// </summary>
    public partial class Lobby : Window
    {
        public string username;
        public static TcpClient client { get; set; }
        StreamReader reader = null;
        StreamWriter writer = null;

        public void setUp(string nickname)
        {
            nickLobby.Content = username;
        }

        public Lobby()
        {
            InitializeComponent();
            client = new TcpClient("127.0.0.1", 9999);
            reader = new(client.GetStream());
            writer = new(client.GetStream());
            

        }
    }
}
