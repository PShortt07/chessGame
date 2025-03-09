using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Player
    {
        private List<Piece> myPieces;
        private List<Piece> takenPieces;
        private int score;
        private bool isWhite;
        public int Score { get => score; set => score = value; }
        public List<Piece> MyPieces { get => myPieces; set => myPieces = value; }
        public List<Piece> TakenPieces { get => takenPieces; set => takenPieces = value; }
        public bool IsWhite { get => isWhite; set => isWhite = value; }

        public Player()
        {
            TakenPieces = new List<Piece>();
            MyPieces = new List<Piece>();
            //all piece values added together
            Score = 390;
        }
    }
}
