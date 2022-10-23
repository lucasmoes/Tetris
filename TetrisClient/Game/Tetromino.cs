using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace TetrisClient.Game
{
    public class Tetromino
    {
        public int offsetX { get; set; }
        public int offsetY { get; set; }
        public Matrix shapeMatrix { get; set; }
        public Tetromino(int x, int y, Random random)
        {
            offsetX = x;
            offsetY = y;
            shapeMatrix = returnRandomShape(random);  
        }
        
        public Tetromino(int x, int y, Matrix shape)
        {
            offsetX = x;
            offsetY = y;
            shapeMatrix = shape;
        }

        /// <summary>
        /// when up or down is pressed it will rotate the tetromino clockwise or counter clockwise
        /// </summary>
        public void turn(string move)
        {
            
            shapeMatrix = move switch
            {
                "UP" => shapeMatrix.Rotate90(),
                "DOWN" => shapeMatrix.Rotate90CounterClockwise(),
                _ => shapeMatrix
            };
        }

        /// <summary>
        /// this returns a random shape to be used in the game
        /// </summary>
        public static Matrix returnRandomShape(Random random)
        {
            var shape = (TetrominoShape)random.Next(0, 7);
            return ReturnShape(shape);
        }

        /// <summary>
        /// returns the matrix of the shape depending on the shape enum
        /// </summary>
        private static Matrix ReturnShape(TetrominoShape shape)
        {
            return shape switch
            {
                TetrominoShape.O => new Matrix(new[,] {{1, 1}, {1, 1}}),
                TetrominoShape.T => new Matrix(new[,] {{2, 2, 2}, {0, 2, 0}, {0, 0, 0}}),
                TetrominoShape.J => new Matrix(new[,] {{0, 3, 0}, {0, 3, 0}, {3, 3, 0}}),
                TetrominoShape.L => new Matrix(new[,] {{0, 4, 0}, {0, 4, 0}, {0, 4, 4}}),
                TetrominoShape.S => new Matrix(new[,] {{0, 5, 5}, {5, 5, 0}, {0, 0, 0}}),
                TetrominoShape.Z => new Matrix(new[,] {{6, 6, 0}, {0, 6, 6}, {0, 0, 0}}),
                TetrominoShape.I => new Matrix(new[,] {{0, 0, 0, 0}, {7, 7, 7, 7}, {0, 0, 0, 0}, {0, 0, 0, 0}}),
                _ => throw new Exception("Unknown shape")
            };
        }
        /// <summary>
        /// depending on the shape enum it returns the color the shape should be
        /// </summary>
        public static Brush DetermineColor(TetrominoShape shape)
        {
            return shape switch
            {
                TetrominoShape.O => new SolidColorBrush(Color.FromRgb(25, 40, 0)),
                TetrominoShape.T => new SolidColorBrush(Color.FromRgb(25, 60, 0)),
                TetrominoShape.J => new SolidColorBrush(Color.FromRgb(30, 90, 1)),
                TetrominoShape.L => new SolidColorBrush(Color.FromRgb(35, 120, 1)),
                TetrominoShape.S => new SolidColorBrush(Color.FromRgb(40, 150, 1)),
                TetrominoShape.Z => new SolidColorBrush(Color.FromRgb(45, 180, 2)),
                TetrominoShape.I => new SolidColorBrush(Color.FromRgb(50, 210, 2)),
                TetrominoShape.Ghost => new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                _ => throw new ArgumentOutOfRangeException(nameof(shape), shape, null)
            };
        }
    }
}