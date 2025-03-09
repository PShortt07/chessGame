using System;
using System.Collections.Generic;
using System.Globalization;
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
        //makes AI decide on and make its move
        public Move makeMove(ref Player human, ref Board chessBoard, Game f)
        {
            //makes variables so that the real values are not changed during evaluation
            List<Piece> tempPieces = MyPieces;
            long humanScore = human.Score;
            long myScore = Score;
            //adds best move for each piece to a list
            List<PotentialMove> possibleMoves = new List<PotentialMove>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (chessBoard.board[i, j].OnCell.PieceName != "empty" && !chessBoard.board[i, j].OnCell.IsWhite)
                    {
                        Piece p = chessBoard.board[i, j].OnCell;
                        //finds all legal moves for the current piece and adds them to a list to be evaluated
                        List<Cell> toSearch = chessBoard.FindLegalMoves(p.PosX, p.PosY);
                        foreach (Cell move in toSearch.OrderByDescending(x => x.Capture))
                        {
                            Move possible = new Move(p.PosX, p.PosY, move.Row, move.Col, false, p, move.OnCell);
                            //small correction by Copilot (alpha and beta values passed in previously were 0 and 0 rather than negative and positive infinity, causing minimax to behave incorrectly)
                            double value = minimax(p, possible, false, double.NegativeInfinity, double.PositiveInfinity, depth, chessBoard, humanScore - move.OnCell.Value, myScore);
                            possibleMoves.Add(new PotentialMove(value, p, move));
                        }
                    }
                }
            }
            //chooses the best move from the list
            PotentialMove bestMove = chooseBestMove(possibleMoves);
            int newX = bestMove.newCell.OnCell.PosX;
            int newY = bestMove.newCell.OnCell.PosY;
            int oldX = bestMove.p.PosX;
            int oldY = bestMove.p.PosY;
            //accounts for taken pieces
            if (bestMove.newCell.OnCell.PieceName != "empty" && bestMove.newCell.OnCell.IsWhite == human.IsWhite)
            {
                TakenPieces.Add(bestMove.newCell.OnCell);
                f.updateTakenPieces(newX, newY, false);
            }
            //changes scores
            changeScoresForMove(ref chessBoard, newX, newY, oldX, oldY, ref human);
            Move thisMove;
            //checks for promotion
            if (chessBoard.board[oldX, oldY].OnCell.PieceName == "pawn" && newY == 7)
            {
                thisMove = new Move(oldX, oldY, newX, newY, true, chessBoard.board[oldX, oldY].OnCell, chessBoard.board[newX, newY].OnCell);
                //always chooses queen for the sake of efficiency
                thisMove.setPromotion(new Queen(IsWhite, newX, newY));
            }
            else
            {
                thisMove = new Move(oldX, oldY, newX, newY, false, chessBoard.board[oldX, oldY].OnCell, chessBoard.board[newX, newY].OnCell);
            }
            //makes move
            chessBoard.movePiece(thisMove, true);
            return thisMove;
        }
        private PotentialMove chooseBestMove(List<PotentialMove> possibleMoves)
        {
            List<PotentialMove> highestValueMoves = new List<PotentialMove>();
            //finds the highest scoring move(s) - small correction by Copilot (previously incorrectly used .OrderBy rather than .OrderByDescending)
            possibleMoves = possibleMoves.OrderByDescending(x => x.value).ToList<PotentialMove>();
            double highestValue = possibleMoves[0].value;
            foreach (PotentialMove move in possibleMoves)
            {
                if (move.value == highestValue)
                {
                    highestValueMoves.Add(move);
                }
            }
            PotentialMove bestMove = null;
            //chooses a random move from the highest scoring moves
            Random RNG = new Random();
            bestMove = highestValueMoves[RNG.Next(highestValueMoves.Count)];
            return bestMove;
        }
        private void changeScoresForMove(ref Board chessBoard, int newX, int newY, int oldX, int oldY, ref Player human)
        {
            //creates new variables to pass in to changeScores() then assigns changed qualities to AI and player
            List<Piece> myPiecesPassIn = MyPieces;
            int scorePassIn = Score;
            List<Piece> humanPiecesPassIn = human.MyPieces;
            int humanScorePassIn = human.Score;
            chessBoard.changeScores(newX, newY, oldX, oldY, ref humanScorePassIn, ref humanPiecesPassIn, ref myPiecesPassIn, ref scorePassIn, true);
            Score = scorePassIn;
            MyPieces = myPiecesPassIn;
            human.Score = humanScorePassIn;
            human.MyPieces = humanPiecesPassIn;
        }
        private double evaluateBoard(double myScore, double humanScore, Piece piece, Board chessBoard)
        {
            //score difference
            double total = myScore - humanScore;
            //try to use lower value pieces earlier in the game
            if (piece.Value == 1)
            {
                total += 2;
            }
            else if (piece.Value == 3)
            {
                total++;
            }
            //try to cover as many cells as possible
            double covered = 0;
            foreach (Cell c in chessBoard.board)
            {
                if (c.CoveredByBlack)
                {
                    covered += 0.1;
                }
            }
            total += covered;
            return total;
        }
        private double minimax(Piece piece, Move thisMove, bool maxPlayer, double alpha, double beta, int depth, Board chessBoard, double humanScore, double myScore)
        {
            //scoring system
            if (depth == 0)
            {
                return evaluateBoard(myScore, humanScore, piece, chessBoard);
            }
            //if the AI has checkmated the player
            if (chessBoard.isGameOver(true) && chessBoard.wInCheck)
            {
                return 500;
            }
            //if the player has checkmated the AI
            else if (chessBoard.isGameOver(false) && chessBoard.bInCheck)
            {
                return -500;
            }
            //minimax
            if (maxPlayer)
            {
                double maxEvaluation = double.MinValue;
                int origX = piece.PosX;
                int origY = piece.PosY;
                List<Piece> pieces = MyPieces;
                chessBoard.movePiece(thisMove, false);
                chessBoard.whiteTurn = false;
                foreach (Cell c in chessBoard.board)
                {
                    //checks for all black pieces
                    if (c.OnCell.PieceName != "empty" && !c.OnCell.IsWhite)
                    {
                        foreach (Cell move in chessBoard.FindLegalMoves(c.Row, c.Col))
                        {
                            long scoreLoss = move.OnCell.Value;
                            Move newMove = new Move(origX, origY, move.Row, move.Col, false, piece, move.OnCell);
                            //promotion
                            if (chessBoard.board[piece.PosX, piece.PosY].OnCell.PieceName == "pawn" && piece.PosY == 7)
                            {
                                thisMove.promoted = true;
                                //always chooses queen for the sake of efficiency
                                thisMove.setPromotion(new Queen(IsWhite, piece.PosX, piece.PosY));
                                scoreLoss -= 9;
                            }
                            humanScore -= scoreLoss;
                            double evaluation = minimax(piece, newMove, false, alpha, beta, depth - 1, chessBoard, humanScore, myScore);
                            humanScore += scoreLoss;
                            //updates maxEvaluation if necessary
                            maxEvaluation = Math.Max(maxEvaluation, evaluation);
                            //alpha-beta pruning
                            alpha = Math.Max(alpha, evaluation);
                            if (beta <= alpha)
                            {
                                break;
                            }
                        }
                    }
                }
                chessBoard.revertMove(thisMove);
                return maxEvaluation;
            }
            else
            {
                double minEvaluation = double.MaxValue;
                int origX = piece.PosX;
                int origY = piece.PosY;
                List<Piece> pieces = MyPieces;
                chessBoard.movePiece(thisMove, false);
                chessBoard.whiteTurn = true;
                foreach (Cell c in chessBoard.board)
                {
                    //checks for all white pieces
                    if (c.OnCell.PieceName != "empty" && c.OnCell.IsWhite)
                    {
                        foreach (Cell move in chessBoard.FindLegalMoves(c.Row, c.Col))
                        {
                            long scoreLoss = move.OnCell.Value;
                            Move newMove = new Move(origX, origY, move.Row, move.Col, false, piece, move.OnCell);
                            //promotion
                            if (chessBoard.board[piece.PosX, piece.PosY].OnCell.PieceName == "pawn" && piece.PosY == 7)
                            {
                                thisMove.promoted = true;
                                //always chooses queen for the sake of efficiency
                                thisMove.setPromotion(new Queen(IsWhite, piece.PosX, piece.PosY));
                                scoreLoss -= 9;
                            }
                            myScore -= scoreLoss;
                            double evaluation = minimax(piece, newMove, true, alpha, beta, depth - 1, chessBoard, humanScore, myScore);
                            myScore += scoreLoss;
                            //updates minEvaluation if necessary
                            minEvaluation = Math.Min(minEvaluation, evaluation);
                            //alpha-beta pruning
                            beta = Math.Min(beta, evaluation);
                            if (beta <= alpha)
                            {
                                break;
                            }
                        }
                    }
                }
                chessBoard.revertMove(thisMove);
                return minEvaluation;
            }
        }
        //https://www.youtube.com/watch?v=l-hh51ncgDI used as a basis for the minimax method
        }
    }