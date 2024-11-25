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
        public void makeMove(ref Player human, ref Board chessBoard)
        {
            List<PotentialMove> storedMoves = new List<PotentialMove>();
            long prevScore = human.Score;
            List<Piece> tempPieces = MyPieces;
            long humanScore = human.Score;
            long myScore = Score;
            //adds best move for each piece to a list then resets the second board for the next piece analysis
            List<PotentialMove> possibleMoves = new List<PotentialMove>();
            foreach (Piece p in tempPieces)
            {
                double value = minimax(p, true, 0, 0, depth, chessBoard, humanScore, myScore);
                possibleMoves.Add(new PotentialMove(value, p, move));
            }
            human.Score = prevScore;
            List<PotentialMove> highestValueMoves = new List<PotentialMove>();
            //finds the highest possible score result of a move
            possibleMoves.OrderBy(o => o.value);
            foreach (PotentialMove move in possibleMoves)
            {
                if (possibleMoves[0].value == move.value)
                {
                    highestValueMoves.Add(move);
                }
            }
            PotentialMove bestMove = null;
            Random RNG = new Random();
            bestMove = highestValueMoves[RNG.Next(highestValueMoves.Count)];
            //accounts for taken pieces
            if (bestMove.newCell.OnCell.PieceName != "empty" && bestMove.newCell.OnCell.IsWhite == !human.IsWhite)
            {
                Score += bestMove.newCell.OnCell.Value;
                TakenPieces.Add(bestMove.newCell.OnCell);
                human.MyPieces.Remove(bestMove.newCell.OnCell);
            }
            //makes move
            int newX = bestMove.newCell.OnCell.PosX;
            int newY = bestMove.newCell.OnCell.PosY;
            int oldX = bestMove.p.PosX;
            int oldY = bestMove.p.PosY;
            //creates new variables to pass in to changeScores() then assigns changed qualities to AI
            List<Piece> myPiecesPassIn = MyPieces;
            long scorePassIn = Score;
            List<Piece> humanPiecesPassIn = human.MyPieces;
            long humanScorePassIn = human.Score;
            chessBoard.changeScores(newX, newY, oldX, oldY, ref humanScorePassIn, ref humanPiecesPassIn, ref myPiecesPassIn, ref scorePassIn, true);
            Score = scorePassIn;
            MyPieces = myPiecesPassIn;
            human.Score = humanScorePassIn;
            human.MyPieces = humanPiecesPassIn;
            chessBoard.movePiece(newX, newY, oldX, oldY, true);
        }
        private PotentialMove findBestMove()
        {

        }
        private double scoreThisMove(Player human, Piece thisPiece, double myScore)
        {
            //calculates difference in score as a base score, makes positive if negative
            double total = myScore - human.Score;
            return total;
        }
        private long minimax(Piece piece, bool maxPlayer, double alpha, double beta, int depth, Board chessBoard, long humanScore, long myScore)
        {
            //scoring system
            if (depth == 0)
            {
                long total = (myScore*10) - (humanScore*10);
                foreach (Cell c in chessBoard.board)
                {
                    if (c.CoveredByBlack)
                    {
                        if (c.OnCell.PosY == 3 || c.OnCell.PosY == 4)
                        {
                            total += 1;
                        }
                    }
                }
                if (piece.Isolated)
                {
                    total -= 5;
                    piece.Isolated = false;
                }
                return total;
            }
            if (chessBoard.isGameOver(true))
            {
                return 200;
            }
            else if (chessBoard.isGameOver(false))
            {
                return -200;
            }
            //minimax
            if (maxPlayer)
            {
                long maxEvaluation = -1000;
                int origX = piece.PosX;
                int origY = piece.PosY;
                List<Piece> pieces = MyPieces;
                foreach(Cell c in chessBoard.board)
                {
                    if (c.OnCell.PieceName != "empty" && !c.OnCell.IsWhite)
                    {
                        foreach (Cell move in chessBoard.FindLegalMoves(c.Row, c.Col))
                        {
                            humanScore -= move.OnCell.Value;
                            chessBoard.movePiece(move.Row, move.Col, piece.PosX, piece.PosY, false);
                            double evaluation = minimax(piece, false, alpha, beta, depth - 1, chessBoard, humanScore, myScore);
                            chessBoard.revertMove(piece, origX, origY, piece.PosX, piece.PosY);
                            maxEvaluation = (long)Math.Max(maxEvaluation, evaluation);
                            alpha = Math.Max(alpha, evaluation);
                            if (beta <= alpha)
                            {
                                break;
                            }
                        }
                    }
                }
                return maxEvaluation;
            }
            else
            {
                long minEvaluation = 1000;
                int origX = piece.PosX;
                int origY = piece.PosY;
                List<Piece> pieces = MyPieces;
                foreach (Cell c in chessBoard.board)
                {
                    if (c.OnCell.PieceName != "empty" && c.OnCell.IsWhite)
                    {
                        foreach (Cell move in chessBoard.FindLegalMoves(c.Row, c.Col))
                        {
                            myScore -= move.OnCell.Value;
                            chessBoard.movePiece(move.Row, move.Col, piece.PosX, piece.PosY, false);
                            double evaluation = minimax(piece, true, alpha, beta, depth - 1, chessBoard, humanScore, myScore);
                            chessBoard.revertMove(piece, origX, origY, piece.PosX, piece.PosY);
                            minEvaluation = (long)Math.Min(minEvaluation, evaluation);
                            beta = Math.Min(beta, evaluation);
                            if (beta <= alpha)
                            {
                                break;
                            }
                        }
                    }
                }
                return minEvaluation;
            }
        }
        //https://www.youtube.com/watch?v=l-hh51ncgDI used as a basis for this method
    }
}