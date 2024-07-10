using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Board
    {
        public Cell[,] board = new Cell[8, 8];
        //constructor - sets up chess board
        public Board()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = new Cell(i,j);
                }
            }
            for (int i = 0; i < 8; i++)
            {
                board[i, 1].OnCell = new Pawn();
                board[i, 1].OnCell.IsWhite = true;
                board[i, 6].OnCell = new Pawn();
            }
            board[0, 0].OnCell = new Rook();
            board[0, 7].OnCell.IsWhite = true;
            board[0, 7].OnCell = new Rook();
            board[7, 0].OnCell = new Rook();
            board[7, 7].OnCell.IsWhite = true;
            board[7, 7].OnCell = new Rook();
            board[1, 0].OnCell = new Knight();
            board[1, 7].OnCell.IsWhite = true;
            board[6, 0].OnCell = new Knight();
            board[6, 7].OnCell.IsWhite = true;
            board[1, 7].OnCell = new Knight();
            board[6, 7].OnCell = new Knight();
            board[2, 0].OnCell = new Bishop();
            board[2, 7].OnCell.IsWhite = true;
            board[5, 0].OnCell = new Bishop();
            board[5, 7].OnCell.IsWhite = true;
            board[2, 7].OnCell = new Bishop();
            board[5, 7].OnCell = new Bishop();
            board[3, 0].OnCell = new Queen();
            board[3, 7].OnCell.IsWhite = true;
            board[3, 7].OnCell = new Queen();
            board[4, 0].OnCell = new King();
            board[4, 7].OnCell.IsWhite = true;
            board[4, 7].OnCell = new King();
        }
        public bool SpaceOccupied(int posX, int posY)
        {
            if (board[posX, posY].OnCell.PieceName == "empty")
            {
                return false;
            }
            return true;
        }
        //exception handling not done
        //finds all available moves
        public void LegalMoves(int posX, int posY)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j].LegalMove = false;
                }
            }
            switch (board[posX, posY].OnCell.PieceName)
            {
                case ("pawn"):
                    if (!board[posX, posY].OnCell.HasMoved)
                    {
                        if (!SpaceOccupied(posX, posY + 2))
                        {
                            board[posX, posY + 2].LegalMove = true;
                        }
                    }
                    if (!SpaceOccupied(posX, posY + 1))
                    {
                        board[posX, posY + 1].LegalMove = true;
                    }
                    if (SpaceOccupied(posX-1, posY+1))
                    {
                        board[posX-1, posY + 1].LegalMove = true;
                    }
                    if (SpaceOccupied(posX + 1, posY + 1))
                    {
                        board[posX + 1, posY + 1].LegalMove = true;
                    }
                    break;
                case ("rook"):
                    for (int i = posY; i < 8; i++)
                    {
                        if (!SpaceOccupied(posX, i))
                        {
                            board[posX, i].LegalMove = true;
                        }
                        else
                        {
                            board[posX, i].LegalMove = true;
                            break;
                        }
                    }
                    break;
                case ("knight"):
                    break;
                case ("bishop"):
                    for (int i = 0; i < 8; i++)
                    {

                    }
                    break;
                case ("queen"):
                    break;
                case ("king"):
                    break;
            }
        }
    }
}
