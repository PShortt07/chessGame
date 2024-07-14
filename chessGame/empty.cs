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
            PieceImage = Properties.Resources.BLANK_ICON;
            PieceName = "empty";
        }
    }
}
