using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Bishop:Piece
    {
        public Bishop(bool white, int x, int y)
        {
            PosX = x;
            PosY = y;
            Value = 3;
            IsWhite = white;
            if (IsWhite)
            {
                PieceImage = Properties.Resources.Wbishop;
            }
            else
            {
                PieceImage = Properties.Resources.bishop;
            }
            PieceName = "bishop";
        }
    }
}
