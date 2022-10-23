using System;
using System.Diagnostics.Contracts;

namespace TetrisClient
{
    /// <summary>
    /// Represents a matrix.
    /// </summary>
    public readonly struct Matrix
    {
        /// <summary>
        /// Readable name for the rotation method.
        /// </summary>
        /// <param name="value">The current Value of the Matrix.</param>
        /// <param name="size">The size of a row in the Matrix.</param>
        /// <param name="i">Index i.</param>
        /// <param name="j">Index j.</param>
        private delegate int RotationMethod(int[,] value, int size, int i, int j);

        /// <summary>
        /// Property for the actual Matrix.
        /// </summary>
        public int[,] Value { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">The value of the matrix, dimensions must be equal.</param>
        /// <exception cref="ArgumentException"></exception>
        public Matrix(int[,] value)
        {
            if (value.GetLength(0) != value.GetLength(1))
            {
                // This can be removed when implementing the line-removal function,
                // since the multidimensional array will be changed into (possibly) an odd-shaped matrix.
                throw new ArgumentException("This matrix does not support odd-shaped matrices, only even shaped.");
            }

            Value = value;
        }

        /// <summary>
        /// Rotates the matrix clockwise by 90 degrees and returns a new instance of it.
        ///
        /// Before rotation:
        /// [
        ///  [ 0, 0, 1 ]
        ///  [ 1, 1, 1 ]
        ///  [ 0, 0, 0 ]
        /// ]
        ///
        /// After rotation:
        /// [
        ///  [ 0, 1, 0 ]
        ///  [ 0, 1, 0 ]
        ///  [ 0, 1, 1 ]
        /// ]
        /// </summary>
        /// <returns>A new Matrix.</returns>
        [Pure]
        public Matrix Rotate90() => Rotate((value, size, i, j) => value[size - 1 - j, i]);

        /// <summary>
        /// Rotates the matrix counterclockwise by 90 degrees and returns a new instance of it.
        ///
        /// Before rotation:
        /// [
        ///  [ 0, 0, 1 ]
        ///  [ 1, 1, 1 ]
        ///  [ 0, 0, 0 ]
        /// ]
        ///
        /// After rotation:
        /// [
        ///  [ 1, 1, 0 ]
        ///  [ 0, 1, 0 ]
        ///  [ 0, 1, 0 ]
        /// ]
        /// </summary>
        /// <returns>A new Matrix.</returns>
        [Pure]
        public Matrix Rotate90CounterClockwise() => Rotate((value, size, i, j) => value[j, size - 1 - i]);

        /// <summary>
        /// A private member method that recieves a rotation method <see cref="RotationMethod"/>
        /// </summary>
        /// <param name="rotationMethod">How the matrix should be rotated</param>
        /// <returns>A new Matrix.</returns>
        [Pure]
        private Matrix Rotate(RotationMethod rotationMethod)
        {
            var size = Value.GetLength(0);
            var rotatedValue = new int[size, size];
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    rotatedValue[i, j] = rotationMethod.Invoke(Value, size, i, j);
                }
            }

            return new Matrix(rotatedValue);
        }
    }
}
