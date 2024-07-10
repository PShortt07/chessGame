using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Bishop:Piece
    {
        public Bishop()
        {
            ImageLocation = new Bitmap(@"C:\\Users\\patri\\Downloads\\bishop.png");
            PieceName = "bishop";
        }
    }
}
