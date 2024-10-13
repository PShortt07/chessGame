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
        private int myPointTotal = 0;
        private int opPointTotal = 0;
        private Board chessBoard;
        private int depth;
        private Player human;

        public AI(Board b, int depth, Player human)
        {
            chessBoard = b;
            this.human = human;
            this.depth = depth;
            IsWhite = false;
            TakenPieces = new List<Piece>();
        }
        public void makeMove()
        {
            List<PotentialMove> storedMoves = new List<PotentialMove>();
            int bestX = 0;
            int bestY = 0;
            Board evalBoard = chessBoard;
            foreach (Piece piece in MyPieces)
            {
                storedMoves.Add(valueMove(piece, depth, true, ref bestX, ref bestY, ref evalBoard));
            }
            int highestValue = 0;
            PotentialMove bestMove = null;
            foreach(PotentialMove potentialMove in storedMoves)
            {
                if (potentialMove.value > highestValue)
                {
                    highestValue = potentialMove.value;
                    bestMove = potentialMove;
                }
            }
            if (bestMove == null)
            {
                Random RNG = new Random();
                Piece p = MyPieces[RNG.Next(MyPieces.Count)];
                List<Cell> possibleMoves = chessBoard.FindLegalMoves(p.PosX, p.PosY, IsWhite);
                bestMove = new PotentialMove(0, p, possibleMoves[RNG.Next(possibleMoves.Count)]);
            }
            if (bestMove.newCell.OnCell.PieceName != "empty" && bestMove.newCell.OnCell.IsWhite == !human.IsWhite)
            {
                Score += bestMove.newCell.OnCell.Value;
                TakenPieces.Add(bestMove.newCell.OnCell);
                human.MyPieces.Remove(bestMove.newCell.OnCell);
            }
            chessBoard.movePiece(bestX, bestY, bestMove.p.PosX, bestMove.p.PosY);
        }
        //https://www.youtube.com/watch?v=l-hh51ncgDI used as reference for this method
        private PotentialMove valueMove(Piece p, int depth, bool maxPlayer, ref int bestX, ref int bestY, ref Board evalBoard)
        {
            if (depth == 0 || chessBoard.isGameOver())
            {
                PotentialMove pM = new PotentialMove(human.Score - Score, p, chessBoard.board[bestX, bestY]);
                return pM;
            }
            List<Cell> possibleMoves = chessBoard.FindLegalMoves(p.PosX, p.PosY, !maxPlayer);
            List<Cell> goodMoves = new List<Cell>();
            int evaluation;
            if (maxPlayer)
            {
                int maxEvaluation = int.MinValue;
                foreach (Cell move in possibleMoves)
                {
                    evaluation = valueMove(p, depth - 1, false, ref bestX, ref bestY, ref evalBoard).value;
                    if (evaluation > maxEvaluation)
                    {
                        goodMoves.Add(move);
                    }
                    maxEvaluation = Math.Max(maxEvaluation, evaluation);
                    evalBoard.movePiece(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY);
                }
                goodMoves.Sort();
                PotentialMove pM = new PotentialMove(maxEvaluation, p, goodMoves.Last());
                return pM;
            }
            else
            {
                int minEvaluation = int.MaxValue;
                foreach (Cell move in possibleMoves)
                {
                    evaluation = valueMove(p, depth - 1, true, ref bestX, ref bestY, ref evalBoard).value;
                    if (evaluation < minEvaluation)
                    {
                        goodMoves.Add(move);
                    }
                    minEvaluation = Math.Min(minEvaluation, evaluation);
                    evalBoard.movePiece(move.OnCell.PosX, move.OnCell.PosY, p.PosX, p.PosY);
                }
                goodMoves.Sort();
                PotentialMove pM = new PotentialMove(minEvaluation, p, goodMoves.First());
                return pM;
            }
        }
        //score system not finished
        private PotentialMove findMoveValue(Cell newCell, Piece p, bool whiteTurn, int dep)
        {
            int thisScore = 0;
            //goes up to certain depth
            for (int i = 0; i < dep; i++)
            {
                //adds value of any taken pieces to score
                if (newCell.OnCell.PieceName != "empty" && newCell.OnCell.IsWhite == !whiteTurn)
                {
                    thisScore += chessBoard.board[newCell.OnCell.PosX, newCell.OnCell.PosX].OnCell.Value;
                }
                //makes the move
                chessBoard.movePiece(newCell.OnCell.PosX, newCell.OnCell.PosY, p.PosX, p.PosY);
                dep--;
                //assesses what white can do next
                List<PotentialMove> othersMoves = new List<PotentialMove>();
                foreach (Piece x in human.MyPieces)
                {
                    //evaluates each move, recursive
                    List<Cell> moves = chessBoard.FindLegalMoves(p.PosX, p.PosY, !whiteTurn);
                    foreach (Cell c in moves)
                    {
                        PotentialMove y = findMoveValue(c, x, whiteTurn, dep);
                        othersMoves.Add(y);
                    }
                }
            }
            PotentialMove move = new PotentialMove(thisScore, p, newCell);
            return move;
        }
        private void findAllLegalMoves()
        {
            //TakenPieces.Add(b.board[currentX, currentY].OnCell);
        }
    }
}
