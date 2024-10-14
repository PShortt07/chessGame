using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace chessGame
{
    internal class AI:Player
    {
        private int depth;

        public AI(Board b, int depth, Player human)
        {
            this.depth = depth;
            IsWhite = false;
            TakenPieces = new List<Piece>();
        }
        //makes AI decide and make its move
        public void makeMove(ref Player human, Board chessBoard)
        {
            List<PotentialMove> storedMoves = new List<PotentialMove>();
            int bestX = 0;
            int bestY = 0;
            Board originalBoard = chessBoard;
            int origHScore = human.Score;
            List<Piece> origHPieces = human.MyPieces;
            int origScore = Score;
            List<Piece> origPieces = MyPieces;
            //adds best move for each piece to a list
            foreach (Piece piece in MyPieces)
            {
                storedMoves.Add(valueMove(piece, depth, true, ref bestX, ref bestY, ref chessBoard, ref human));
            }
            Score = origScore;
            MyPieces = origPieces;
            human.Score = origHScore;
            human.MyPieces = origHPieces;
            chessBoard = originalBoard;
            int highestValue = 0;
            PotentialMove bestMove = null;
            //finds the move with the best result
            foreach(PotentialMove potentialMove in storedMoves)
            {
                if (potentialMove.value > highestValue)
                {
                    highestValue = potentialMove.value;
                    bestMove = potentialMove;
                }
            }
            //if no move has a score then a random move is played
            if (bestMove == null)
            {
                Random RNG = new Random();
                Piece p = MyPieces[RNG.Next(MyPieces.Count)];
                List<Cell> possibleMoves = chessBoard.FindLegalMoves(p.PosX, p.PosY, IsWhite);
                bestMove = new PotentialMove(0, p, possibleMoves[RNG.Next(possibleMoves.Count)]);
            }
            //accounts for taken pieces
            if (bestMove.newCell.OnCell.PieceName != "empty" && bestMove.newCell.OnCell.IsWhite == !human.IsWhite)
            {
                Score += bestMove.newCell.OnCell.Value;
                TakenPieces.Add(bestMove.newCell.OnCell);
                human.MyPieces.Remove(bestMove.newCell.OnCell);
            }
            //makes move
            chessBoard.movePiece(bestMove.newCell.OnCell.PosX, bestMove.newCell.OnCell.PosX, bestMove.p.PosX, bestMove.p.PosY);
        }
        //https://www.youtube.com/watch?v=l-hh51ncgDI used as reference for this method
        private PotentialMove valueMove(Piece p, int depth, bool maxPlayer, ref int bestX, ref int bestY, ref Board chessBoard, ref Player human)
        {
            //move returned if out of depth or the game is over
            if (depth == 0 || chessBoard.isGameOver())
            {
                PotentialMove pM = new PotentialMove(human.Score - Score, p, chessBoard.board[bestX, bestY]);
                return pM;
            }
            List<Cell> possibleMoves = chessBoard.FindLegalMoves(p.PosX, p.PosY, !maxPlayer);
            List<Cell> goodMoves = new List<Cell>();
            int evaluation;
            //maximising player
            if (maxPlayer)
            {
                //sets to lowest possible value
                int maxEvaluation = int.MinValue;
                //evaluates possible moves
                foreach (Cell move in possibleMoves)
                {
                    //evaluates possible moves from a possible move until out of depth
                    evaluation = valueMove(p, depth - 1, false, ref bestX, ref bestY, ref chessBoard, ref human).value;
                    //adds better moves to goodMoves
                    if (evaluation > maxEvaluation)
                    {
                        goodMoves.Add(move);
                    }
                    maxEvaluation = Math.Max(maxEvaluation, evaluation);
                    //makes move
                    AI meAgain = this;
                    chessBoard.changeScores(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY, ref human, ref meAgain);
                    Score = meAgain.Score;
                    MyPieces = meAgain.MyPieces;
                    chessBoard.movePiece(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY);
                }
                goodMoves.Sort();
                PotentialMove pM = new PotentialMove(maxEvaluation, p, goodMoves.Last());
                return pM;
            }
            //minimising player
            else
            {
                int minEvaluation = int.MaxValue;
                foreach (Cell move in possibleMoves)
                {
                    evaluation = valueMove(p, depth - 1, true, ref bestX, ref bestY, ref chessBoard, ref human).value;
                    if (evaluation < minEvaluation)
                    {
                        goodMoves.Add(move);
                    }
                    minEvaluation = Math.Min(minEvaluation, evaluation);
                    AI meAgain = this;
                    chessBoard.changeScores(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY, ref human, ref meAgain);
                    Score = meAgain.Score;
                    MyPieces = meAgain.MyPieces;
                    chessBoard.movePiece(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY);
                }
                goodMoves.Sort();
                PotentialMove pM = new PotentialMove(minEvaluation, p, goodMoves.First());
                return pM;
            }
        }
    }
}
