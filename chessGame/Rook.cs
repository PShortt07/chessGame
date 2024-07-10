using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Rook:Piece
    {
        public Rook()
        {
            ImageLocation = new Bitmap(@"C:\Users\patri\Downloads\rook.png");
            PieceName = "rook";
        }
    }
}
