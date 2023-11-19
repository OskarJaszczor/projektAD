// See https://aka.ms/new-console-template for more information
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;

namespace chatWatki
{
    class Game
    {
        public ObservableCollection<Message> messages = new ObservableCollection<Message>();
        public ObservableCollection<TicTacToe> tictactoe = new ObservableCollection<TicTacToe>();
        public ObservableCollection<Players> players = new ObservableCollection<Players>();
        public List<TcpClient> clients = new();
        public bool game_over = false;
        public bool gameFlag = true;

        public bool IsValidMove(int move, bool flag)
        {
            if (game_over != true)
            {
                foreach (var e in tictactoe)
                {
                    if (e.Clicked == move)
                    {
                        return false;
                    }
                }

                if (IsGameWon(flag) != true)
                    if (IsGameTie(flag) != true)
                        return true;
            }

            return false;
        }

        public bool IsGameWon(bool flag)
        {
            int[] board = new int[9];
            foreach (var ttt in tictactoe)
            {
                if (ttt.Character == flag)
                {
                    board[ttt.Clicked] = flag ? 1 : -1;
                }
            }

            int[][] winningCombinations = new int[][]
            {
                new int[] {0, 1, 2}, // Pozioma linia górna
                new int[] {3, 4, 5}, // Pozioma linia środkowa
                new int[] {6, 7, 8}, // Pozioma linia dolna
                new int[] {0, 3, 6}, // Pionowa linia lewa
                new int[] {1, 4, 7}, // Pionowa linia środkowa
                new int[] {2, 5, 8}, // Pionowa linia prawa
                new int[] {0, 4, 8}, // Przekątna lewa-górna do prawego-dolnego
                new int[] {2, 4, 6}  // Przekątna prawa-górna do lewa-dolnego
            };

            foreach (var combo in winningCombinations)
            {
                int sum = board[combo[0]] + board[combo[1]] + board[combo[2]];
                if (sum == 3 || sum == -3)
                {
                    game_over = true;
                    return true;
                }
            }

            return false;
        }

        public bool IsGameTie(bool flag)
        {
            int counter = 0;
            foreach (var e in tictactoe)
            {
                counter++;
            }
            if (counter == 9)
            {
                if (IsGameWon(flag) == false)
                {
                    game_over = true;
                    return true;
                }
            }
            return false;
        }

        public void sendWin(bool flag)
        {
            foreach (TcpClient client in clients)
            {
                StreamWriter writer = new(client.GetStream());
                writer.WriteLine(GameMessageType.TicTacToeWin + "\0" + flag);
                writer.Flush();
            }
        }
        public void sendDraw(bool flag)
        {
            foreach (TcpClient client in clients)
            {
                StreamWriter writer = new(client.GetStream());
                writer.WriteLine(GameMessageType.TicTacToeDraw);
                writer.Flush();
            }
        }
        public void resetBoard()
        {
            foreach (TcpClient client in clients)
            {
                StreamWriter writer = new(client.GetStream());
                writer.WriteLine(GameMessageType.TicTacToeReset);
                writer.Flush();
            }
        }


        private void startMessagesThread()
        {
            messages.CollectionChanged += (sender, e) =>
            {
                if (e.Action != NotifyCollectionChangedAction.Add) return;
                var newMessages = e.NewItems;

                Thread t = new(() =>
                {
                    foreach (TcpClient client in clients)
                    {
                        StreamWriter writer = new StreamWriter(client.GetStream());
                        foreach (Message msg in newMessages)
                            writer.WriteLine(GameMessageType.Chat + "\0" + msg.User + "\0" + msg.Msg);
                        writer.Flush();
                    }
                });
                t.Start();
            };
        }
        private void startTicTacToeThread()
        {
            tictactoe.CollectionChanged += (sender, e) =>
            {
                if (e.Action != NotifyCollectionChangedAction.Add) return;
                var newMessages = e.NewItems;

                Thread t = new(() =>
                {
                    foreach (TcpClient client in clients)
                    {
                        StreamWriter writer = new StreamWriter(client.GetStream());
                        foreach (TicTacToe ttt in newMessages)
                            writer.WriteLine(GameMessageType.TicTacToeMove + "\0" + ttt.Clicked + "\0" + ttt.Character);
                        writer.Flush();
                    }
                });
                t.Start();
            };
        }
        private void startPlayersThread()
        {

            players.CollectionChanged += (sender, e) =>
            {
                Thread t = new Thread(() =>
                {
                    foreach (Players player in players)
                    {
                        Console.WriteLine(player.nickname + ":" + player.ip);
                    }
                    foreach (TcpClient client in clients)
                    {
                        StreamWriter writer = new(client.GetStream());
                        foreach (Players player in players)
                        {
                            writer.WriteLine(GameMessageType.IpAdressNick + "\0" + player.nickname + "\0" + player.ip);

                        }
                    }

                });
                t.Start();
            };

        }
        private void startPlayerCountThread()
        {/*

            //watek do monitorowania ilosci graczy na serverze

            Thread ActivePlayers = new(() =>
            {
                foreach (Players player in players)
                {
                    //Pomysly: 1. klienci przesylaja wiadomosci (jakis byte). Sprawdzam czy kazdy to robi, jesli jakis nie to znaczy ze sie rozlaczyl
                    // 2. foreachem wysylam byte doi klientow co sekunde, kazdy z nich go odczytuje i odsyla to samo
                }
            });

            ActivePlayers.Start();

            */
        }

        public void Start()
        {
            new Thread(() => {
                startMessagesThread();
                startTicTacToeThread();
                startPlayersThread();
                startPlayerCountThread();
            }).Start();

        }

        public void addClient(TcpClient client)
        {
            clients.Add(client);

            Thread t = new Thread(() =>
            {
                StreamReader reader = new StreamReader(client.GetStream());
                string data = reader.ReadLine();
                string[] splitted1 = data.Split('\0');
                IPAddress ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                string nickname = null;
                if (splitted1[0] == "Nick")
                {
                    nickname = splitted1[1];

                }
                players.Add(new Players(ip.ToString(), nickname));
                //Console.WriteLine($"Nowe połączenie od {ip.ToString()}"); // tak sie adres IP klienta pobiera 

                while (true)
                {

                    string raw = reader.ReadLine();
                    string[] splitted = raw.Split('\0');
                    switch (splitted[0])
                    {
                        case "Chat":
                            string user = splitted[1],
                            msg = splitted[2];
                            Console.WriteLine($"[{user}] {msg}");
                            messages.Add(new Message(msg, user));
                            break;
                        case "TicTacToeMove":
                            int move = Int32.Parse(splitted[1]);
                            //Console.WriteLine(gameFlag);

                            if (IsValidMove(move, gameFlag) == true)
                            {
                                tictactoe.Add(new TicTacToe(move, gameFlag));
                                Console.WriteLine($"[{move}] {gameFlag}");
                                if (IsGameWon(gameFlag) == true)
                                {

                                    Console.WriteLine("Win" + gameFlag);
                                    tictactoe.Clear();
                                    sendWin(gameFlag);

                                }
                                if (IsGameTie(gameFlag) == true)
                                {
                                    Console.WriteLine("Draw" + gameFlag);
                                    tictactoe.Clear();
                                    sendDraw(gameFlag);
                                }
                                gameFlag = !gameFlag;
                            }
                            break;
                        case "TicTacToeReset":
                            resetBoard();
                            game_over = false;
                            break;
                    }
                }
            });
            t.Start();
        }
    }
}
