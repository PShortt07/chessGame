using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Knight:Piece
    {
        public Knight(bool white, int x, int y) : base(white, x, y)
        {
            Value = 30;
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
