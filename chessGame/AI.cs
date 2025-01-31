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
        private Dictionary<string, long> transpoTable;
        private Dictionary<int, Dictionary<long, long>> tableWithDepth;
        public AI(Board b, int depth, Player human)
        {
            this.depth = depth;
            IsWhite = false;
            TakenPieces = new List<Piece>();
            transpoTable = new Dictionary<string, long>();
            tableWithDepth = new Dictionary<int, Dictionary<long, long>>();
        }
        //makes AI decide and make its move
        public void makeMove(ref Player human, ref Board chessBoard, Form1 f)
        {
            List<PotentialMove> storedMoves = new List<PotentialMove>();
            List<Piece> tempPieces = MyPieces;
            long humanScore = human.Score;
            long myScore = Score;
            //adds best move for each piece to a list then resets the second board for the next piece analysis
            List<PotentialMove> possibleMoves = new List<PotentialMove>();
            foreach (Piece p in tempPieces)
            {
                List<Cell> toSearch = chessBoard.FindLegalMoves(p.PosX, p.PosY);
                foreach (Cell move in toSearch.OrderByDescending(x => x.Capture))
                {
                    Move possible = new Move(p.PosX, p.PosY, move.Row, move.Col, false, p, move.OnCell);
                    long value = minimax(p, possible, false, double.NegativeInfinity, double.PositiveInfinity, depth, chessBoard, humanScore - move.OnCell.Value, myScore);
                    possibleMoves.Add(new PotentialMove(value, p, move));
                }
            }
            List<PotentialMove> highestValueMoves = new List<PotentialMove>();
            //finds the highest possible score result of a move
            possibleMoves = possibleMoves.OrderByDescending(x => x.value).ToList<PotentialMove>();
            long highestValue = possibleMoves[0].value;
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
            Move thisMove;
            //checks for promotion
            if (chessBoard.board[oldX, oldY].OnCell.PieceName == "pawn" && newY == 7)
            {
                thisMove = new Move(oldX, oldY, newX, newY, true, chessBoard.board[oldX, oldY].OnCell, chessBoard.board[newX, newY].OnCell);
                thisMove.setPromotion(new Queen(IsWhite, newX, newY));
                Piece pawn = chessBoard.board[newX, newY].OnCell;
            }
            else
            {
                thisMove = new Move(oldX, oldY, newX, newY, false, chessBoard.board[oldX, oldY].OnCell, chessBoard.board[newX, newY].OnCell);
            }
            chessBoard.movePiece(thisMove, true);
        }
        private long minimax(Piece piece, Move thisMove, bool maxPlayer, double alpha, double beta, int depth, Board chessBoard, long humanScore, long myScore)
        {
            //scoring system
            if (depth == 0)
            {
                //score difference
                long total = myScore - humanScore;
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
                total += (long)covered;
                return total;
            }
            if (chessBoard.isGameOver(true))
            {
                transpoTable.Add(generateHashValue(chessBoard, depth), 500);
                return 500;
            }
            else if (chessBoard.isGameOver(false))
            {
                transpoTable.Add(generateHashValue(chessBoard, depth), -500);
                return -500;
            }
            //minimax
            if (maxPlayer)
            {
                long maxEvaluation = long.MinValue;
                int origX = piece.PosX;
                int origY = piece.PosY;
                List<Piece> pieces = MyPieces;
                chessBoard.movePiece(thisMove, false);
                chessBoard.whiteTurn = false;
                foreach (Cell c in chessBoard.board)
                {
                    if (c.OnCell.PieceName != "empty" && !c.OnCell.IsWhite)
                    {
                        foreach (Cell move in chessBoard.FindLegalMoves(c.Row, c.Col))
                        {
                            long scoreLoss = move.OnCell.Value;
                            Move newMove = new Move(origX, origY, move.Row, move.Col, false, piece, move.OnCell);
                            if (chessBoard.board[piece.PosX, piece.PosY].OnCell.PieceName == "pawn" && piece.PosY == 7)
                            {
                                thisMove.promoted = true;
                                //always chooses queen for the sake of efficiency
                                thisMove.setPromotion(new Queen(IsWhite, piece.PosX, piece.PosY));
                                scoreLoss -= 9;
                            }
                            humanScore -= scoreLoss;
                            long evaluation = minimax(piece, newMove, false, alpha, beta, depth - 1, chessBoard, humanScore, myScore);
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
                chessBoard.revertMove(thisMove);
                return maxEvaluation;
            }
            else
            {
                long minEvaluation = long.MaxValue;
                int origX = piece.PosX;
                int origY = piece.PosY;
                List<Piece> pieces = MyPieces;
                chessBoard.movePiece(thisMove, false);
                chessBoard.whiteTurn = true;
                foreach (Cell c in chessBoard.board)
                {
                    if (c.OnCell.PieceName != "empty" && c.OnCell.IsWhite)
                    {
                        foreach (Cell move in chessBoard.FindLegalMoves(c.Row, c.Col))
                        {
                            long scoreLoss = move.OnCell.Value;
                            Move newMove = new Move(origX, origY, move.Row, move.Col, false, piece, move.OnCell);
                            if (chessBoard.board[piece.PosX, piece.PosY].OnCell.PieceName == "pawn" && piece.PosY == 7)
                            {
                                thisMove.promoted = true;
                                //always chooses queen for the sake of efficiency
                                thisMove.setPromotion(new Queen(IsWhite, piece.PosX, piece.PosY));
                                scoreLoss -= 9;
                            }
                            myScore -= scoreLoss;
                            long evaluation = minimax(piece, newMove, true, alpha, beta, depth - 1, chessBoard, humanScore, myScore);
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
                chessBoard.revertMove(thisMove);
                return minEvaluation;
            }
        }
        //https://www.youtube.com/watch?v=l-hh51ncgDI used as a basis for this method
        private string generateHashValue(Board chessBoard, int depth)
        {
            long hashValue = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8 ; j++)
                {
                    if (chessBoard.board[i, j].OnCell.PieceName != "empty")
                    {
                        switch (chessBoard.board[i, j].OnCell.PieceName)
                        {
                            case "pawn":
                                if (chessBoard.board[i, j].OnCell.IsWhite)
                                {
                                    hashValue ^= chessBoard.board[i, j].WPawnV;
                                }
                                else
                                {
                                    hashValue ^= chessBoard.board[i, j].BPawnV;
                                }
                                break;
                            case "rook":
                                if (chessBoard.board[i, j].OnCell.IsWhite)
                                {
                                    hashValue ^= chessBoard.board[i, j].WRookV;
                                }
                                else
                                {
                                    hashValue ^= chessBoard.board[i, j].BRookV;
                                }
                                break;
                            case "knight":
                                if (chessBoard.board[i, j].OnCell.IsWhite)
                                {
                                    hashValue ^= chessBoard.board[i, j].WKnightV;
                                }
                                else
                                {
                                    hashValue ^= chessBoard.board[i, j].BKnightV;
                                }
                                break;
                            case "bishop":
                                if (chessBoard.board[i, j].OnCell.IsWhite)
                                {
                                    hashValue ^= chessBoard.board[i, j].WBishopV;
                                }
                                else
                                {
                                    hashValue ^= chessBoard.board[i, j].BBishopV;
                                }
                                break;
                            case "queen":
                                if (chessBoard.board[i, j].OnCell.IsWhite)
                                {
                                    hashValue ^= chessBoard.board[i, j].WQueenV;
                                }
                                else
                                {
                                    hashValue ^= chessBoard.board[i, j].BQueenV;
                                }
                                break;
                            case "king":
                                if (chessBoard.board[i, j].OnCell.IsWhite)
                                {
                                    hashValue ^= chessBoard.board[i, j].WKingV;
                                }
                                else
                                {
                                    hashValue ^= chessBoard.board[i, j].BKingV;
                                }
                                break;
                        }
                    }
                }
            }
            string toReturn = hashValue.ToString() + depth.ToString();
            return toReturn;
        }
        }
    }