using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Pawn:Piece
    {
        public Pawn()
        {
            ImageLocation = new Bitmap(@"C:\Users\AQ231513\Downloads\pawn.png");
        }
        public static void promote(Piece p)
        {

        }
        public override void move()
        {
            if (canCapture())
            {

            }
        }
        public override bool canCapture()
        {
            return false;
        }
    }
}
