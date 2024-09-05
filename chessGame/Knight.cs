using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Knight:Piece
    {
        public Knight(bool white)
        {
            Value = 3;
            IsWhite = white;
            if (IsWhite)
            {
                PieceImage = Properties.Resources.Wknight;
            }
            else
            {
                PieceImage = Properties.Resources.knight;
            }
            PieceName = "knight";
        }
    }
}
