using static System.Linq.Enumerable;
using Range = System.Range;


namespace TetrisClient.Game
{
    public class Board
    {
        public readonly int[,] BoardArray;

        public Board() => BoardArray = GenerateEmptyBoard();

        private static int[,] GenerateEmptyBoard()
        {
            return new[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };
        }

        /// <summary>
        /// check if the tetromino can be placed on the board.
        /// </summary>
        /// <param name="tetromino"></param>
        /// <param name="offsetx"></param>
        /// <param name="offsety"></param>
        /// <returns></returns>
        public bool FitInBoard(Tetromino tetromino, int offsetx = 0, int offsety = 0)
        {
            for (var y = 0; y < tetromino.shapeMatrix.Value.GetLength(0); y++)
            for (var x = 0; x < tetromino.shapeMatrix.Value.GetLength(1); x++)
            {
                if (tetromino.shapeMatrix.Value[y, x] == 0) continue;
                if (x + offsetx + tetromino.offsetX < 0 ||
                    x + offsetx + tetromino.offsetX > BoardArray.GetLength(1) - 1 ||
                    y + tetromino.offsetY + offsety > BoardArray.GetLength(0) - 1) return false;
            }

            return true;
        }
        
        /// <summary>
        /// checks if the tetromino can rotate and if it can, rotates it
        /// it can rotate if the new shape fits in the board and if the new shape doesn't collide with the other shapes.
        /// </summary>
        /// <param name="tetromino"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool PossibleRotate(Tetromino tetromino, string type)
        {
            Tetromino tetrominoClone = new Tetromino(tetromino.offsetX, tetromino.offsetY, tetromino.shapeMatrix);
            tetrominoClone.shapeMatrix = type switch
            {
                "UP" => tetrominoClone.shapeMatrix.Rotate90(),
                "DOWN" => tetrominoClone.shapeMatrix.Rotate90CounterClockwise(),
                _ => tetrominoClone.shapeMatrix
            };
            return FitInBoard(tetrominoClone) && !CollisionCheck(tetrominoClone);
        }

        /// <summary>
        /// adds the tetromino to the board.
        /// </summary>
        /// <param name="tetromino"></param>
        /// <param name="offsetx"></param>
        /// <param name="offsety"></param>
        public void AddTetrominoToBoard(Tetromino tetromino, int offsetx = 0, int offsety = 0)
        {
            for (var y = 0; y < tetromino.shapeMatrix.Value.GetLength(0); y++)
                Range(0, tetromino.shapeMatrix.Value.GetLength(1)).ToList().ForEach(x =>
                {
                    if (tetromino.shapeMatrix.Value[y, x] != 0) 
                        BoardArray[y + offsety + tetromino.offsetY, x + offsetx + tetromino.offsetX] = tetromino.shapeMatrix.Value[y, x];
                });
        }

        /// <summary>
        /// checks if the tetromino collides with the board.
        /// </summary>
        /// <param name="tetromino"></param>
        /// <param name="offsetx"></param>
        /// <param name="offsety"></param>
        /// <returns>true if there is an collision</returns>
        public bool CollisionCheck(Tetromino tetromino, int offsetx = 0, int offsety = 0)
        {
            for (var y = 0; y < tetromino.shapeMatrix.Value.GetLength(0); y++)
                
            for (var x = 0; x < tetromino.shapeMatrix.Value.GetLength(1); x++)
            {
                if (tetromino.shapeMatrix.Value[y, x] == 0) continue;
                if (BoardArray[y + tetromino.offsetY + offsety, x + tetromino.offsetX + offsetx] != 0) 
                    return true;
                if (y + tetromino.offsetY + offsety == BoardArray.GetLength(0) - 1) 
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if there is a line full of blocks,
        /// deletes the line and moves all lines above down
        /// </summary>
        /// <returns>amount of removed lines</returns>
        public int RemoveFullRow()
        {
            var counter = 0;
            for (var y = 0; y < BoardArray.GetLength(0); y++)
            {
                var rowIsFull = Range(0, BoardArray.GetLength(1)).Select(x => BoardArray[y, x]).All(x => x != 0);
                if (!rowIsFull) continue;
                counter++;
                for (var k = y; k > 0; k--)
                    Range(0, BoardArray.GetLength(1)).ToList().ForEach(x => BoardArray[k, x] = BoardArray[k - 1, x]);
            }
            Range(0, BoardArray.GetLength(1)).ToList().ForEach(x => BoardArray[0, x] = 0);
            return counter;
        }
        
    }
}
