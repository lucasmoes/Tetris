
using System;

namespace TetrisClient.Game
{
    /// <summary>
    /// Score keeps track of the number of lines cleared, level and the amount of point.
    /// </summary>
    public class Score
    {
        public int Level { get; set; }
        public int Points { get; set; }
        public int Lines { get; set; }
        
        public Score()
        {
            Level = 0;
            Points = 0;
            Lines = 0;
        }
        
        /// <summary>
        /// add point is used in when there is a soft and hard drop
        /// </summary>
        public void AddPoint()
        {
            Points += 1;
        }
        
        /// <summary>
        /// when lines are being cleared it returns the number of lines
        /// cleared and uses the level to calculate the amount of points
        /// </summary>
        public void RowsPoints(int numberOfRows)
        {
            Lines += numberOfRows;
            var points = numberOfRows switch
            {
                1 => 40,
                2 => 100,
                3 => 300,
                4 => 1200,
                _ => throw new ArgumentException("Invalid number of rows")
                ,
            };
            
            Points += (points * (Level + 1));
            CalculateLevel();
        }
        /// <summary>
        /// calculates the level based on the number of lines cleared
        /// </summary>
        private void CalculateLevel()
        {
            Level = Lines switch
            {
                10 => 1,
                20 => 2,
                30 => 3,
                40 => 4,
                50 => 5,
                60 => 6,
                70 => 7,
                80 => 8,
                90 => 9,
                100 => 10,
                110 => 11,
                120 => 12,
                130 => 13,
                140 => 14,
                150 => 15,
                _ => Level
            };
        }
    }
}