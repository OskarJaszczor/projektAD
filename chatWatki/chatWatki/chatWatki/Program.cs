// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace chatWatki
{
    enum GameMessageType
    {
        Chat,
        TicTacToeMove,
        TicTacToeWin,
        TicTacToeDraw,
        TicTacToeReset
    }

    
    class Program
    {
        private static ObservableCollection<Message> messages = new ObservableCollection<Message>();
        private static ObservableCollection<TicTacToe> tictactoe = new ObservableCollection<TicTacToe>();
        private static List<TcpClient> clients = new();
        public static bool game_over = false;
        private static bool player1Turn = true;
        private static bool player2Turn = false;
        static void Main(string[] args)
        {
            Console.WriteLine("Server start!");

            TcpListener listener = new TcpListener(IPAddress.Any, 9999);
            listener.Start();

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
            bool gameFlag = true;
            while (true)
            {
                int x = messages.Count;
                TcpClient client = listener.AcceptTcpClient();
                clients.Add(client);
                
                Thread t = new Thread(() =>
                {
                    StreamReader reader = new StreamReader(client.GetStream());

                    Console.WriteLine("Client connected!");
                    
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
                                    if(IsGameTie(gameFlag) == true) 
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

        private static bool IsValidMove(int move, bool flag)
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

        private static bool IsGameWon(bool flag)
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

        private static bool IsGameTie(bool flag)
        {
            int counter = 0;
            foreach(var e in tictactoe)
            {
                counter++;
            }
            if(counter == 9)
            {
                if(IsGameWon(flag) == false)
                {
                    game_over = true;
                    return true;
                }
            }
            return false;
        }

        private static void sendWin(bool flag)
        {
            foreach(TcpClient client in clients)
            {
                StreamWriter writer = new(client.GetStream());
                writer.WriteLine(GameMessageType.TicTacToeWin + "\0" + flag);
                writer.Flush();
            }
        }
        private static void sendDraw(bool flag)
        {
            foreach (TcpClient client in clients)
            {
                StreamWriter writer = new(client.GetStream());
                writer.WriteLine(GameMessageType.TicTacToeDraw);
                writer.Flush();
            }
        }
        private static void resetBoard()
        {
            foreach (TcpClient client in clients)
            {
                StreamWriter writer = new(client.GetStream());
                writer.WriteLine(GameMessageType.TicTacToeReset);
                writer.Flush();
            }
        }
    }
}
