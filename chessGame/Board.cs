using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Board
    {
        public Piece[,] board = new Piece[8, 8];
        public Board()
        {
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    board[i, j] = new Empty();
                }
            }
            for (int i = 0; i < 8; i++)
            {
                board[i, 1] = new Pawn();
                board[i, 6] = new Pawn();
            }
            board[0, 0] = new Rook();
            board[0, 7] = new Rook();
            board[7, 0] = new Rook();
            board[7, 7] = new Rook();
            board[1, 0] = new Knight();
            board[6, 0] = new Knight();
            board[1, 7] = new Knight();
            board[6, 7] = new Knight();
            board[2, 0] = new Bishop();
            board[5, 0] = new Bishop();
            board[2, 7] = new Bishop();
            board[5, 7] = new Bishop();
            board[3, 0] = new Queen();
            board[3, 7] = new Queen();
            board[4, 0] = new King();
            board[4, 7] = new King();
        }
        public void SetBoard()
        {

        }
    }
}
