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
        private Dictionary<long, long> transpoTable;
        public AI(Board b, int depth, Player human)
        {
            this.depth = depth;
            IsWhite = false;
            TakenPieces = new List<Piece>();
            transpoTable = new Dictionary<long, long>();
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
                    long value = minimax(p, move, false, double.NegativeInfinity, double.PositiveInfinity, depth, chessBoard, humanScore - move.OnCell.Value, myScore);
                    possibleMoves.Add(new PotentialMove(value, p, move));
                }
            }
            List<PotentialMove> highestValueMoves = new List<PotentialMove>();
            //finds the highest possible score result of a move
            possibleMoves.OrderByDescending(x => x.value);
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
            long hashValue = generateHashValue(chessBoard);
            if (transpoTable.ContainsKey(hashValue))
            {
                return transpoTable[hashValue];
            }
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
                transpoTable.Add(generateHashValue(chessBoard), total);
                return total;
            }
            if (chessBoard.isGameOver(true))
            {
                transpoTable.Add(generateHashValue(chessBoard), 500);
                return 200;
            }
            else if (chessBoard.isGameOver(false))
            {
                transpoTable.Add(generateHashValue(chessBoard), -500);
                return -200;
            }
            //minimax
            if (maxPlayer)
            {
                long maxEvaluation = long.MinValue;
                int origX = piece.PosX;
                int origY = piece.PosY;
                List<Piece> pieces = MyPieces;
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
                long minEvaluation = long.MaxValue;
                int origX = piece.PosX;
                int origY = piece.PosY;
                List<Piece> pieces = MyPieces;
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
        private long generateHashValue(Board chessBoard)
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
            return hashValue;
        }
        }
    }