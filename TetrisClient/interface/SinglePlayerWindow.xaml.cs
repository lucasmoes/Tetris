using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using TetrisClient.Game;

namespace TetrisClient
{
    public partial class SinglePlayerWindow : Window
    {
        private DispatcherTimer _frontEndTimer;
        private readonly TetrisEngine _tetrisEngine = new();
        
        public SinglePlayerWindow()
        {
            InitializeComponent();
            _tetrisEngine.StartGame(new Random(Guid.NewGuid().GetHashCode()));
            Timer();
            RenderGrid();
        }
        
        private void Timer()
        {
            _frontEndTimer = new DispatcherTimer();
            _frontEndTimer.Tick += dispatcherTimer_Tick;
            _frontEndTimer.Interval = _tetrisEngine.GameTimer.Interval;
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
                case Key.P:
                    TogglePause(null, null);
                    break;
                case Key.Escape:
                    Quit(null, null);
                    break;
                case Key.Enter:
                    if (GameOverText.Visibility == Visibility.Visible) Restart(null, null);
                    break;
            }
            
            if (!_frontEndTimer.IsEnabled) return;
            if (!_tetrisEngine.GameTimer.IsEnabled) return;
            
            // In-game actions
            switch (e.Key)
            {
                case Key.Right:                                      
                    _tetrisEngine.MoveRight();
                    break;
                case Key.Left:                   
                    _tetrisEngine.MoveLeft();
                    break;
                case Key.Up:
                    _tetrisEngine.HandleRotation("UP");
                    break;
                case Key.Down:
                    _tetrisEngine.HandleRotation("DOWN");
                    break;
                case Key.Space:
                    _tetrisEngine.HardDrop();
                    break;
                case Key.LeftShift:
                    _tetrisEngine.SoftDrop();
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
            LevelTextBlock.Text = $"{_tetrisEngine.Score.Level}";
            PointsTextBlock.Text = $"{_tetrisEngine.Score.Points}";
            LinesTextBlock.Text = $"{_tetrisEngine.Score.Lines}";

            RenderGrid();
            GameOver();
            
            
        }
        
        /// <summary>
        /// fill in the current tetromino in the grid.
        /// </summary>
        private void RenderGrid()
        {
            TetrisGrid.Children.Clear();
            
            RenderBoard();
            GhostPiece();

            Matrix matrix = _tetrisEngine.Tetromino.shapeMatrix;

            int[,] values = matrix.Value;
            for (int i = 0; i < values.GetLength(0); i++)
            for (int j = 0; j < values.GetLength(1); j++)
            {
                // Als de waarde niet gelijk is aan 1,
                // dan hoeft die niet getekent te worden:
                if (values[i, j] == 0) continue;
                Rectangle rectangle = CreateRectangle(ConvertNumberToBrush(values[i, j]),1);
                TetrisGrid.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
                Grid.SetRow(rectangle, i + _tetrisEngine.Tetromino.offsetY); // Zet de rij
                Grid.SetColumn(rectangle, j + _tetrisEngine.Tetromino.offsetX); // Zet de kolom
            }
            RenderNextTetromino();
        }
        
        /// <summary>
        /// places the ghost piece in the grid.
        /// </summary>
        public void GhostPiece()
        {
            NextGrid.Children.Clear();
            Tetromino ghostPiece = _tetrisEngine.GhostPiece();
            Matrix matrix = ghostPiece.shapeMatrix;

            int[,] values = matrix.Value;
            for (int i = 0; i < values.GetLength(0); i++)
            for (int j = 0; j < values.GetLength(1); j++)
            {
                // Als de waarde niet gelijk is aan 1,
                // dan hoeft die niet getekent te worden:
                if (values[i, j] == 0) continue;
                Rectangle rectangle = CreateRectangle(ConvertNumberToBrush(8), 0.25);
                TetrisGrid.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
                Grid.SetRow(rectangle, i + ghostPiece.offsetY); // Zet de rij
                Grid.SetColumn(rectangle, j + ghostPiece.offsetX); // Zet de kolom
                
            }
        }

        /// <summary>
        /// renders the already placed tetrominoes in the grid.
        /// </summary>
        public void RenderBoard()
        {
            var board = _tetrisEngine.Board.BoardArray;
            for (int y = 0; y < board.GetLength(0); y++)
            for (int x = 0; x < board.GetLength(1); x++)
            {
                if (board[y, x] == 0) continue;
                Rectangle rectangle = CreateRectangle(ConvertNumberToBrush(board[y, x]),1);
                TetrisGrid.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
                Grid.SetRow(rectangle, y); // Zet de rij
                Grid.SetColumn(rectangle, x); // Zet de kolom
            }
        }

        /// <summary>
        /// fills the next tetromino in the grid.
        /// </summary>
        public void RenderNextTetromino()
        {
            NextGrid.Children.Clear();
            Matrix matrix = _tetrisEngine.NextTetromino.shapeMatrix;

            int[,] values = matrix.Value;
            for (int i = 0; i < values.GetLength(0); i++)
            for (int j = 0; j < values.GetLength(1); j++)
            {
                // Als de waarde niet gelijk is aan 1,
                // dan hoeft die niet getekent te worden:
                if (values[i, j] == 0) continue;
                Rectangle rectangle = CreateRectangle(ConvertNumberToBrush(values[i, j]),1);
                NextGrid.Children.Add(rectangle);
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
        
        private void Quit(object sender, RoutedEventArgs routedEventArgs) => Application.Current.Shutdown();
        
        private void TogglePause(object sender, RoutedEventArgs routedEventArgs)
        {
            PauseButton.Content = (string) PauseButton.Content == "Pause" ? "Resume" : "Pause";
            _frontEndTimer.IsEnabled = !_frontEndTimer.IsEnabled;
            _tetrisEngine.TogglePause();
            
        }
        
        /// <summary>
        /// shows the game over screen when the game is over.
        /// ends the game timer.
        /// </summary>
        private void GameOver()
        {
            if (!_tetrisEngine.IsGameOver()) return;
            GameOverText.Visibility = Visibility.Visible;
            _frontEndTimer.IsEnabled = false;
        }

        /// <summary>
        /// restart the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="routedEventArgs"></param>
        private void Restart(object sender, RoutedEventArgs routedEventArgs)
        {
            GameOverText.Visibility = Visibility.Hidden;
            _tetrisEngine.RestartGame(new Random(Guid.NewGuid().GetHashCode()));
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
