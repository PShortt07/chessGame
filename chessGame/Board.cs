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
                board[i, 1].OnCell = new Pawn(false); //black pawns
                board[i, 6].OnCell = new Pawn(true); //white pawns
            }
            board[0, 0].OnCell = new Rook(false);
            board[0, 7].OnCell = new Rook(true);
            board[7, 0].OnCell = new Rook(false);
            board[7, 7].OnCell = new Rook(true);
            board[1, 0].OnCell = new Knight(false);
            board[6, 0].OnCell = new Knight(false);
            board[1, 7].OnCell = new Knight(true);
            board[6, 7].OnCell = new Knight(true);
            board[2, 0].OnCell = new Bishop(false);
            board[5, 0].OnCell = new Bishop(false);
            board[2, 7].OnCell = new Bishop(true);
            board[5, 7].OnCell = new Bishop(true);
            board[3, 0].OnCell = new Queen(false);
            board[3, 7].OnCell = new Queen(true);
            board[4, 0].OnCell = new King(false);
            board[4, 7].OnCell = new King(true);
        }
        //checks occupation status of a cell
        public string SpaceOccupied(int posX, int posY)
        {
            if (posX > 7 || posY > 7 || posX < 0 || posY < 0)
            {
                return "outside";
            }
            else
            {
                if (board[posX, posY].OnCell.PieceName == "empty")
                {
                    return "free";
                }
                else if (board[posX, posY].OnCell.IsWhite)
                {
                    return "own piece";
                }
                else
                {
                    return "piece";
                }
            }
        }
        //updates board array when a piece is moved
        //needs to be updated for taking pieces
        public void movePiece(int newX, int newY, int pastX, int pastY)
        {
            board[newX, newY].OnCell = board[pastX, pastY].OnCell;
            if (!board[newX, newY].OnCell.HasMoved)
            {
                board[newX, newY].OnCell.HasMoved = true;
            }
            board[pastX, pastY].OnCell = new Empty();
        }
        //finds all available moves
        public void FindLegalMoves(int posX, int posY)
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
                        if (SpaceOccupied(posX, posY - 2) == "free")
                        {
                            board[posX, posY - 2].LegalMove = true;
                        }
                    }
                    if (SpaceOccupied(posX, posY - 1) == "free")
                    {
                        board[posX, posY - 1].LegalMove = true;
                    }
                    if (SpaceOccupied(posX-1, posY-1) == "piece")
                    {
                        board[posX - 1, posY - 1].LegalMove = true;
                    }
                    if (SpaceOccupied(posX + 1, posY - 1) == "piece")
                    {
                        board[posX + 1, posY - 1].LegalMove = true;
                    }
                    break;
                case ("rook"):
                    for (int i = posY; i >= 0; i--)
                    {
                        checkLegal(posX, i);
                    }
                    break;
                case ("knight"):
                    checkLegal(posX - 2, posY - 1);
                    checkLegal(posX - 1, posY - 2);
                    checkLegal(posX + 1, posY - 2);
                    checkLegal(posX + 2, posY - 1);
                    checkLegal(posX + 2, posY + 1);
                    checkLegal(posX + 1, posY + 2);
                    checkLegal(posX - 1, posY + 2);
                    checkLegal(posX - 2, posY + 1);
                    break;
                case ("bishop"):
                    for (int i = 1; SpaceOccupied(posX-i, posY-i) != "outside"; i++)
                    {
                        //left side
                        if (SpaceOccupied(posX - i, posY - i) == "own piece")
                        {
                            board[posX - i, posY - i].LegalMove = false;
                            break;
                        }
                        else if (SpaceOccupied(posX - i, posY - i) == "piece")
                        {
                            board[posX - i, posY - i].LegalMove = true;
                            break;
                        }
                        else
                        {
                            board[posX - i, posY - i].LegalMove = true;
                        }
                    }
                    for (int i = 1; SpaceOccupied(posX + i, posY - i) != "outside"; i++)
                    {
                        //right side
                        if (SpaceOccupied(posX + i, posY - i) == "own piece")
                        {
                            board[posX + i, posY - i].LegalMove = false;
                            break;
                        }
                        else if (SpaceOccupied(posX + i, posY - i) == "piece")
                        {
                            board[posX + i, posY - i].LegalMove = true;
                            break;
                        }
                        else
                        {
                            board[posX + i, posY - i].LegalMove = true;
                        }
                    }
                    break;
                case ("queen"):
                    break;
                case ("king"):
                    break;
            }
        }
        private void checkLegal(int posX, int posY)
        {
            if (SpaceOccupied(posX, posY) == "free")
            {
                board[posX, posY].LegalMove = true;
            }
            else if (SpaceOccupied(posX, posY) == "own piece")
            {
                board[posX, posY].LegalMove = false;
            }
            else if (SpaceOccupied(posX, posY) == "piece")
            {
                board[posX, posY].LegalMove = true;
            }
        }
    }
}
