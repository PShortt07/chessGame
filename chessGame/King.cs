using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class King:Piece
    {
        public King(bool white)
        {
            IsWhite = white;
            if (IsWhite)
            {
                PieceImage = Properties.Resources.Wking;
            }
            else
            {
                PieceImage = Properties.Resources.king;
            }
            PieceName = "king";
        }
    }
}
