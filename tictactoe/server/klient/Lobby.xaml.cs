using System;
using System.Collections.Generic;
using System.Configuration;
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
using SharedData;
namespace klient
{
    /// <summary>
    /// Logika interakcji dla klasy Lobby.xaml
    /// </summary>
    public partial class Lobby : Window
    {
        public string username;
        public TcpClient client { get; set; }
        StreamReader reader = null;
        StreamWriter writer = null;

        public void setUp(string nickname)
        {
            nickLobby.Content = username;
        }

        public Lobby()
        {
            InitializeComponent();
            client = new TcpClient("127.0.0.1", 4444);
            reader = new(client.GetStream());
            writer = new(client.GetStream());

            Thread DataReaderThread = new(() =>
            {
                while(true)
                {                    
                    readData();
                }
            });
            DataReaderThread.Start();

        }
        private void sendMessageBtn_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageContentTbox.Text;
            MessageContentTbox.Text = null;
            sendData(Config.GameMessageType.Chat, message);
            
        }
        private void sendData(Config.GameMessageType type,string message)
        {
            //MessageBox.Show(message);
            writer.WriteLine(type + "\0" + message);
            writer.Flush();
        }
        private void readData()
        {
            string message = reader.ReadLine();
            //MessageBox.Show(message);
            var splitted = message.Split('\0');
            switch (splitted[0])
            {
                case "Chat":
                    showDataOnChat(splitted[1]);                   
                    break;
                case "Game":
                    Dispatcher.Invoke(() =>
                    {
                        Game game = new Game();
                        Visibility = Visibility.Hidden;
                        game.Visibility = Visibility.Visible;
                    });

                    break;
            }
        }
        private void showDataOnChat(string data)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                receivedTbox.Text += data + "\n";
            }));
            
        }

        private void searchGameButton_Click(object sender, RoutedEventArgs e)
        {
            writer.WriteLine(Config.GameMessageType.Play);
            writer.Flush();
        }
    }
}
