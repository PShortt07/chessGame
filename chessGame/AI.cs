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
        public void makeMove(ref Player human, ref Board chessBoard, Form1 f)
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
                foreach (Cell move in chessBoard.FindLegalMoves(p.PosX, p.PosY))
                {
                    long value = minimax(p, move, false, 0, 0, depth, chessBoard, humanScore, myScore);
                    possibleMoves.Add(new PotentialMove(value, p, move));
                }
            }
            human.Score = prevScore;
            List<PotentialMove> highestValueMoves = new List<PotentialMove>();
            //finds the highest possible score result of a move
            possibleMoves.OrderBy(o => o.value);
            long highestValue = possibleMoves[0].value;
            foreach (PotentialMove move in possibleMoves)
            {
                if (move.value > highestValue)
                {
                    highestValue = move.value;
                }
            }
            foreach (PotentialMove move in possibleMoves)
            {
                if (move.value == highestValue)
                {
                    highestValueMoves.Add(move);
                }
            }
            PotentialMove bestMove = null;
            Random RNG = new Random();
            bestMove = highestValueMoves[RNG.Next(highestValueMoves.Count)];
            //accounts for taken pieces
            int newX = bestMove.newCell.OnCell.PosX;
            int newY = bestMove.newCell.OnCell.PosY;
            int oldX = bestMove.p.PosX;
            int oldY = bestMove.p.PosY;
            if (bestMove.newCell.OnCell.PieceName != "empty" && bestMove.newCell.OnCell.IsWhite == human.IsWhite)
            {
                Score += bestMove.newCell.OnCell.Value;
                TakenPieces.Add(bestMove.newCell.OnCell);
                f.updateTakenPieces(newX, newY, false);
            }
            //makes move
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
        private long minimax(Piece piece, Cell position, bool maxPlayer, double alpha, double beta, int depth, Board chessBoard, long humanScore, long myScore)
        {
            //scoring system
            if (depth == 0)
            {
                long total = myScore - humanScore;
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
                long maxEvaluation = -10000;
                int origX = piece.PosX;
                int origY = piece.PosY;
                List<Piece> pieces = MyPieces;
                myScore -= position.OnCell.Value;
                chessBoard.movePiece(position.Row, position.Col, piece.PosX, piece.PosY, false);
                chessBoard.whiteTurn = false;
                foreach (Cell c in chessBoard.board)
                {
                    if (c.OnCell.PieceName != "empty" && !c.OnCell.IsWhite)
                    {
                        foreach (Cell move in chessBoard.FindLegalMoves(c.Row, c.Col))
                        {
                            long scoreLoss = move.OnCell.Value;
                            humanScore -= scoreLoss;
                            long evaluation = minimax(piece, move, false, alpha, beta, depth - 1, chessBoard, humanScore, myScore);
                            humanScore += scoreLoss;
                            maxEvaluation = (long)Math.Max(maxEvaluation, evaluation);
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
                long minEvaluation = 10000;
                int origX = piece.PosX;
                int origY = piece.PosY;
                List<Piece> pieces = MyPieces;
                humanScore -= position.OnCell.Value;
                chessBoard.movePiece(position.Row, position.Col, piece.PosX, piece.PosY, false);
                chessBoard.whiteTurn = true;
                foreach (Cell c in chessBoard.board)
                {
                    if (c.OnCell.PieceName != "empty" && c.OnCell.IsWhite)
                    {
                        foreach (Cell move in chessBoard.FindLegalMoves(c.Row, c.Col))
                        {
                            long scoreLoss = move.OnCell.Value;
                            myScore -= scoreLoss;
                            long evaluation = minimax(piece, move, true, alpha, beta, depth - 1, chessBoard, humanScore, myScore);
                            myScore += scoreLoss;
                            minEvaluation = (long)Math.Min(minEvaluation, evaluation);
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
        private long minimax2(bool maxPlayer, double alpha, double beta, int depth, Board chessBoard, ref long humanScore, ref long myScore, ref List<PotentialMove> possibleMoves)
        {
            //scoring system
            if (depth == 0)
            {
                long total = myScore - humanScore;
                foreach (Cell c in chessBoard.board)
                {
                    if (c.CoveredByBlack)
                    {
                        total += 1;
                    }
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
                long maxEvaluation = long.MinValue;
                List<Piece> pieces = MyPieces;
                chessBoard.whiteTurn = false;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Cell c = chessBoard.board[i, j];
                        if (c.OnCell.PieceName != "empty" && !c.OnCell.IsWhite)
                        {
                            int origX = c.OnCell.PosX;
                            int origY = c.OnCell.PosY;
                            Piece piece = c.OnCell;
                            foreach (Cell move in chessBoard.FindLegalMoves(c.Row, c.Col))
                            {
                                humanScore -= move.OnCell.Value;
                                chessBoard.movePiece(move.Row, move.Col, piece.PosX, piece.PosY, false);
                                long evaluation = minimax2(false, alpha, beta, depth - 1, chessBoard, ref humanScore, ref myScore, ref possibleMoves);
                                humanScore += move.OnCell.Value;
                                chessBoard.revertMove(piece, origX, origY, move.Row, move.Col);
                                if (evaluation > maxEvaluation)
                                {
                                    maxEvaluation = evaluation;
                                    possibleMoves.Add(new PotentialMove(evaluation, c.OnCell, move));
                                }
                                alpha = Math.Max(alpha, evaluation);
                                if (beta <= alpha)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                return maxEvaluation;
            }
            else
            {
                long minEvaluation = long.MaxValue;
                List<Piece> pieces = MyPieces;
                chessBoard.whiteTurn = true;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Cell c = chessBoard.board[i, j];
                        if (c.OnCell.PieceName != "empty" && c.OnCell.IsWhite)
                        {
                            int origX = c.OnCell.PosX;
                            int origY = c.OnCell.PosY;
                            Piece piece = c.OnCell;
                            foreach (Cell move in chessBoard.FindLegalMoves(piece.PosX, piece.PosY))
                            {
                                myScore -= move.OnCell.Value;
                                chessBoard.movePiece(move.Row, move.Col, piece.PosX, piece.PosY, false);
                                long evaluation = minimax2(true, alpha, beta, depth - 1, chessBoard, ref humanScore, ref myScore, ref possibleMoves);
                                myScore += move.OnCell.Value;
                                chessBoard.revertMove(piece, origX, origY, move.Row, move.Col);
                                if (evaluation < minEvaluation)
                                {
                                    minEvaluation = evaluation;
                                }
                                beta = Math.Min(beta, evaluation);
                                if (beta <= alpha)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                return minEvaluation;
            }
        }
    }
}