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
        private Bitmap imageLocation = new Bitmap(@"C:\Users\AQ231513\Downloads\bishop.png");

        protected int Value { get => value; set => this.value = value; }
        protected int PosX { get => posX; set => posX = value; }
        protected int PosY { get => posY; set => posY = value; }
        protected bool Captured { get => captured; set => captured = value; }
        public Bitmap ImageLocation { get => imageLocation; set => imageLocation = value; }

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

        }
    }
}
