using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IUserInterface ui = new InteractiveUI();
            
            Player p1 = Player.NewPlayer(ui);
            Player p2 = Player.NewPlayer(ui, p1.Symbol == "X" ? "O" : "X");

            ui.DisplayMessage($"\nHej {p1.Name} och {p2.Name}.\nVälkommen till Tic Tac Toe");
            ui.DisplayMessage($"\n{p1.Name} spelar med symbolen {p1.Symbol}. {p2.Name} spelar med symbolen {p2.Symbol}");
            ui.DisplayMessage("För att spela matar ni in en siffra mellan 1-9 som representerar positionerna på brädan\n\n");
            Game game = new Game(p1, p2, ui);
            game.playGame();
            Console.ReadKey();
        }
    }

    public interface IUserInterface
    {
        void DisplayMessage(string message);
        string GetUserInput();

    }

    public class InteractiveUI : IUserInterface 
    {
        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public string GetUserInput()
        {
            return Console.ReadLine();
        }
    }

    public class Board
    {
        private string[] _board;
        private IUserInterface _ui;

        public Board(IUserInterface ui)
        {
            _board = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            _ui = ui;
        }

        public string GetPosition(int input)
        {
            return _board[input];
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
                string combinations = _board[combination[0]] + _board[combination[1]] + _board[combination[2]];
                bool allSame = combinations.All(c => c == combinations[0]);

                if (allSame)
                {
                    if (_board[combination[0]] == player1.Symbol)
                    {
                        _ui.DisplayMessage($"\n\n{player1.Name} wins the game!");
                        _ui.DisplayMessage("Here is the result!");
                        DisplayBoard();
                        return true;
                    }
                    else
                    {
                        _ui.DisplayMessage($"\n\n{player2.Name} wins the game!");
                        _ui.DisplayMessage("Here is the result!");
                        DisplayBoard();
                        return true;
                    }
                }
            }
            return false;
        }
        public void DisplayBoard()
        {
            _ui.DisplayMessage($"{_board[0]} | {_board[1]} | {_board[2]}");
            _ui.DisplayMessage("----------");
            _ui.DisplayMessage($"{_board[3]} | {_board[4]} | {_board[5]}");
            _ui.DisplayMessage("----------");
            _ui.DisplayMessage($"{_board[6]} | {_board[7]} | {_board[8]}");
            _ui.DisplayMessage("");  // Print a blank line after the board
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
        private IUserInterface _ui;

        public Game(Player player1, Player player2, IUserInterface ui)
        {
            _player1 = player1;
            _player2 = player2;
            _board = new Board(ui);
            _currentPlayer1 = false;
            _ui = ui;
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
                    _ui.DisplayMessage("Spelet är över, ni är sämst, ingen vann");
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
            _ui.DisplayMessage($"\nDet är {player.Name}'s tur att spela. Du spelar med symbolen {player.Symbol}");
            _ui.DisplayMessage("Fyll i en ledig position med din symbol");

            bool correctChoice = false;
            int input = 0;

            while (!correctChoice)
            {
                if (Int32.TryParse(_ui.GetUserInput(), out input))
                {
                    if (input >= 1 && input <= 9 && _board.GetPosition(input - 1) != "X" && _board.GetPosition(input - 1) != "O")
                    {
                        correctChoice = true;
                    }
                    else
                    {
                        _ui.DisplayMessage("Ogiltigt val, välj en siffra mellan 1-9 samt en position som ej är upptagen");
                    }
                }
                else
                {
                    _ui.DisplayMessage("Ogiltigt val, vänligen välj en siffra mellan 1-9");
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
        private IUserInterface _ui;

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

        public Player(string playerName, string playerSymbol, IUserInterface ui)
        {
            _name = playerName;
            _symbol = playerSymbol;
            _ui = ui;
        }

        public static Player NewPlayer(IUserInterface ui, string playerOneSymbol = null)
        {
            ui.DisplayMessage(playerOneSymbol == null ? "Skriv in namnet för spelare 1" : "Skriv in namnet för spelare 2");
            string playerName = ui.GetUserInput();

            string playerSymbol;

            if (playerOneSymbol == null)
            {
                ui.DisplayMessage($"Välj vilken symbol {playerName} ska vara, välj mellan X eller O");
                playerSymbol = ui.GetUserInput().ToUpper();

                while (playerSymbol != "X" && playerSymbol != "O")
                {
                    ui.DisplayMessage("Fel val av symbol, vänligen välj mellan X eller O");
                    playerSymbol = ui.GetUserInput().ToUpper();
                }
            }
            else
            {
                playerSymbol = playerOneSymbol;
            }

            return new Player(playerName, playerSymbol, ui);
        }
    }
}