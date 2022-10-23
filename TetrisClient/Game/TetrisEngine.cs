using System;
using System.Windows.Threading;

namespace TetrisClient.Game
{
    public class TetrisEngine
    {
        public Tetromino Tetromino;
        public Tetromino NextTetromino;
        public Score Score;
        public DispatcherTimer GameTimer;
        public Board Board;
        private Random _random;
        
        /// <summary>
        /// starts the game, creates a new board, creates a new tetromino and starts the game timer
        /// </summary>
        /// <param name="random"></param>
        public void StartGame(Random random)
        {
            Tetromino = new Tetromino(4, 0, random);
            Score = new Score();
            _random = random;
            GameUpdater();
            SetNextTetromino();
            Board = new Board();
        }

        /// <summary>
        /// in charge of the game speed every 1000ms (1s) the tetromino wil move down
        /// </summary>
        public void GameUpdater()
        {
            GameTimer = new DispatcherTimer();
            GameTimer.Tick += MoveDown;
            GameTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            GameTimer.Start();
        }
        
        /// <summary>
        /// reset the game and include variables part of the game
        /// </summary>
        public void RestartGame(Random random)
        {
            GameTimer.Stop();
            Tetromino = new Tetromino(4, 0, random);
            Score = new Score();
            GameUpdater();
            SetNextTetromino();
            Board = new Board();
        }

        /// <summary>
        /// checks if the tetromino can move down if it can it will move down
        /// </summary>
        public void MoveDown(object sender, EventArgs e)
        {
            if (Board.CollisionCheck(Tetromino, offsety: 1)) Collision();
            else Tetromino.offsetY ++;
        }

        /// <summary>
        ///  checks if the tetromino can move right if it can it will move right
        /// </summary>
        public void MoveRight()
        {
            if(Board.FitInBoard(Tetromino, offsetx: 1) && !Board.CollisionCheck(Tetromino, offsetx: 1, offsety: 1))
            {
                Tetromino.offsetX ++;
            }
        }
        /// <summary>
        ///  checks if the tetromino can move left if it can it will move left
        /// </summary>
        public void MoveLeft()
        {
            if(Board.FitInBoard(Tetromino, offsetx: -1) && !Board.CollisionCheck(Tetromino, offsetx: -1, offsety: 1)) Tetromino.offsetX--;
        }
        /// <summary>
        ///  checks if the tetromino can move rotate if it can it will move rotate to the chosen direction
        /// </summary>
        public void HandleRotation(string type)
        {
            if (Board.PossibleRotate(Tetromino, type)) Tetromino.turn(type);
        }

        /// <summary>
        /// creates a tetremino with the shape and rotation of the current tetromino and places it on the lowest possible place.
        /// </summary>
        public Tetromino GhostPiece()
        {
            Tetromino ghostPiece = new Tetromino(Tetromino.offsetX, Tetromino.offsetY, new Random());
            ghostPiece.shapeMatrix = Tetromino.shapeMatrix;
            while (!Board.CollisionCheck(ghostPiece, offsety: 1)) ghostPiece.offsetY++;
            return ghostPiece;
        }

        /// <summary>
        /// hard drop the tetromino making use of the soft drop
        /// hard drop equals 2 point per square drop thats why the score is added twice
        /// on ein the hard drop and one in the soft drop
        /// </summary>
        public void HardDrop()
        {
            while (SoftDrop()) Score.AddPoint();
        }
        /// <summary>
        /// checks if the tetromino can move down if it can it will move down
        /// points are also added to the score
        /// </summary>
        public bool SoftDrop()
        {
            if (Board.CollisionCheck(Tetromino, offsety: 1)) Collision();
            else if (Board.FitInBoard(Tetromino, offsety:1))
            {
                Score.AddPoint();
                Tetromino.offsetY++;
                return true;
            }
            return false;

        }

        /// <summary>
        /// when the is a collision the tetromino is placed on the board and the next tetromino is set
        /// if there is an full row, that row is removed and the score is added
        /// </summary>
        public void Collision()
        {
            Board.AddTetrominoToBoard(Tetromino);
            var rowsDeleted = Board.RemoveFullRow();
            if (rowsDeleted > 0) Score.RowsPoints(rowsDeleted);
            Tetromino = NextTetromino;
            SetNextTetromino();
        }
        
        /// <summary>
        /// next tetromino is set to a random tetromino
        /// </summary>
        public void SetNextTetromino()
        {
            NextTetromino = new Tetromino(4, 0, _random);
            
        }
        public void TogglePause() => GameTimer.IsEnabled = !GameTimer.IsEnabled;
        
        /// <summary>
        /// checks if the game is over and if it is it stops the game
        /// </summary>
        public bool IsGameOver()
        {
            if (Board.CollisionCheck(Tetromino, offsety: 1)&&Tetromino.offsetY==0)
            {
                GameTimer.Stop();
                return true;
            }
            return false;
        }
    }
}