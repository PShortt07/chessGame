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
        private double score;
        private bool isWhite;

        public bool MyMove { get => myMove; set => myMove = value; }
        public double Score { get => score; set => score = value; }
        public List<Piece> MyPieces { get => myPieces; set => myPieces = value; }
        public List<Piece> TakenPieces { get => takenPieces; set => takenPieces = value; }
        public bool IsWhite { get => isWhite; set => isWhite = value; }

        public Player()
        {
            TakenPieces = new List<Piece>();
            MyPieces = new List<Piece>();
            Score = 39;
        }
    }
}
