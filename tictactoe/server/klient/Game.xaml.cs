using shared;
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
        string username;
        public Game()
        {
            InitializeComponent();
            client = Lobby.client;
            reader = new(client.GetStream());
            writer = new(client.GetStream());
            username = Lobby.username;
            nickGame.Content = username;
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
        public void drawOnBoard(int index, string character)
        {
            string buttonName = "boxName" + index;
            Dispatcher.Invoke(() =>
            {
                Button foundButton = FindName(buttonName) as Button;
                foundButton.Content = character == "X" ? "X" : "O";
            });
        }
        public void move(int index)
        {
            sendData(Config.GameMessageType.Move, index.ToString());
        }
        private void boxName0_Click(object sender, RoutedEventArgs e)
        {
            move(1);
        }

        private void boxName1_Click(object sender, RoutedEventArgs e)
        {
            move(2);
        }

        private void boxName2_Click(object sender, RoutedEventArgs e)
        {
            move(3);
        }

        private void boxName3_Click(object sender, RoutedEventArgs e)
        {
            move(4);
        }

        private void boxName4_Click(object sender, RoutedEventArgs e)
        {
            move(5);
        }

        private void boxName5_Click(object sender, RoutedEventArgs e)
        {
            move(6);
        }

        private void boxName6_Click(object sender, RoutedEventArgs e)
        {
            move(7);
        }

        private void boxName7_Click(object sender, RoutedEventArgs e)
        {
            move(8);
        }

        private void boxName8_Click(object sender, RoutedEventArgs e)
        {
            move(9);
        }

    }
}
