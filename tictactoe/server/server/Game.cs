using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using shared;

namespace server
{
    class Game
    {
        public List<Client> game_clients = new List<Client>();
        Board board;
        private int currentPlayerIndex = 0;
        private object lobbyLock = new object();
        private bool gameEnded;
        public List<Leaderboard> Leaderboard { get; private set; } = new List<Leaderboard>();

        public event EventHandler<List<Leaderboard>> LeaderboardUpdated;

        private void OnLeaderboardUpdated(List<Leaderboard> leaderboard)
        {
            LeaderboardUpdated?.Invoke(this, leaderboard);
        }
        public Game() 
        {
            board = new Board();
            gameEnded = false;
            startListeningThread();


        }

        public void startGame()
        {
            foreach (Client client in game_clients)
            {
                string message = Config.GameMessageType.Game.ToString();
                client.sendMessage(message);
                Console.WriteLine(client.client_nickname);
            }
            game_clients[0].character = "O";
            game_clients[1].character = "X";
            game_clients[0].sendMessage(Config.GameMessageType.Character + "\0" + "O");
            game_clients[1].sendMessage(Config.GameMessageType.Character + "\0" + "X");
            currentPlayerIndex = 0;
        }
        private void startListeningThread()
        {
            Thread listeningThread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10);
                    if (game_clients.Count > 0)
                    {
                        lock(lobbyLock)
                        {
                            foreach (Client client in game_clients.ToList())
                            {
                                if (gameEnded == true)
                                    break;
                                string message = client.getPacket();
                                if (message != null)
                                {
                                    //Console.WriteLine(message);
                                    var splitted = message.Split('\0');
                                    switch (splitted[0])
                                    {
                                        case "InGameChat":
                                            //Console.WriteLine(message);
                                            sendMessageToAll(Config.GameMessageType.InGameChat, $"{splitted[1]}\0{client.client_nickname}");
                                            break;
                                        case "Move":
                                            int moveIndex = Int32.Parse(splitted[1]);

                                            // Sprawdź, czy aktualny gracz ma prawo do wykonania ruchu
                                            if (game_clients[currentPlayerIndex] == client)
                                            {
                                                // Sprawdź, czy ruch jest dozwolony
                                                bool validMove = board.PlaceMark(moveIndex, client.character[0]);

                                                if (validMove)
                                                {
                                                    // Zmiana aktualnego gracza na następnego
                                                    if (board.CheckWin(game_clients[currentPlayerIndex].character[0]))
                                                    {
                                                        // Gra zakończona, jest zwycięzca
                                                        Console.WriteLine($"Gracz {game_clients[currentPlayerIndex].client_nickname} wygrał!");
                                                        game_clients[currentPlayerIndex].game_wins++;
                                                        Leaderboard.Add(new Leaderboard(game_clients[currentPlayerIndex].client_nickname, game_clients[currentPlayerIndex].game_wins));
                                                        string data = "";
                                                        foreach(var e in Leaderboard)
                                                        {
                                                            data += e.Nickname + ":" + e.WinGames + ";";
                                                        }
                                                        sendMessageToAll(Config.GameMessageType.Win, $"{client.character}\0{moveIndex}\0{data}");
                                                       
                                                        Thread.Sleep(100);
                                                        endGame();
                                                        break;

                                                        // Dodaj kod obsługi zakończenia gry
                                                    }
                                                    else
                                                    {
                                                        // Sprawdzenie remisu po ruchu gracza, ale tylko jeśli nie ma jeszcze zwycięzcy
                                                        if (board.IsFull())
                                                        {
                                                            // Gra zakończona, remis
                                                            Console.WriteLine("Remis!");
                                                            sendMessageToAll(Config.GameMessageType.Draw, $"{client.character}\0{moveIndex}");
                                                            
                                                            endGame();
                                                            // Dodaj kod obsługi zakończenia gry
                                                        }
                                                        else
                                                        {
                                                            // Przełącz do następnego gracza
                                                            currentPlayerIndex = (currentPlayerIndex + 1) % 2;
                                                        }
                                                    }

                                                    // Wysłanie informacji o ruchu do wszystkich klientów
                                                    Console.WriteLine($"{client.character}\0{moveIndex}");
                                                    sendMessageToAll(Config.GameMessageType.Move, $"{client.character}\0{moveIndex}\0${client.client_nickname}");
                                                }
                                                else
                                                {
                                                    // Wysłanie informacji zwrotnej do klienta, że ruch był nieprawidłowy
                                                    client.sendMessage($"{Config.GameMessageType.InvalidMove}\0Invalid move. Try again.");
                                                }
                                            }
                                            break;
                                    }
                                }
                            }
                        }                     
                        
                    }
                }
            });
            listeningThread.Start();
        }
        private void sendMessageToAll(Config.GameMessageType type, string data)
        {
            foreach (Client client in game_clients)
            {
                string message = type.ToString() + "\0" + data;
               // Console.WriteLine(message);
                client.sendMessage(message);
            }
        }
        private void endGame()
        {
            lock (lobbyLock)
            {
                game_clients[0].readyToPlay = false;
                game_clients[1].readyToPlay = false;
                Lobby.lobby_clients.Add(game_clients[0]);
                Lobby.lobby_clients.Add(game_clients[1]);
                game_clients.Clear();
                
                gameEnded = true;
            }
        }
        private void SendLeaderboard()
        {
            OnLeaderboardUpdated(Leaderboard);
            string message = "";
            foreach(Leaderboard lb in Leaderboard)
            {
                message += lb.Nickname + ":" + lb.WinGames + ";";
            }
            
            foreach (Client client in game_clients)
            {
                client.sendMessage(Config.GameMessageType.LeaderBoard + "\0" + message);
            }

            Console.WriteLine(Config.GameMessageType.LeaderBoard + "\0" + message);
        }
    }
}
