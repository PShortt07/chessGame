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
        private int posX;
        private int posY;
        private bool captured;
        private bool isWhite;
        private string pieceName;
        private bool hasMoved;
        private Image pieceImage;

        protected int Value { get => value; set => this.value = value; }
        protected int PosX { get => posX; set => posX = value; }
        protected int PosY { get => posY; set => posY = value; }
        protected bool Captured { get => captured; set => captured = value; }
        public bool IsWhite { get => isWhite; set => isWhite = value; }
        public string PieceName { get => pieceName; set => pieceName = value; }
        public bool HasMoved { get => hasMoved; set => hasMoved = value; }
        public Image PieceImage { get => pieceImage; set => pieceImage = value; }

        public virtual void move()
        {

        }
        public static void capture()
        {

        }
        public virtual bool canCapture()
        {
            return false;
        }
        public Piece()
        {
            HasMoved = false;
        }
    }
}
