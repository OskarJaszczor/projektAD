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
        TcpClient client;
        public static string arena;
        public string? log;
        private MainWindow mainWindow;
        public Lobby(MainWindow mainWindow)
        {
            InitializeComponent();
            log = MainWindow.log;
            TcpClient client = mainWindow.client;
            serverLeaderBoard.Items.Add(log);
            
            serverListBox.Items.Add("Arena 1");
            serverListBox.Items.Add("Arena 2");
            serverListBox.Items.Add("Arena 3");
            serverListBox.Items.Add("Arena 4");
            serverListBox.Items.Add("Arena 5");


        }

        private void serverListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            arena = serverListBox.SelectedItem.ToString();
            StreamWriter writer = new(client.GetStream());
            writer.WriteLine(arena);
            writer.Flush();
        }
    }
}
