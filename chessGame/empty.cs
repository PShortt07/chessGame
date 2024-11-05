using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Empty:Piece
    {
        public Empty(bool white, int x, int y) : base(white, x, y)
        {
            PieceImage = Properties.Resources.BLANK_ICON;
            PieceName = "empty";
            PosX = x;
            PosY = y;
            IsWhite = false;
            Empty = true;
        }
    }
}
