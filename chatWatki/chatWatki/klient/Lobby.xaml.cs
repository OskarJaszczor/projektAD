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
    /// 
    public partial class Lobby : Window
    {
        TcpClient client1;
        public int arena;
        public string? log;
        private MainWindow mainWindow;
        public Lobby(MainWindow mainWindow)
        {
            InitializeComponent();
            log = MainWindow.log;
            client1 = MainWindow.client;
            serverLeaderBoard.Items.Add(log);
            
            serverListBox.Items.Add("Arena 1");
            serverListBox.Items.Add("Arena 2");
            serverListBox.Items.Add("Arena 3");
            serverListBox.Items.Add("Arena 4");
            serverListBox.Items.Add("Arena 5");


        }

        private void arenaResponse(StreamReader reader)
        {
            string raw = reader.ReadLine();
            string[] splitted = raw.Split('\0');
            if(splitted[0] == "Arena")
            {
                int avaiable = Int32.Parse(splitted[1]);
                if(avaiable == 1)
                {
                    //dostepne
                }
                if (avaiable == 0)
                {
                    //niedostepne
                    MessageBox.Show("Arena zajeta!");
                }
            }
        }

        private void serverListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            arena = serverListBox.SelectedIndex;
            arena++;
            MessageBox.Show(arena.ToString());
            StreamWriter writer = new(client1.GetStream());
            StreamReader reader = new(client1.GetStream());
            writer.WriteLine(Config.GameMessageType.Arena + "\0" + arena);
            writer.Flush();
            arenaResponse(reader);
        }
    }
}
