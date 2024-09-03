using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using static ConsoleApp6.Program;

namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Player p1 = Player.NewPlayer();
            Player p2 = Player.NewPlayer(p1.Symbol == "X" ? "O" : "X");

            Console.WriteLine($"\nHej {p1.Name} och {p2.Name}.\nVälkommen till Tic Tac Toe");
            Console.WriteLine($"\n{p1.Name} spelar med symbolen {p1.Symbol}. {p2.Name} spelar med symbolen {p2.Symbol}");
            Console.WriteLine("För att spela matar ni in en siffra mellan 1-9 som representerar positionerna på brädan\n\n");
            Game game = new Game(p1, p2);
            game.playGame();
            Console.ReadKey();

        }


        public class Board
        {
            private string[] _board;

            public Board()
            {
                _board = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            }

            int[][] winCondition = new int[][]
            {
                new int [] {0, 1, 2},
                new int [] {3, 4, 5},
                new int [] {6, 7, 8},
                new int [] {0, 3, 6},
                new int [] {1, 4, 7},
                new int [] {2, 5, 8},
                new int [] {0, 4 ,8},
                new int [] {2, 4, 6},
            };

            public bool GameOver(Player player1, Player player2)
            {
                foreach (int[] combination in winCondition)
                {

                    {
                        string combinations = _board[combination[0]] + _board[combination[1]] + _board[combination[2]];
                        bool allSame = combinations.All(c => c == combinations[0]);

                        if (allSame)
                        {
                            if (_board[combination[0]] == player1.Symbol)
                            {
                                Console.WriteLine($"\n\n{player1.Name} wins the game!");
                                Console.WriteLine("Here is the result!");
                                DisplayBoard();
                                return true;
                            }
                            else
                            {
                                Console.WriteLine($"\n\n{player2.Name} wins the game!");
                                Console.WriteLine("Here is the result!");
                                DisplayBoard();
                                return true;
                            }
                        }
                    }
                }
                return false;
            }



            public string[] boardState => _board;

            public void DisplayBoard()
            {
                for (int i = 0; i < _board.Length; i++)
                {
                    Console.Write($" {_board[i]} ");

                    if ((i + 1) % 3 == 0)
                    {
                        Console.WriteLine("");
                    }
                }
            }

            public void BoardUpdate(int input, string symbol)
            {
                _board[input - 1] = symbol;

            }

            public bool BoardFull()
            {
                return _board.All(position => position == "X" || position == "O");
            }

        }

        public class Game
        {
            private Player _player1;
            private Player _player2;
            private Board _board;
            private bool _currentPlayer1;
            public Game(Player player1, Player player2)

            {
                _player1 = player1;
                _player2 = player2;
                _board = new Board();
                _currentPlayer1 = false;
            }

            public void playGame()
            {

                bool gameOn = true;
                while (gameOn)
                {
                    PlayerChoice(switchPlayer());
                    if (_board.GameOver(_player1, _player2))
                    {
                        gameOn = false;
                    }
                    if (_board.BoardFull())
                    {
                        _board.DisplayBoard();
                        Console.WriteLine("Spelet är över, ni är sämst, ingen vann");
                        gameOn = false;
                        break;
                    }
                }
            }


            public Player switchPlayer()
            {
                _currentPlayer1 = !_currentPlayer1;
                return _currentPlayer1 ? _player1 : _player2;
            }

            public void PlayerChoice(Player player)
            {


                _board.DisplayBoard();
                Console.WriteLine($"\nDet är {player.Name}'s tur att spela. Du spelar med symbolen {player.Symbol}");
                Console.WriteLine("Fyll i en ledig position med din symbol");

                bool correctChoice = false;
                int input = 0;

                while (correctChoice == false)
                {

                    if (Int32.TryParse(Console.ReadLine(), out input))
                    {
                        if (input >= 1 && input <= 9 && _board.boardState[input - 1] != "X" && _board.boardState[input - 1] != "O")
                        {
                            correctChoice = true;
                        }
                        else
                        {
                            Console.WriteLine("Ogiltigt val, välj en siffra mellan 1-9 samt en position som ej är upptagen");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ogiltigt val, vänligen välj en siffra mellan 1-9");
                    }
                }

                _board.BoardUpdate(input, player.Symbol);
            }

        }

        public class Player
        {
            private string _name;
            private string _symbol;
            private int _playerScore;


            public string Name
            {
                get { return _name; }
                set { _name = value; }

            }

            public string Symbol
            {
                get { return _symbol; }
                set { _symbol = value; }
            }

            public int PlayerScore
            {
                get { return _playerScore; }
                set { _playerScore = value; }

            }

            public Player(string playerName, string playerSymbol)
            {
                _name = playerName;
                _symbol = playerSymbol;

            }


            public static Player NewPlayer(string playerOneSymbol = null)
            {

                Console.WriteLine(playerOneSymbol == null ? "Skriv in namnet för spelare 1" : "Skriv in namnet för spelare 2");
                string playerName = Console.ReadLine();

                string playerSymbol;

                if (playerOneSymbol == null)
                {
                    Console.WriteLine($"Välj vilken symbol {playerName} ska vara, välj mellan X eller O");
                    playerSymbol = Console.ReadLine().ToUpper();

                    while (playerSymbol != "X" && playerSymbol != "O")
                    {
                        Console.WriteLine("Fel val av symbol, vänligen välj mellan X eller O");
                        playerSymbol = Console.ReadLine().ToUpper();
                    }
                }
                else
                {
                    playerSymbol = playerOneSymbol;
                }



                return new Player(playerName, playerSymbol);


            }


        }

    }
}
