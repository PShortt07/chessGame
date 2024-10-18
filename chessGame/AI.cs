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
        public void makeMove(ref Player human, Board chessBoard)
        {
            List<PotentialMove> storedMoves = new List<PotentialMove>();
            int bestX = 0;
            int bestY = 0;
            Board tempBoard = chessBoard;
            Board origBoard = chessBoard;
            int origHScore = human.Score;
            List<Piece> origHPieces = human.MyPieces;
            int origScore = Score;
            List<Piece> origPieces = MyPieces;
            //adds best move for each piece to a list
            foreach (Piece piece in MyPieces)
            {
                storedMoves.Add(valueMove(piece, depth, true, ref bestX, ref bestY, ref tempBoard, ref human));
                tempBoard = chessBoard;
            }
            //resets to current board status
            Score = origScore;
            MyPieces = origPieces;
            for (int i = 0; i < MyPieces.Count; i++)
            {
                MyPieces[i] = origPieces[i];
            }
            for (int i = 0; i < human.MyPieces.Count; i++)
            {
                human.MyPieces[i] = origHPieces[i];
            }
            human.Score = origHScore;
            human.MyPieces = origHPieces;
            double highestValue = 0;
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
            int newX = bestMove.newCell.OnCell.PosX;
            int newY = bestMove.newCell.OnCell.PosY;
            int oldX = bestMove.p.PosX;
            int oldY = bestMove.p.PosY;
            //creates new AI object to pass in to changeScores() then assigns changed qualities to current object
            AI meAgain = this;
            chessBoard.changeScores(newX, newY, oldX, oldY, ref human, ref meAgain);
            Score = meAgain.Score;
            MyPieces = meAgain.MyPieces;
            chessBoard.movePiece(newX, newY, oldX, oldY);
        }
        private double scoreThisMove(Player human, Piece thisPiece)
        {
            //calculates difference in score as a base score, makes positive if negative
            double total = human.Score - Score;
            if (total < 0)
            {
                total *= -1;
            }
            //pawns can only move forward - closer to promotion
            if (thisPiece.PieceName == "pawn")
            {
                total += 0.1;
            }
            //these should be used earlier in the game
            else if (thisPiece.PieceName == "bishop" ||  thisPiece.PieceName == "knight")
            {
                total += 0.1;
            }
            return total;
        }
        //https://www.youtube.com/watch?v=l-hh51ncgDI used as reference for this method
        private PotentialMove valueMove(Piece p, int depth, bool maxPlayer, ref int bestX, ref int bestY, ref Board chessBoard, ref Player human)
        {
            //move returned if out of depth or the game is over
            if (depth == 0 || chessBoard.isGameOver())
            {
                PotentialMove pM = new PotentialMove(scoreThisMove(human, p), p, chessBoard.board[bestX, bestY]);
                return pM;
            }
            List<Cell> possibleMoves = chessBoard.FindLegalMoves(p.PosX, p.PosY, !maxPlayer);
            List<Cell> goodMoves = new List<Cell>();
            double evaluation;
            //maximising player
            if (maxPlayer)
            {
                //sets to lowest possible value
                double maxEvaluation = double.MinValue;
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
                if (goodMoves.Count > 0)
                {
                    goodMoves.Sort();
                    PotentialMove pM = new PotentialMove(maxEvaluation, p, goodMoves.Last());
                    return pM;
                }
                else
                {
                    PotentialMove pM = new PotentialMove(0, p, chessBoard.board[5,4]);
                    return pM;
                }
            }
            //minimising player
            else
            {
                double minEvaluation = double.MaxValue;
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
                if (goodMoves.Count > 0)
                {
                    goodMoves.Sort();
                    PotentialMove pM = new PotentialMove(minEvaluation, p, goodMoves.First());
                    return pM;
                }
                else
                {
                    Random RNG = new Random();
                    PotentialMove pM = new PotentialMove(minEvaluation, p, possibleMoves[RNG.Next(possibleMoves.Count)]);
                    return pM;
                }
            }
        }
    }
}
