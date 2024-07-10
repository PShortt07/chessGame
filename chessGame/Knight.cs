using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Knight:Piece
    {
        public Knight()
        {
            ImageLocation = new Bitmap(@"C:\Users\patri\Downloads\knight.png");
            PieceName = "knight";
        }
    }
}
