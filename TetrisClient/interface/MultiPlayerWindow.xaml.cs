using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.AspNetCore.SignalR.Client;
using TetrisClient.Game;

namespace TetrisClient.@interface
{
    public partial class MultiPlayerWindow : Window
    {
        private readonly HubConnection _connection;
        private bool _readyP1;
        private bool _alreadyStarted;

        private DispatcherTimer _frontEndTimer;
        private readonly TetrisEngine _tetrisEngineP1 = new();
        private readonly TetrisEngine _tetrisEngineP2 = new();
        
        
        public MultiPlayerWindow()
        {
            InitializeComponent();
            

            // De url waar de meegeleverde TetrisHub op draait:
            string url = "http://127.0.0.1:5000/TetrisHub"; 
            
            // De Builder waarmee de connectie aangemaakt wordt:
            _connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();
            
            // De eerste paramater moet gelijk zijn met de methodenaam in TetrisHub.cs
            // Wat er tussen de <..> staat bepaald wat de type van de paramater `seed` is.
            // Op deze manier loopt het onderstaande gelijk met de methode in TetrisHub.cs.
            _connection.On<int>("ReadyUp", seed =>
            {
                // Seed van de andere client:
                if (_readyP1) // Als de andere client al klaar is:
                {
                    Dispatcher.Invoke(() => GameStart(seed)); // Start de game.
                    SendGameStart(seed); // Stuur de seed naar de andere client.
                }
            });
            
            _connection.On<string>("MoveShape", direction =>
            {
                if (direction == "LEFT") Dispatcher.Invoke(() => _tetrisEngineP2.MoveLeft()); // if the other player moves left, move the shape left
                else if (direction == "RIGHT") Dispatcher.Invoke(() => _tetrisEngineP2.MoveRight()); // if the other player moves right, move the shape right
            });
            _connection.On<string>("RotateShape", direction => Dispatcher.Invoke(() => _tetrisEngineP2.HandleRotation(direction))); // if the other player rotates, rotate the shape    
            
            _connection.On<bool>("DropShape", state => Dispatcher.Invoke(() => _tetrisEngineP2.SoftDrop())); // if the other player drops, drop the shape
            
            _connection.On<bool>("HardDrop", state => Dispatcher.Invoke(() => _tetrisEngineP2.HardDrop())); // if the other player hard drops, hard drop the shape
            
            _connection.On<int>("StartGame", seed => Dispatcher.Invoke(() => GameStart(seed))); // if the other player starts the game, start the game
           
            
            // Let op: het starten van de connectie moet *nadat* alle event listeners zijn gezet!
            // Als de methode waarin dit voorkomt al `async` (asynchroon) is, dan kan `Task.Run` weggehaald worden.
            // In het startersproject staat dit in de constructor, daarom is dit echter wel nodig:
            Task.Run(async () => await _connection.StartAsync());
        }

        // Events kunnen `async` zijn in WPF:
        private async void StartGame_OnClick(object sender, RoutedEventArgs e)
        {
            // Als de connectie nog niet is geïnitialiseerd, dan kan er nog niks verstuurd worden:
            if (_connection.State != HubConnectionState.Connected)
            {
                return;
            }
            Button startGameButton = (Button) sender;
            startGameButton.IsEnabled = false; // De knop wordt niet meer gebruikt.
            startGameButton.Content = "Ready"; // De knop krijgt de naam ready
            
            int seed = Guid.NewGuid().GetHashCode();
            // Het aanroepen van de TetrisHub.cs methode `ReadyUp`.
            // Hier geven we de int mee die de methode `ReadyUp` verwacht.
            await _connection.InvokeAsync("ReadyUp", seed); 
            _readyP1 = true; // jij staat nu klaar.
        }

        private void SendGameStart(int seed)
        {
            if (_connection.State != HubConnectionState.Connected) return;
                // Het aanroepen van de TetrisHub.cs methode `MoveShape`.
            // Hier geven we de string mee die de methode `MoveShape` verwacht.
            _connection.InvokeAsync("StartGame", seed);
        }

        private void SendMovement(string direction)
        {
            if (_connection.State != HubConnectionState.Connected) return;
            // Het aanroepen van de TetrisHub.cs methode `MoveShape`.
            // Hier geven we de string mee die de methode `MoveShape` verwacht.
            _connection.InvokeAsync("MoveShape", direction);
        }
        
        private void SendRotation(string direction)
        {
            if (_connection.State != HubConnectionState.Connected) return;
            // Het aanroepen van de TetrisHub.cs methode `RotateShape`.
            // Hier geven we de string mee die de methode `RotateShape` verwacht.
            _connection.InvokeAsync("RotateShape", direction);
        }
        
        private void SendDrop()
        {
            if (_connection.State != HubConnectionState.Connected) return;
            // Het aanroepen van de TetrisHub.cs methode `DropShape`.
            // Hier geven we geen mee wat de methode `DropShape` verwacht.
            _connection.InvokeAsync("DropShape", true);
        }
        
        private void HardDrop()
        {
            if (_connection.State != HubConnectionState.Connected) return;
            // Het aanroepen van de TetrisHub.cs methode `HardDrop`.
            // Hier geven we geen mee wat de methode `HardDrop` verwacht.
            _connection.InvokeAsync("HardDrop", true);
        }
        
        private void Quit(object sender, RoutedEventArgs routedEventArgs) => Application.Current.Shutdown();

        /// <summary>
        /// start the game if there is a second player,
        /// restart the game if the game is already started.
        /// </summary>
        /// <param name="random"></param>
        public void GameStart(int random)
        {
            _readyP1 = false;
            if (_alreadyStarted)
            {
                Restart(random);
            }
            else
            {
                _tetrisEngineP1.StartGame(new Random(random));
                _tetrisEngineP2.StartGame(new Random(random));
                Timer();
                RenderGrid();
                _alreadyStarted = true;    
            }
        }
        
        /// <summary>
        /// start the game timer
        /// </summary>
        private void Timer()
        {
            _frontEndTimer = new DispatcherTimer();
            _frontEndTimer.Tick += dispatcherTimer_Tick;
            _frontEndTimer.Interval = _tetrisEngineP1.GameTimer.Interval;
            _frontEndTimer.Start();
        }
        
        
        private void dispatcherTimer_Tick(object sender, EventArgs e) => UpdateGame();

        /// <summary>
        /// executes the required actions when button is pressed.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Non in-game actions related keyboard controls
            switch (e.Key)
            {
                case Key.Escape:
                    Quit(null, null);
                    break;
            }
            
            if (!_frontEndTimer.IsEnabled) return;
            if (!_tetrisEngineP1.GameTimer.IsEnabled) return;

            // In-game actions
            switch (e.Key)
            {
                case Key.Right:                                      
                    _tetrisEngineP1.MoveRight();
                    SendMovement("RIGHT");
                    break;
                case Key.Left:                   
                    _tetrisEngineP1.MoveLeft();
                    SendMovement("LEFT");
                    break;
                case Key.Up:
                    _tetrisEngineP1.HandleRotation("UP");
                    SendRotation("UP");
                    break;
                case Key.Down:
                    _tetrisEngineP1.HandleRotation("DOWN");
                    SendRotation("DOWN");
                    break;
                case Key.Space:
                    _tetrisEngineP1.HardDrop();
                    HardDrop();
                    break;
                case Key.LeftShift:
                    _tetrisEngineP1.SoftDrop();
                    SendDrop();
                    break;
                default:
                    return;
            }
            UpdateGame();
        }
        
        
        /// <summary>
        /// Updates the game state and renders the grid.
        /// checks if the game is over.
        /// </summary>
        private void UpdateGame()
        {
            LevelTextBlockP1.Text = $"{_tetrisEngineP1.Score.Level}";
            PointsTextBlockP1.Text = $"{_tetrisEngineP1.Score.Points}";
            LinesTextBlockP1.Text = $"{_tetrisEngineP1.Score.Lines}";
            
            LevelTextBlockP2.Text = $"{_tetrisEngineP2.Score.Level}";
            PointsTextBlockP2.Text = $"{_tetrisEngineP2.Score.Points}";
            LinesTextBlockP2.Text = $"{_tetrisEngineP2.Score.Lines}";
            
            RenderGrid();
            GameOver();
        }
        
        /// <summary>
        ///  renders everything on the two grids.
        /// </summary>
        private void RenderGrid()
        {
            TetrisGridP1.Children.Clear(); //clear the first grid
            RenderBoard(_tetrisEngineP1, TetrisGridP1); //render the first grid
            
            TetrisGridP2.Children.Clear(); //clear the second grid
            RenderBoard(_tetrisEngineP2, TetrisGridP2); //render the second grid
            
            RenderTetromino(_tetrisEngineP1, TetrisGridP1); //render the current tetromino of the first grid
            RenderTetromino(_tetrisEngineP2, TetrisGridP2); //render the current tetromino of the second grid
            
            NextGridP1.Children.Clear();
            RenderNextTetromino(_tetrisEngineP1, NextGridP1); //render the next tetromino of the first grid
            NextGridP2.Children.Clear();
            RenderNextTetromino(_tetrisEngineP2, NextGridP2); //render the next tetromino of the second grid
            
            GhostPiece();
        }
        
        /// <summary>
        /// renders the board on the selected grid.
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="grid"></param>
        public void RenderBoard(TetrisEngine engine, Panel grid)
        {
            var board = engine.Board.BoardArray;
            for (int y = 0; y < board.GetLength(0); y++)
            for (int x = 0; x < board.GetLength(1); x++)
            {
                if (board[y, x] == 0) continue;
                Rectangle rectangle = CreateRectangle(ConvertNumberToBrush(board[y, x]),1);
                grid.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
                Grid.SetRow(rectangle, y); // Zet de rij
                Grid.SetColumn(rectangle, x); // Zet de kolom
            }
        }

        /// <summary>
        /// renders current tetromino on the selected grid.
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="grid"></param>
        private void RenderTetromino(TetrisEngine engine, Panel grid)
        {
            var values = engine.Tetromino.shapeMatrix.Value;
            for (var i = 0; i < values.GetLength(0); i++)
            for (var j = 0; j < values.GetLength(1); j++)
            {
                // Als de waarde niet gelijk is aan 1,
                // dan hoeft die niet getekent te worden:
                if (values[i, j] == 0) continue;
                Rectangle rectangle = CreateRectangle(ConvertNumberToBrush(values[i, j]),1);
                grid.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
                Grid.SetRow(rectangle, i + engine.Tetromino.offsetY); // Zet de rij
                Grid.SetColumn(rectangle, j + engine.Tetromino.offsetX); // Zet de kolom
            }
        }

        /// <summary>
        /// places the ghost piece in the grid.
        /// </summary>
        public void GhostPiece()
        {
            Tetromino ghostPiece = _tetrisEngineP1.GhostPiece();
            Matrix matrix = ghostPiece.shapeMatrix;
        
            int[,] values = matrix.Value;
            for (int i = 0; i < values.GetLength(0); i++)
            for (int j = 0; j < values.GetLength(1); j++)
            { 
                // Als de waarde niet gelijk is aan 1
                // dan hoeft die niet getekent te worden:
                if (values[i, j] == 0) continue;
                Rectangle rectangle = CreateRectangle(ConvertNumberToBrush(8), 0.25);
                TetrisGridP1.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
                Grid.SetRow(rectangle, i + ghostPiece.offsetY); // Zet de rij
                Grid.SetColumn(rectangle, j + ghostPiece.offsetX); // Zet de kolom
            }
        }
        
        /// <summary>
        /// renders the next tetromino on the selected grid.
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="grid"></param>
        public void RenderNextTetromino(TetrisEngine engine, Panel grid)
        {
            int[,] values = engine.NextTetromino.shapeMatrix.Value;
            for (int i = 0; i < values.GetLength(0); i++)
            for (int j = 0; j < values.GetLength(1); j++) 
            {
                // Als de waarde niet gelijk is aan 1,
                // dan hoeft die niet getekent te worden:
                if (values[i, j] == 0) continue;
                Rectangle rectangle = CreateRectangle(ConvertNumberToBrush(values[i, j]),1);
                grid.Children.Add(rectangle);
                Grid.SetRow(rectangle, i); // Zet de rij
                Grid.SetColumn(rectangle, j); // Zet de kolom
            }
        }
        /// <summary>
        /// used for creating a rectangle including the brush and the opacity
        /// </summary>
        private Rectangle CreateRectangle(Brush color, double opacity)
        {
            return new Rectangle()
            {
                Width = 25, // Breedte van een 'cell' in de Grid
                Height = 25, // Hoogte van een 'cell' in de Grid
                Stroke = new SolidColorBrush(Color.FromRgb(15,56,15)), // De rand
                StrokeThickness = 1, // Dikte van de rand
                Fill = color, // Achtergrondkleur
                Opacity = opacity
            };
        }
        
        /// <summary>
        /// shows the game over screen when the game is over.
        /// ends the game timer.
        /// </summary>
        private void GameOver()
        {
            if (_tetrisEngineP1.IsGameOver())
            {
                if ( GameOverTextP1.Visibility != Visibility.Visible)
                {
                    GameOverTextP1.Visibility = Visibility.Visible;
                    StartGame.Content = "replay";
                    StartGame.IsEnabled = true;
                }
            }
            if (_tetrisEngineP2.IsGameOver()) GameOverTextP2.Visibility = Visibility.Visible;
        }
        
        /// <summary>
        /// resets the game when the replay button is clicked.
        /// </summary>
        /// <param name="seed"></param>
        private void Restart(int seed)
        {
            GameOverTextP1.Visibility = Visibility.Hidden;
            _tetrisEngineP1.RestartGame(new Random(seed));
            GameOverTextP2.Visibility = Visibility.Hidden;
            _tetrisEngineP2.RestartGame(new Random(seed));
            Timer();
            RenderGrid();
            UpdateGame();
        }

        /// <summary>
        /// depending on the number it returns the color the shape by en used for rendering on the board
        /// </summary>
        private static Brush ConvertNumberToBrush(int num)
        {
            return num switch
            {
                1 => Tetromino.DetermineColor(TetrominoShape.O),
                2 => Tetromino.DetermineColor(TetrominoShape.T),
                3 => Tetromino.DetermineColor(TetrominoShape.J),
                4 => Tetromino.DetermineColor(TetrominoShape.L),
                5 => Tetromino.DetermineColor(TetrominoShape.S),
                6 => Tetromino.DetermineColor(TetrominoShape.Z),
                7 => Tetromino.DetermineColor(TetrominoShape.I),
                8 => Tetromino.DetermineColor(TetrominoShape.Ghost),
                _ => throw new ArgumentOutOfRangeException(nameof(num), num, null)
            };
        }
        
    }
}
