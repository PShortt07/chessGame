using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Rook:Piece
    {
        public Rook(bool white, int x, int y)
        {
            PosX = x;
            PosY = y;
            Value = 5;
            IsWhite = white;
            if (IsWhite)
            {
                PieceImage = Properties.Resources.Wrook;
            }
            else
            {
                PieceImage = Properties.Resources.rook;
            }
            PieceName = "rook";
        }
    }
}
