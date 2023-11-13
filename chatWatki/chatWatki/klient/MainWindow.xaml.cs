using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.PortableExecutable;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace klient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum GameMessageType
        {
            Chat,
            TicTacToeMove,
            TicTacToeWin,
            TicTacToeDraw,
            TicTacToeReset
        }
        public class boxClicked
        {
            public int Clicked { get; set; }
            public boxClicked(int clicked)
            {
                Clicked = clicked;
            }
        }
        class TicTacToe
        {
            public int Clicked;
            public bool Character; // x - false, o - true

            public TicTacToe(int clicked, bool character)
            {

                Clicked = clicked;
                Character = character;
            }
        }

        public TcpClient client;
        private string? log;
        public ObservableCollection<boxClicked> boxes = new();
        private static ObservableCollection<TicTacToe> tictactoe = new ObservableCollection<TicTacToe>();
        public MainWindow()
        {
            InitializeComponent();

            Visibility = Visibility.Hidden;
            nickWin win = new nickWin();

            if (win.ShowDialog() == true) {
                log = win.Login;
                Visibility = Visibility.Visible;
            }

            client = new TcpClient("127.0.0.1", 9999);

            Thread t = new Thread(() =>
            {
            StreamReader streamReader = new StreamReader(client.GetStream());
            while (true)
            {
                string raw = streamReader.ReadLine();
                string[] splitted = raw.Split('\0');
                switch (splitted[0])
                {
                    case "Chat":
                        string user = splitted[1],
                        msg = splitted[2];
                        Dispatcher.Invoke(() => receivedTbox.Text += $"[{user}] {msg}\n");
                        break;
                    case "TicTacToeMove":
                        bool player = Convert.ToBoolean(splitted[2]);
                        string index = splitted[1];
                        
                        tictactoe.Add(new TicTacToe(Int32.Parse(index), player));

                        break;
                        case "TicTacToeWin":
                            Dispatcher.Invoke(() => { ResetGameButton.Visibility = Visibility.Visible; });
                            StreamReader reader = new StreamReader(client.GetStream());
                            bool winnerBool = Convert.ToBoolean(splitted[1]);
                            string winner =  winnerBool ? "Kołko" : "Krzyżyk";
                            Dispatcher.Invoke( ()=>
                            {
                                winnerTxt.Text = winner + " Is Winner";
                                winnerTxt.Visibility = Visibility.Visible;
                            });
                            break;
                        case "TicTacToeDraw":
                            Dispatcher.Invoke(() => { ResetGameButton.Visibility = Visibility.Visible; });
                            Dispatcher.Invoke( ()=> drawTxt.Visibility = Visibility.Visible);
                            break;
                        case "TicTacToeReset":
                            CleanAll();
                            break;
                    }
                }
            });
            t.Start();


            tictactoe.CollectionChanged += (sender, e) =>
            {
                if (e.Action != NotifyCollectionChangedAction.Add) return;
                var newMessages = e.NewItems;

                Thread t = new(() =>
                {
                    CleanGameBoard();
                    foreach(var e in tictactoe)
                    {
                        string buttonName = "boxName" + e.Clicked.ToString();
                        Dispatcher.Invoke(() =>
                        {
                            Button foundButton = FindName(buttonName) as Button;
                            if (e.Character == false)
                            {
                                foundButton.Content = "X";
                            }
                            else
                            {
                                foundButton.Content = "O";
                            }

                        });
                    }
                });
                t.Start();
            };

        }

        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter writer = new StreamWriter(client.GetStream());
            string message = log + '\0' + sendTbox.Text;
            sendTbox.Text = "";
            var msg = (string)(GameMessageType.Chat + "\0" + message);      
            writer.WriteLine(msg);
            writer.Flush();

        }

        private void box1(object sender, RoutedEventArgs e)
        {
            boxes.Add(new boxClicked(0));
            SendTicTacToeMove(0);
        }

        private void box2(object sender, RoutedEventArgs e)
        {
            boxes.Add(new boxClicked(1));
            SendTicTacToeMove(1);
        }

        private void box3(object sender, RoutedEventArgs e)
        {
            boxes.Add(new boxClicked(2));
            SendTicTacToeMove(2);
        }
        
        private void box4(object sender, RoutedEventArgs e)
        {
            boxes.Add(new boxClicked(3));
            SendTicTacToeMove(3);
        }

        private void box5(object sender, RoutedEventArgs e)
        {
            boxes.Add(new boxClicked(4));
            SendTicTacToeMove(4);
        }

        private void box6(object sender, RoutedEventArgs e)
        {
            boxes.Add(new boxClicked(5));
            SendTicTacToeMove(5);
        }

        private void box7(object sender, RoutedEventArgs e)
        {
            boxes.Add(new boxClicked(6));
            SendTicTacToeMove(6);
        }

        private void box8(object sender, RoutedEventArgs e)
        {
            boxes.Add(new boxClicked(7));
            SendTicTacToeMove(7);
        }

        private void box9(object sender, RoutedEventArgs e)
        {
            boxes.Add(new boxClicked(8));
            SendTicTacToeMove(8);
        }

        private void SendTicTacToeMove(int index)
        {
            StreamWriter writer = new(client.GetStream());
            string msg = GameMessageType.TicTacToeMove + "\0" + index.ToString();
            //MessageBox.Show(msg);
            writer.WriteLine(msg);
            writer.Flush();

        }

        private void CleanGameBoard()
        {
            Dispatcher.Invoke(() => {
                boxName0.Content = "";
                boxName1.Content = "";
                boxName2.Content = "";
                boxName3.Content = "";
                boxName4.Content = "";
                boxName5.Content = "";
                boxName6.Content = "";
                boxName7.Content = "";
                boxName8.Content = "";
            });


        }
        private void CleanAll()
        {
            Dispatcher.Invoke(() => {
                boxName0.Content = "";
                boxName1.Content = "";
                boxName2.Content = "";
                boxName3.Content = "";
                boxName4.Content = "";
                boxName5.Content = "";
                boxName6.Content = "";
                boxName7.Content = "";
                boxName8.Content = "";
                tictactoe.Clear();
                boxes.Clear();
                ResetGameButton.Visibility = Visibility.Hidden;
                winnerTxt.Visibility = Visibility.Hidden;
                drawTxt.Visibility = Visibility.Hidden;
            });
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter writer = new(client.GetStream());
            writer.WriteLine(GameMessageType.TicTacToeReset);
            writer.Flush();

        }
    }
}
