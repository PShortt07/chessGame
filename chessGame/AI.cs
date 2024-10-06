using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void makeMove(bool whiteTurn)
        {
            List<PotentialMove> storedMoves = new List<PotentialMove>();
            foreach (Piece p in MyPieces)
            {
                List<Cell> moves = chessBoard.FindLegalMoves(p.PosX, p.PosY, whiteTurn);
                //finds value of every move
                foreach (Cell move in moves)
                {
                    PotentialMove x = findMoveValue(move, p, whiteTurn, depth);
                    storedMoves.Add(x);
                }
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
