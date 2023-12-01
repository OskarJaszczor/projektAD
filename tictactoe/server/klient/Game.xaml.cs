using SharedData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
    /// Logika interakcji dla klasy Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        StreamReader reader = null;
        StreamWriter writer = null;
        TcpClient client = null;
        public Game()
        {
            InitializeComponent();
            client = Lobby.client;
            reader = new(client.GetStream());
            writer = new(client.GetStream());

        }

        private void sendMessageBtn_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageContentTbox.Text;
            MessageContentTbox.Text = null;
            sendData(Config.GameMessageType.InGameChat, message);

        }
        private void sendData(Config.GameMessageType type, string message)
        {
            //MessageBox.Show(message);
            string combined = type + "\0" + message;
            writer.WriteLine(combined);
            writer.Flush();
        }
        public void showDataOnChat(string data)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                receivedTbox.Text += data + "\n";
            }));

        }

    }
}
