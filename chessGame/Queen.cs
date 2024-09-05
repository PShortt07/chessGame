using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Queen:Piece
    {
        public Queen(bool white)
        {
            Value = 9;
            IsWhite = white;
            if (IsWhite)
            {
                PieceImage = Properties.Resources.Wqueen;
            }
            else
            {
                PieceImage = Properties.Resources.queen;
            }
            PieceName = "queen";
        }
    }
}
