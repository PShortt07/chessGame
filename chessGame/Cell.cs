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
        private bool coveredByWhite;
        private bool coveredByBlack;

        public int Row { get => row; set => row = value; }
        public int Col { get => col; set => col = value; }
        public bool Occupied { get => occupied; set => occupied = value; }
        public bool LegalMove { get => legalMove; set => legalMove = value; }
        public Piece OnCell { get => onCell; set => onCell = value; }
        public bool CoveredByWhite { get => coveredByWhite; set => coveredByWhite = value; }
        public bool CoveredByBlack { get => coveredByBlack; set => coveredByBlack = value; }

        public Cell(int xPos, int yPos)
        {
            OnCell = new Empty();
            row = xPos;
            col = yPos;
        }
    }
}
