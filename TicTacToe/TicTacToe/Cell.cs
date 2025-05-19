using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class Cell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        private Cell(int row, int column)
        {
            Row = row;
            Column = column;
        }
        public static Cell From(int row, int column)
        {
            return new Cell(row, column);
        }
        public static Cell ErrorCell()
        {
            return new Cell(-1, -1);
        }
        public bool IsErrorCell()
        {
            return Row == -1 && Column == -1;
        }
        public bool IsValidGameFieldCell()
        {
            return Row >= 0 && Row <= 2 && Column >= 0 && Column <= 2;
        }
    }
}
