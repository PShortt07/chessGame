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
            int bestX = 0;
            int bestY = 0;
            //makes a second board, human player object, pieces list and score to pass into the minimax algorithm so that they can be changed without affecting their real values
            Board tempBoard = chessBoard;
            Player tempHuman = human;
            List<Piece> tempPieces = MyPieces;
            double tempScore = Score;
            //adds best move for each piece to a list then resets the second board for the next piece analysis
            foreach (Piece piece in tempPieces)
            {
                storedMoves.Add(valueMove(piece, depth, true, ref bestX, ref bestY, ref tempBoard, ref tempHuman, ref tempPieces, ref tempScore));
                tempBoard = chessBoard;
            }
            double highestValue = 0;
            PotentialMove bestMove = null;
            //finds the highest possible score result of a move
            foreach(PotentialMove potentialMove in storedMoves)
            {
                if (potentialMove.value > highestValue)
                {
                    highestValue = potentialMove.value;
                }
            }
            //adds all moves with that value to a list
            List<PotentialMove> highestValueMoves = new List<PotentialMove>();
            foreach(PotentialMove potentialMove in storedMoves)
            {
                if (potentialMove.value == highestValue)
                {
                    highestValueMoves.Add(potentialMove);
                }
            }
            //chooses a random move from highest value list
            Random RNG = new Random();
            bestMove = highestValueMoves[RNG.Next(highestValueMoves.Count)];
            //if no move has a score then a random move is played
            if (bestMove == null)
            {
                Piece p = MyPieces[RNG.Next(MyPieces.Count)];
                List<Cell> possibleMoves = chessBoard.FindLegalMoves(p.PosX, p.PosY);
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
            List<Piece> myPiecesPassIn = MyPieces;
            double scorePassIn = Score;
            chessBoard.changeScores(newX, newY, oldX, oldY, ref human, ref myPiecesPassIn, ref scorePassIn);
            Score = scorePassIn;
            MyPieces = myPiecesPassIn;
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
        //https://www.youtube.com/watch?v=l-hh51ncgDI used as a reference for this method
        private PotentialMove valueMove(Piece p, int depth, bool maxPlayer, ref int bestX, ref int bestY, ref Board tempBoard, ref Player human, ref List<Piece> tempPieces, ref double tempScore)
        {
            //move returned if out of depth or the game is over
            if (depth == 0 || tempBoard.isGameOver())
            {
                PotentialMove pM = new PotentialMove(scoreThisMove(human, p), p, tempBoard.board[bestX, bestY]);
                return pM;
            }
            List<Cell> possibleMoves = tempBoard.FindLegalMoves(p.PosX, p.PosY);
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
                    tempBoard.changeScores(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY, ref human, ref tempPieces, ref tempScore);
                    int lastPosX = p.PosX;
                    int lastPosY = p.PosY;
                    tempBoard.movePiece(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY);
                    //evaluates possible moves from a possible move until out of depth
                    evaluation = valueMove(p, depth - 1, false, ref bestX, ref bestY, ref tempBoard, ref human, ref tempPieces, ref tempScore).value;
                    //adds better moves to goodMoves
                    if (evaluation > maxEvaluation)
                    {
                        goodMoves.Add(move);
                    }
                    maxEvaluation = Math.Max(maxEvaluation, evaluation);
                    //reverts move
                    tempBoard.movePiece(lastPosX, lastPosY, move.OnCell.PosX, move.OnCell.PosY);
                }
                goodMoves.Sort();
                if (goodMoves.Count > 0)
                {
                    goodMoves.Sort();
                    PotentialMove pM = new PotentialMove(maxEvaluation, p, goodMoves.Last());
                    bestX = goodMoves.Last().OnCell.PosX;
                    bestY = goodMoves.Last().OnCell.PosY;
                    return pM;
                }
                else
                {
                    PotentialMove pM = new PotentialMove(0, p, tempBoard.board[5,4]);
                    return pM;
                }
            }
            //minimising player
            else
            {
                double minEvaluation = double.MaxValue;
                foreach (Cell move in possibleMoves)
                {
                    tempBoard.changeScores(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY, ref human, ref tempPieces, ref tempScore);
                    int lastPosX = p.PosX;
                    int lastPosY = p.PosY;
                    tempBoard.movePiece(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY);
                    evaluation = valueMove(p, depth - 1, true, ref bestX, ref bestY, ref tempBoard, ref human, ref tempPieces, ref tempScore).value;
                    if (evaluation < minEvaluation)
                    {
                        goodMoves.Add(move);
                    }
                    minEvaluation = Math.Min(minEvaluation, evaluation);
                    //reverts move
                    tempBoard.movePiece(lastPosX, lastPosY, move.OnCell.PosX, move.OnCell.PosY);
                }
                if (goodMoves.Count > 0)
                {
                    goodMoves.Sort();
                    PotentialMove pM = new PotentialMove(minEvaluation, p, goodMoves.First());
                    bestX = goodMoves.Last().OnCell.PosX;
                    bestY = goodMoves.Last().OnCell.PosY;
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
