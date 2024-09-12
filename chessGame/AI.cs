using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class AI:Player
    {
        public AI()
        {
            IsWhite = false;
            TakenPieces = new List<Piece>();
        }
    }
}
