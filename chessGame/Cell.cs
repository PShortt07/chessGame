using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{

    internal class Cell
    {
        private int row;
        private int col;
        private bool occupied;
        private bool legalMove;
        private Piece onCell;

        public int Row { get => row; set => row = value; }
        public int Col { get => col; set => col = value; }
        public bool Occupied { get => occupied; set => occupied = value; }
        public bool LegalMove { get => legalMove; set => legalMove = value; }
        public Piece OnCell { get => onCell; set => onCell = value; }

        public Cell(int xPos, int yPos)
        {
            OnCell = new Empty();
            row = xPos;
            col = yPos;
        }
    }
}
