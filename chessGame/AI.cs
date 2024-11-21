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
            double prevScore = human.Score;
            List<Piece> tempPieces = MyPieces;
            double humanScore = human.Score;
            double myScore = Score;
            //adds best move for each piece to a list then resets the second board for the next piece analysis
            List<PotentialMove> possibleMoves = new List<PotentialMove>();
            foreach (Piece p in tempPieces)
            {
                foreach (Cell move in chessBoard.FindLegalMoves(p.PosX, p.PosY))
                {
                    double value = minimax(p, move, false, 0, 0, depth, chessBoard, ref humanScore, ref myScore);
                    possibleMoves.Add(new PotentialMove(value, p, move));
                }
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
            double scorePassIn = Score;
            List<Piece> humanPiecesPassIn = human.MyPieces;
            double humanScorePassIn = human.Score;
            chessBoard.changeScores(newX, newY, oldX, oldY, ref humanScorePassIn, ref humanPiecesPassIn, ref myPiecesPassIn, ref scorePassIn, true);
            Score = scorePassIn;
            MyPieces = myPiecesPassIn;
            human.Score = humanScorePassIn;
            human.MyPieces = humanPiecesPassIn;
            chessBoard.movePiece(newX, newY, oldX, oldY, true);
        }
        private double scoreThisMove(Player human, Piece thisPiece, double myScore)
        {
            //calculates difference in score as a base score, makes positive if negative
            double total = myScore - human.Score;
            return total;
        }
        private double minimax(Piece piece, Cell position, bool maxPlayer, double alpha, double beta, int depth, Board chessBoard, ref double humanScore, ref double myScore)
        {
            if (depth == 0)
            {
                double total = myScore - humanScore;
                foreach (Cell c in chessBoard.board)
                {
                    if (c.CoveredByBlack)
                    {
                        total = total + 0.1;
                        if (c.OnCell.PosY == 3 || c.OnCell.PosY == 4)
                        {
                            total += 0.1;
                        }
                    }
                    else
                    {
                        total -= 0.1;
                    }
                }
                if (piece.Isolated)
                {
                    total -= 0.5;
                    piece.Isolated = false;
                }
                return total;
            }
            if (chessBoard.isGameOver(true))
            {
                return 500;
            }
            else if (chessBoard.isGameOver(false))
            {
                return -500;
            }
            else if (chessBoard.isGameOver(false))
            {
                return double.MinValue;
            }
            if (maxPlayer)
            {
                double maxEvaluation = double.MinValue;
                int origX = piece.PosX;
                int origY = piece.PosY;
                List<Piece> pieces = MyPieces;
                chessBoard.changeScores(position.Row, position.Col, origX, origY, ref humanScore, ref pieces, ref pieces, ref myScore, false);
                chessBoard.movePiece(position.Row, position.Col, piece.PosX, piece.PosY, false);
                foreach(Cell c in chessBoard.board)
                {
                    if (c.OnCell.PieceName != "Empty" && !c.OnCell.IsWhite)
                    {
                        foreach (Cell move in chessBoard.FindLegalMoves(c.Row, c.Col))
                        {
                            double evaluation = minimax(piece, move, false, alpha, beta, depth - 1, chessBoard, ref humanScore, ref myScore);
                            maxEvaluation = Math.Max(maxEvaluation, evaluation);
                            alpha = Math.Max(alpha, evaluation);
                            if (beta <= alpha)
                            {
                                break;
                            }
                        }
                    }
                }
                chessBoard.revertMove(piece, origX, origY, piece.PosX, piece.PosY);
                return maxEvaluation;
            }
            else
            {
                double minEvaluation = double.MaxValue;
                int origX = piece.PosX;
                int origY = piece.PosY;
                chessBoard.movePiece(position.Row, position.Col, piece.PosX, piece.PosY, false);
                foreach (Cell c in chessBoard.board)
                {
                    if (c.OnCell.PieceName != "Empty" && c.OnCell.IsWhite)
                    {
                        foreach (Cell move in chessBoard.FindLegalMoves(c.Row, c.Col))
                        {
                            double evaluation = minimax(piece, move, false, alpha, beta, depth - 1, chessBoard, ref humanScore, ref myScore);
                            minEvaluation = Math.Min(minEvaluation, evaluation);
                            beta = Math.Min(beta, evaluation);
                            if (beta <= alpha)
                            {
                                break;
                            }
                        }
                    }
                }
                chessBoard.revertMove(piece, origX, origY, piece.PosX, piece.PosY);
                return minEvaluation;
            }
        }
        //https://www.youtube.com/watch?v=l-hh51ncgDI used as a basis for this method
    }
}