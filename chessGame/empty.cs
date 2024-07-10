using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Empty:Piece
    {
        public Empty()
        {
            ImageLocation = new Bitmap(@"C:\Users\patri\Downloads\BLANK_ICON.png");
            PieceName = "empty";
        }
    }
}
