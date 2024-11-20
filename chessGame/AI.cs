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
            double tempScore = Score;
            //adds best move for each piece to a list then resets the second board for the next piece analysis
            List<PotentialMove> possibleMoves = new List<PotentialMove>();
            foreach (Piece p in tempPieces)
            {
                foreach (Cell move in chessBoard.FindLegalMoves(p.PosX, p.PosY))
                {
                    double value = minimax(p, move, false, 0, 0, depth, chessBoard, human);
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
            chessBoard.changeScores(newX, newY, oldX, oldY, ref human, ref myPiecesPassIn, ref scorePassIn, true);
            Score = scorePassIn;
            MyPieces = myPiecesPassIn;
            chessBoard.movePiece(newX, newY, oldX, oldY, true);
        }
        private double scoreThisMove(Player human, Piece thisPiece, double myScore)
        {
            //calculates difference in score as a base score, makes positive if negative
            double total = myScore - human.Score;
            return total;
        }
        private double minimax(Piece piece, Cell position, bool maxPlayer, double alpha, double beta, int depth, Board chessBoard, Player human)
        {
            double startHScore = human.Score;
            double startMyScore = human.Score;
            if (depth == 0 || chessBoard.isGameOver(true))
            {
                return Score - human.Score;
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
                double myScore = Score;
                chessBoard.changeScores(position.Row, position.Col, origX, origY, ref human, ref pieces, ref myScore, false);
                Score = myScore;
                chessBoard.movePiece(position.Row, position.Col, piece.PosX, piece.PosY, false);
                foreach(Cell c in chessBoard.board)
                {
                    if (c.OnCell.PieceName != "Empty" && !c.OnCell.IsWhite)
                    {
                        foreach (Cell move in chessBoard.FindLegalMoves(c.Row, c.Col))
                        {
                            double evaluation = minimax(piece, move, false, alpha, beta, depth - 1, chessBoard, human);
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
                human.Score = startHScore;
                Score = startMyScore;
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
                            double evaluation = minimax(piece, move, false, alpha, beta, depth - 1, chessBoard, human);
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
                human.Score = startHScore;
                Score = startMyScore;
                return minEvaluation;
            }
        }
        //https://www.youtube.com/watch?v=l-hh51ncgDI used as a basis for this method
        private PotentialMove valueMove(int depth, Piece p, bool maxPlayer, int bestX, int bestY, double alpha, double beta, ref Board chessBoard, ref Player human, ref List<Piece> tempPieces, ref double tempScore)
        {
            PotentialMove pM;
            List<Cell> possibleMoves = chessBoard.FindLegalMoves(p.PosX, p.PosY);
            //move returned if out of depth or the game is over - wrong idea
            if (depth == 0 || chessBoard.isGameOver(true) || chessBoard.isGameOver(false))
            {
                PotentialMove bestMove = new PotentialMove(scoreThisMove(human, p, tempScore), p, chessBoard.board[bestX, bestY]);
                return bestMove;
            }
            Random RNG = new Random();
            double evaluation;
            //maximising player
            if (maxPlayer && possibleMoves.Count > 0)
            {
                pM = new PotentialMove(double.MinValue, p, possibleMoves[RNG.Next(possibleMoves.Count)]);
                //sets to lowest possible value
                double maxEvaluation = double.MinValue;
                //evaluates possible moves
                int lastPosX = p.PosX;
                int lastPosY = p.PosY;
                foreach (Cell move in possibleMoves)
                {
                    chessBoard.changeScores(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY, ref human, ref tempPieces, ref tempScore, false);
                    chessBoard.movePiece(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY, false);
                    //evaluates possible moves from a possible move until out of depth
                    foreach (Piece piece in tempPieces)
                    {
                        chessBoard.movePiece(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY, false);
                        evaluation = valueMove(depth - 1, piece, false, bestX, bestY, alpha, beta, ref chessBoard, ref human, ref tempPieces, ref tempScore).value;
                        //reverts move
                        chessBoard.revertMove(p, lastPosX, lastPosY, move.OnCell.PosX, move.OnCell.PosY);
                        if (evaluation > maxEvaluation)
                        {
                            maxEvaluation = evaluation;
                            bestX = move.OnCell.PosX;
                            bestY = move.OnCell.PosY;
                            pM = new PotentialMove(evaluation, p, chessBoard.board[bestX, bestY]);
                        }
                        alpha = Math.Max(alpha, evaluation);
                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
                return pM;
            }
            //minimising player
            else if (!maxPlayer && possibleMoves.Count > 0)
            {
                pM = new PotentialMove(double.MaxValue, p, chessBoard.board[0, 0]);
                double minEvaluation = double.MaxValue;
                int lastPosX = p.PosX;
                int lastPosY = p.PosY;
                foreach (Cell move in possibleMoves)
                {
                    chessBoard.changeScores(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY, ref human, ref tempPieces, ref tempScore, false);
                    foreach (Piece piece in tempPieces)
                    {
                        chessBoard.movePiece(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY, false);
                        evaluation = valueMove(depth - 1, p, true, bestX, bestY, alpha, beta, ref chessBoard, ref human, ref tempPieces, ref tempScore).value;
                        //reverts move
                        chessBoard.revertMove(p, lastPosX, lastPosY, move.OnCell.PosX, move.OnCell.PosY);
                        if (evaluation < minEvaluation)
                        {
                            minEvaluation = evaluation;
                            bestX = move.OnCell.PosX;
                            bestY = move.OnCell.PosY;
                            pM = new PotentialMove(minEvaluation, p, chessBoard.board[bestX, bestY]);
                        }
                        beta = Math.Min(beta, evaluation);
                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
                return pM;
            }
            return null;
        }
    }
}