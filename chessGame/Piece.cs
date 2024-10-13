using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    abstract class Piece
    {
        private int value;
        private int lastPosX;
        private int lastPosY;
        private int posX;
        private int posY;
        private bool captured;
        private bool isWhite;
        private string pieceName;
        private bool hasMoved;
        private Image pieceImage;
        private bool empty;

        public int Value { get => value; set => this.value = value; }
        public int PosX { get => posX; set => posX = value; }
        public int PosY { get => posY; set => posY = value; }
        protected bool Captured { get => captured; set => captured = value; }
        public bool IsWhite { get => isWhite; set => isWhite = value; }
        public string PieceName { get => pieceName; set => pieceName = value; }
        public bool HasMoved { get => hasMoved; set => hasMoved = value; }
        public Image PieceImage { get => pieceImage; set => pieceImage = value; }
        public bool Empty { get => empty; set => empty = value; }

        public Piece()
        {
            HasMoved = false;
            Empty = false;
        }
    }
}
