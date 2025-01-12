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
        private bool capture;
        private long bBishopV;
        private long wBishopV;
        private long bRookV;
        private long wRookV;
        private long bQueenV;
        private long wQueenV;
        private long bKnightV;
        private long wKnightV;
        private long bPawnV;
        private long wPawnV;
        private long bKingV;
        private long wKingV;

        public int Row { get => row; set => row = value; }
        public int Col { get => col; set => col = value; }
        public bool Occupied { get => occupied; set => occupied = value; }
        public bool LegalMove { get => legalMove; set => legalMove = value; }
        public Piece OnCell { get => onCell; set => onCell = value; }
        public bool CoveredByWhite { get => coveredByWhite; set => coveredByWhite = value; }
        public bool CoveredByBlack { get => coveredByBlack; set => coveredByBlack = value; }
        public bool Capture { get => capture; set => capture = value; }
        public long BBishopV { get => bBishopV; set => bBishopV = value; }
        public long WBishopV { get => wBishopV; set => wBishopV = value; }
        public long BRookV { get => bRookV; set => bRookV = value; }
        public long WRookV { get => wRookV; set => wRookV = value; }
        public long BQueenV { get => bQueenV; set => bQueenV = value; }
        public long WQueenV { get => wQueenV; set => wQueenV = value; }
        public long BKnightV { get => bKnightV; set => bKnightV = value; }
        public long WKnightV { get => wKnightV; set => wKnightV = value; }
        public long BPawnV { get => bPawnV; set => bPawnV = value; }
        public long WPawnV { get => wPawnV; set => wPawnV = value; }
        public long BKingV { get => bKingV; set => bKingV = value; }
        public long WKingV { get => wKingV; set => wKingV = value; }

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
