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
            
        }

        private void searchGameButton_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter writer = new(client1.GetStream());
            StreamReader reader = new(client1.GetStream());

            writer.WriteLine(Config.GameMessageType.Arena);
            writer.Flush();
        }
    }
}
