using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class AI:Player
    {
        private int myPointTotal = 0;
        private int opPointTotal = 0;

        public AI()
        {
            IsWhite = false;
            TakenPieces = new List<Piece>();
        }
        public void makeMove()
        {
            findAllLegalMoves();
        }
        private void findAllLegalMoves()
        {
            List<Cell> legalMoves = new List<Cell>();
        }
    }
}
