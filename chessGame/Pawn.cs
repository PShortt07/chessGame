using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Pawn:Piece
    {
        public Pawn(bool white, int x, int y) : base(white, x, y)
        {
            Value = 1;
            if (IsWhite)
            {
                PieceImage = Properties.Resources.Wpawn;
            }
            else
            {
                PieceImage = Properties.Resources.pawn;
            }
            PieceName = "pawn";
        }
    }
}
