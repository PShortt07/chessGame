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
        private Bitmap imageLocation = new Bitmap(@"C:\Users\patri\Downloads\bishop.png");
        private bool isWhite;
        private string pieceName;
        private bool hasMoved;

        protected int Value { get => value; set => this.value = value; }
        protected int PosX { get => posX; set => posX = value; }
        protected int PosY { get => posY; set => posY = value; }
        protected bool Captured { get => captured; set => captured = value; }
        public Bitmap ImageLocation { get => imageLocation; set => imageLocation = value; }
        public bool IsWhite { get => isWhite; set => isWhite = value; }
        public string PieceName { get => pieceName; set => pieceName = value; }
        public bool HasMoved { get => hasMoved; set => hasMoved = value; }

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
            isWhite = false;
            HasMoved = false;
        }
    }
}
