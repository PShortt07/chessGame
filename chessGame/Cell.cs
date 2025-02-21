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
        private bool legalMove;
        private Piece onCell;
        private bool coveredByWhite;
        private bool coveredByBlack;
        private bool capture;

        public int Row { get => row; set => row = value; }
        public int Col { get => col; set => col = value; }
        public bool LegalMove { get => legalMove; set => legalMove = value; }
        public Piece OnCell { get => onCell; set => onCell = value; }
        public bool CoveredByWhite { get => coveredByWhite; set => coveredByWhite = value; }
        public bool CoveredByBlack { get => coveredByBlack; set => coveredByBlack = value; }
        public bool Capture { get => capture; set => capture = value; }

        public Cell(int xPos, int yPos)
        {
            Capture = false;
            onCell = new Empty(false, xPos, yPos);
            OnCell = new Empty(false, xPos, yPos);
            row = xPos;
            col = yPos;
        }
    }
}
