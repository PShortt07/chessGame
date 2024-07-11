using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Player
    {
        private bool myMove;
        private List<Piece> myPieces;
        private List<Piece> takenPieces;
        private int score;
        private bool isWhite;

        public bool MyMove { get => myMove; set => myMove = value; }
        public int Score { get => score; set => score = value; }
        internal List<Piece> MyPieces { get => myPieces; set => myPieces = value; }
        internal List<Piece> TakenPieces { get => takenPieces; set => takenPieces = value; }
        public bool IsWhite { get => isWhite; set => isWhite = value; }

        public Player()
        {
            IsWhite = true;
        }
    }
}
