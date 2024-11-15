﻿using System;
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
            bool notChanged = true;
            //adds best move for each piece to a list then resets the second board for the next piece analysis
            storedMoves.Add(valueMove(depth, true, 0, 0, double.MinValue, double.MaxValue, ref chessBoard, ref human, ref tempPieces, ref tempScore, ref notChanged));
            human.Score = prevScore;
            double highestValue = 0;
            List<PotentialMove> highestValueMoves = new List<PotentialMove>();
            //finds the highest possible score result of a move
            foreach(PotentialMove potentialMove in storedMoves)
            {
                if (potentialMove.value > highestValue)
                {
                    highestValue = potentialMove.value;
                }
            }
            foreach(PotentialMove potentialMove in storedMoves)
            {
                if (potentialMove.value == highestValue)
                {
                    highestValueMoves.Add(potentialMove);
                    break;
                }
            }
            PotentialMove bestMove = null;
            Random RNG = new Random();
            try
            {
                bestMove = highestValueMoves[RNG.Next(highestValueMoves.Count)];
            }
            catch
            {
                bestMove = storedMoves[RNG.Next(highestValueMoves.Count)];
            }
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
        //https://www.youtube.com/watch?v=l-hh51ncgDI used as a basis for this method
        private PotentialMove valueMove(int depth, bool maxPlayer, int bestX, int bestY, double alpha, double beta, ref Board chessBoard, ref Player human, ref List<Piece> tempPieces, ref double tempScore, PotentialMove bestMove, ref bool notChanged)
        {
            foreach (Piece p in tempPieces)
            {
                List<Cell> possibleMoves = chessBoard.FindLegalMoves(p.PosX, p.PosY);
                //move returned if out of depth or the game is over - wrong idea
                if (depth == 0 || chessBoard.isGameOver(true) || chessBoard.isGameOver(false) || possibleMoves.Count == 0)
                {
                    return bestMove;
                }
                Random RNG = new Random();
                Cell thisMove = possibleMoves[RNG.Next(possibleMoves.Count)];
                double evaluation;
                //maximising player
                if (maxPlayer)
                {
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
                        evaluation = valueMove(depth - 1, false, bestX, bestY, alpha, beta, ref chessBoard, ref human, ref tempPieces, ref tempScore, bestMove, ref notChanged).value;
                        //reverts move
                        chessBoard.revertMove(p, lastPosX, lastPosY, move.OnCell.PosX, move.OnCell.PosY);
                        //adds better moves to goodMoves
                        if (evaluation > maxEvaluation)
                        {
                            maxEvaluation = evaluation;
                            PotentialMove newBest = new PotentialMove(maxEvaluation, p, chessBoard.board[move.Row, move.Col]);
                            bestMove = newBest;
                            notChanged = false;
                        }
                        alpha = Math.Max(alpha, evaluation);
                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                    return bestMove;
                }
                //minimising player
                else
                {
                    double minEvaluation = double.MaxValue;
                    int lastPosX = p.PosX;
                    int lastPosY = p.PosY;
                    foreach (Cell move in possibleMoves)
                    {
                        chessBoard.changeScores(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY, ref human, ref tempPieces, ref tempScore, false);
                        chessBoard.movePiece(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY, false);
                        evaluation = valueMove(depth - 1, true, bestX, bestY, alpha, beta, ref chessBoard, ref human, ref tempPieces, ref tempScore, bestMove, ref notChanged).value;
                        //reverts move
                        chessBoard.revertMove(p, lastPosX, lastPosY, move.OnCell.PosX, move.OnCell.PosY);
                        if (evaluation < minEvaluation)
                        {
                            minEvaluation = evaluation;
                            PotentialMove newBest = new PotentialMove(minEvaluation, p, chessBoard.board[move.Row, move.Col]);
                            bestMove = newBest;
                            notChanged = false;
                        }
                        minEvaluation = Math.Min(minEvaluation, evaluation);
                        beta = Math.Min(beta, evaluation);
                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                    return bestMove;
                }
            }
            return bestMove;
        }
    }
}