using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Rook:Piece
    {
        public Rook(bool white, int x, int y) : base(white, x, y)
        {
            Value = 50;
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
