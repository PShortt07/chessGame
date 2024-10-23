﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Board
    {
        public bool whiteTurn = true;
        public Cell[,] board = new Cell[8, 8];
        private bool wInCheck;
        private bool bInCheck;
        public int bKingX;
        public int bKingY;
        public int wKingX;
        public int wKingY;
        //constructor - sets up chess board
        public Board()
        {
            wInCheck = false;
            bInCheck = false;
            bKingX = 4;
            bKingY = 0;
            wKingX = 4;
            wKingY = 7;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = new Cell(i,j);
                }
            }
            for (int i = 0; i < 8; i++)
            {
                board[i, 1].OnCell = new Pawn(false, i, 1); //black pawns
                board[i, 6].OnCell = new Pawn(true, i, 6); //white pawns
            }
            board[0, 0].OnCell = new Rook(false, 0, 0);
            board[0, 7].OnCell = new Rook(true, 0, 7);
            board[7, 0].OnCell = new Rook(false, 7, 0);
            board[7, 7].OnCell = new Rook(true, 7, 7);
            board[1, 0].OnCell = new Knight(false, 1, 0);
            board[6, 0].OnCell = new Knight(false, 6, 0);
            board[1, 7].OnCell = new Knight(true, 1, 7);
            board[6, 7].OnCell = new Knight(true, 6, 7);
            board[2, 0].OnCell = new Bishop(false, 2, 0);
            board[5, 0].OnCell = new Bishop(false, 5, 0);
            board[2, 7].OnCell = new Bishop(true, 2, 7);
            board[5, 7].OnCell = new Bishop(true, 5, 7);
            board[3, 0].OnCell = new Queen(false, 3, 0);
            board[3, 7].OnCell = new Queen(true, 3, 7);
            board[4, 0].OnCell = new King(false, 4, 0);
            board[4, 7].OnCell = new King(true, 4, 7);
        }
        public void refreshLists(ref Player human, ref AI AI)
        {
            human.MyPieces = new List<Piece>();
            AI.MyPieces = new List<Piece>();
            foreach (Cell c in board)
            {
                Piece assessing = c.OnCell;
                if (assessing.IsWhite == human.IsWhite && !assessing.Empty)
                {
                    human.MyPieces.Add(assessing);
                }
                else if (assessing.IsWhite == AI.IsWhite && !assessing.Empty)
                {
                    AI.MyPieces.Add(assessing);
                }
            }
        }
        public bool isGameOver()
        {
            foreach (Cell c in board)
            {
                if (c.OnCell.PieceName != "empty" && c.OnCell.IsWhite)
                {
                    List<Cell> moves = new List<Cell>();
                    moves = FindLegalMoves(c.OnCell.PosX, c.OnCell.PosY);
                    if (moves.Count > 0)
                    {
                        return false;
                    }
                }
                else if (c.OnCell.PieceName != "empty" && !c.OnCell.IsWhite)
                {
                    List<Cell> moves = new List<Cell>();
                    moves = FindLegalMoves(c.OnCell.PosX, c.OnCell.PosY);
                    if (moves.Count > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        //checks occupation status of a cell
        public string SpaceStatus(int posX, int posY, bool forWhite)
        {
            if (posX > 7 || posY > 7 || posX < 0 || posY < 0)
            {
                return "outside";
            }
            else
            {
                if (board[posX, posY].OnCell.PieceName == "empty")
                {
                    return "empty";
                }
                else if (board[posX, posY].OnCell.IsWhite == forWhite)
                {
                    return "own piece";
                }
                else if (board[posX, posY].OnCell.IsWhite != forWhite)
                {
                    return "opponent's piece";
                }
                else
                {
                    return "";
                }
            }
        }
        public bool doesTakeOutOfCheck(int newX, int newY, int currentX, int currentY)
        {
            //makes the move, finds which spaces are covered by which colour, then reverts the move
            movePiece(newX, newY, currentX, currentY);
            //checks if the player whose turn it is has been taken out of check
            bool outOfCheck = false;
            if (whiteTurn)
            {
                outOfCheck = !findIfInCheck(true);
            }
            else
            {
                outOfCheck = !findIfInCheck(false);
            }
            movePiece(currentX, currentY, newX, newY);
            return outOfCheck;
        }
        public void changeScores(int newX, int newY, int oldX, int oldY, ref Player human, ref List<Piece> AIPieces, ref double AIScore)
        {
            if (board[newX, newY].OnCell.PieceName != "empty" && board[newX, newY].OnCell.IsWhite == true)
            {
                human.Score += board[newX, newY].OnCell.Value;
                AIPieces.Remove(board[newX, newY].OnCell);
            }
            else if (board[newX, newY].OnCell.PieceName != "empty" && board[newX, newY].OnCell.IsWhite == false)
            {
                AIScore += board[newX, newY].OnCell.Value;
                human.MyPieces.Remove(board[newX, newY].OnCell);
            }
        }
        //updates board array when a piece is moved
        public void movePiece(int newX, int newY, int pastX, int pastY)
        {
            //keeps track of kings
            if (board[pastX, pastY].OnCell.PieceName == "king")
            {
                if (board[pastX, pastY].OnCell.IsWhite)
                {
                    wKingX = newX;
                    wKingY = newY;
                }
                else
                {
                    bKingX = newX;
                    bKingY = newY;
                }
            }
            board[newX, newY].LastOnCell = board[newX, newY].OnCell;
            board[newX, newY].OnCell = board[pastX, pastY].OnCell;
            board[newX, newY].OnCell.PosX = newX;
            board[newX, newY].OnCell.PosY = newY;
            if (!board[newX, newY].OnCell.HasMoved)
            {
                board[newX, newY].OnCell.HasMoved = true;
            }
            //empties previous cell
            board[pastX, pastY].OnCell = new Empty(pastX, pastY);
        }
        public void placePiece(int posX, int posY, Piece p)
        {
            board[posX, posY].OnCell = board[posX, posY].LastOnCell;
        }
        //finds all available moves
        public List<Cell> FindLegalMoves(int posX, int posY)
        {
            wInCheck = findIfInCheck(true);
            bInCheck = findIfInCheck(false);
            Cell current = board[posX, posY];
            List<Cell> allowedMoves = new List<Cell>();
            if (wInCheck || bInCheck)
            {
                allowMovesThatTakeOutOfCheck(board[posX, posY], ref allowedMoves);
            }
            else
            {
                addMovesToList(ref allowedMoves, current, true);
                if (whiteTurn)
                {
                    foreach (Cell c in board)
                    {
                        if (allowedMoves.Contains(c))
                        {
                            c.LegalMove = true;
                        }
                        else
                        {
                            c.LegalMove = false;
                        }
                    }
                }
            }
            return allowedMoves;
        }
        //allows only moves that will take the in check player out of check
        private void allowMovesThatTakeOutOfCheck(Cell selected, ref List<Cell> allowedMoves)
        {
            List<Cell> possibleMoves = new List<Cell>();
            addMovesToList(ref possibleMoves, selected, true);
            List<Cell> takeOutOfCheck = new List<Cell>();
            foreach (Cell newPos in possibleMoves)
            {
                if (doesTakeOutOfCheck(newPos.OnCell.PosX, newPos.OnCell.PosY, selected.OnCell.PosX, selected.OnCell.PosY))
                {
                    takeOutOfCheck.Add(newPos);
                }
            }
            foreach (Cell c in takeOutOfCheck)
            {
                allowedMoves.Add(c);
            }
        }
        private void showLegalPawn(int posX, int posY, ref List<Cell> allowedMoves, bool whitePiece)
        {
            if (whitePiece)
            {
                if (!board[posX, posY].OnCell.HasMoved)
                {
                    if (SpaceStatus(posX, posY - 2, whitePiece) == "empty" && SpaceStatus(posX, posY - 1, whitePiece) == "empty")
                    {
                        allowedMoves.Add(board[posX, posY - 2]);
                    }
                }
                if (SpaceStatus(posX, posY - 1, whitePiece) == "empty")
                {
                    allowedMoves.Add(board[posX, posY - 1]);
                }
                if (SpaceStatus(posX - 1, posY - 1, whitePiece) == "opponent's piece")
                {
                    allowedMoves.Add(board[posX - 1, posY - 1]);
                }
                if (SpaceStatus(posX + 1, posY - 1, whitePiece) == "opponent's piece")
                {
                    allowedMoves.Add(board[posX + 1, posY - 1]);
                }
            }
            else
            {
                if (!board[posX, posY].OnCell.HasMoved)
                {
                    if (SpaceStatus(posX, posY + 2, whitePiece) == "empty" && SpaceStatus(posX, posY + 1, whitePiece) == "empty")
                    {
                        allowedMoves.Add(board[posX, posY + 2]);
                    }
                }
                if (SpaceStatus(posX, posY + 1, whitePiece) == "empty")
                {
                    allowedMoves.Add(board[posX, posY + 1]);
                }
                if (SpaceStatus(posX + 1, posY + 1, whitePiece) == "opponent's piece")
                {
                    allowedMoves.Add(board[posX + 1, posY + 1]);
                }
                if (SpaceStatus(posX - 1, posY + 1, whitePiece) == "opponent's piece")
                {
                    allowedMoves.Add(board[posX - 1, posY + 1]);
                }
            }
        }
        private void showLegalRook(int posX, int posY, ref List<Cell> allowedMoves, bool whitePiece)
        {
            //up
            for (int i = posY - 1; i >= 0; i--)
            {
                if (SpaceStatus(posX, i, whitePiece) == "empty")
                {
                    allowedMoves.Add(board[posX, i]);
                }
                else if (SpaceStatus(posX, i, whitePiece) == "opponent's piece")
                {
                    allowedMoves.Add(board[posX, i]);
                    break;
                }
                else
                {
                    break;
                }
            }
            //down
            for (int i = posY + 1; i <= 8; i++)
            {
                if (SpaceStatus(posX, i, whitePiece) == "empty")
                {
                    allowedMoves.Add(board[posX, i]);
                }
                else if (SpaceStatus(posX, i, whitePiece) == "opponent's piece")
                {
                    allowedMoves.Add(board[posX, i]);
                    break;
                }
                else
                {
                    break;
                }
            }
            //left
            for (int i = posX - 1; i >= 0; i--)
            {
                if (SpaceStatus(i, posY, whitePiece) == "empty")
                {
                    allowedMoves.Add(board[i, posY]);
                }
                else if (SpaceStatus(i, posY, whitePiece) == "opponent's piece")
                {
                    allowedMoves.Add(board[i, posY]);
                    break;
                }
                else
                {
                    break;
                }
            }
            //right
            for (int i = posX + 1; i <= 8; i++)
            {
                if (SpaceStatus(i, posY, whitePiece) == "empty")
                {
                    allowedMoves.Add(board[i, posY]);
                }
                else if (SpaceStatus(i, posY, whitePiece) == "opponent's piece")
                {
                    allowedMoves.Add(board[i, posY]);
                    break;
                }
                else
                {
                    break;
                }
            }

        }
        private void showLegalKnight(int posX, int posY, ref List<Cell> allowedMoves, bool whitePiece)
        {
            checkLegal(posX - 2, posY - 1, ref allowedMoves, whitePiece);
            checkLegal(posX - 1, posY - 2, ref allowedMoves, whitePiece);
            checkLegal(posX + 1, posY - 2, ref allowedMoves, whitePiece);
            checkLegal(posX + 2, posY - 1, ref allowedMoves, whitePiece);
            checkLegal(posX + 2, posY + 1, ref allowedMoves, whitePiece);
            checkLegal(posX + 1, posY + 2, ref allowedMoves, whitePiece);
            checkLegal(posX - 1, posY + 2, ref allowedMoves, whitePiece);
            checkLegal(posX - 2, posY + 1, ref allowedMoves, whitePiece);
        }
        private void showLegalBishop(int posX, int posY, ref List<Cell> allowedMoves, bool whitePiece)
        {
            //left side up
            for (int i = 1; SpaceStatus(posX - i, posY - i, whitePiece) != "outside"; i++)
            {
                if (SpaceStatus(posX - i, posY - i, whitePiece) == "opponent's piece")
                {
                    allowedMoves.Add(board[posX - i, posY - i]);
                    break;
                }
                else if (SpaceStatus(posX - i, posY - i, whitePiece) == "empty")
                {
                    allowedMoves.Add(board[posX - i, posY - i]);
                }
                else
                {
                    break;
                }
            }
            //right side up
            for (int i = 1; SpaceStatus(posX + i, posY - i, whitePiece) != "outside"; i++)
            {
                if (SpaceStatus(posX + i, posY - i, whitePiece) == "opponent's piece")
                {
                    allowedMoves.Add(board[posX + i, posY - i]);
                    break;
                }
                else if (SpaceStatus(posX + i, posY - i, whitePiece) == "empty")
                {
                    allowedMoves.Add(board[posX + i, posY - i]);
                }
                else
                {
                    break;
                }
            }
            //left side down
            for (int i = 1; SpaceStatus(posX - i, posY + i, whitePiece) != "outside"; i++)
            {
                if (SpaceStatus(posX - i, posY + i, whitePiece) == "opponent's piece")
                {
                    allowedMoves.Add(board[posX - i, posY + i]);
                    break;
                }
                else if (SpaceStatus(posX - i, posY + i, whitePiece) == "empty")
                {
                    allowedMoves.Add(board[posX - i, posY + i]);
                }
                else
                {
                    break;
                }
            }
            //right side down
            for (int i = 1; SpaceStatus(posX + i, posY + i, whitePiece) != "outside"; i++)
            {
                if (SpaceStatus(posX + i, posY + i, whitePiece) == "opponent's piece")
                {
                    allowedMoves.Add(board[posX + i, posY + i]);
                    break;
                }
                else if (SpaceStatus(posX + i, posY + i, whitePiece) == "empty")
                {
                    allowedMoves.Add(board[posX + i, posY + i]);
                }
                else
                {
                    break;
                }
            }
        }
        private void showLegalQueen(int posX, int posY, ref List<Cell> allowedMoves, bool whitePiece)
        {
            showLegalBishop(posX, posY, ref allowedMoves, whitePiece);
            showLegalRook(posX, posY, ref allowedMoves, whitePiece);
        }
        private void showLegalKing(int posX, int posY, ref List<Cell> allowedMoves, bool whitePiece)
        {
            checkLegal(posX - 1, posY, ref allowedMoves, whitePiece);
            checkLegal(posX - 1, posY + 1, ref allowedMoves, whitePiece);
            checkLegal(posX - 1, posY - 1, ref allowedMoves, whitePiece);
            checkLegal(posX, posY - 1, ref allowedMoves, whitePiece);
            checkLegal(posX, posY + 1, ref allowedMoves, whitePiece);
            checkLegal(posX + 1, posY, ref allowedMoves, whitePiece);
            checkLegal(posX + 1, posY - 1, ref allowedMoves, whitePiece);
            checkLegal(posX + 1, posY + 1, ref allowedMoves, whitePiece);
        }
        //to be run at the end of every move, bool = who is being assessed for check
        private bool findIfInCheck(bool checkingForWhite)
        {
            //find spaces covered by opponent
            findSpacesCovered(!checkingForWhite);
            //check if white's king can be taken by black
            if (checkingForWhite)
            {
                return board[wKingX, wKingY].CoveredByBlack;
            }
            //check opposite
            else
            {
                return board[bKingX, bKingY].CoveredByWhite;
            }
        }
        private bool doesThisMovePutInCheck(bool checkingForWhite, Cell currentCell, Cell newCell)
        {
            movePiece(newCell.OnCell.PosX, newCell.OnCell.PosY, currentCell.OnCell.PosX, currentCell.OnCell.PosY);
            bool does = findIfInCheck(checkingForWhite);
            movePiece(currentCell.OnCell.PosX, currentCell.OnCell.PosY, newCell.OnCell.PosX, newCell.OnCell.PosY);
            return does;
        }
        //resets board for next legal moves to be processed
        public void resetLegal()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j].LegalMove = false;
                }
            }
        }
        //finds spaces covered by a player
        private void findSpacesCovered(bool checkingWhite)
        {
            List<Cell> allowedMoves = new List<Cell>();
            foreach (Cell c in board)
            {
                //runs if piece in space and the piece is the aim colour
                if (c.OnCell.PieceName != "empty" && c.OnCell.IsWhite == checkingWhite)
                {
                    addMovesToList(ref allowedMoves, c, false);
                }
            }
            foreach (Cell c in board)
            {
                if (checkingWhite)
                {
                    if (allowedMoves.Contains(c))
                    {
                        c.CoveredByWhite = true;
                    }
                    else
                    {
                        c.CoveredByWhite = false;
                    }
                }
                else
                {
                    if (allowedMoves.Contains(c))
                    {
                        c.CoveredByBlack = true;
                    }
                    else
                    {
                        c.CoveredByBlack = false;
                    }
                }
            }
        }
        //returns true if a cell is empty or has a piece belonging to the opponent
        private void checkLegal(int posX, int posY, ref List<Cell> allowedMoves, bool whitePiece)
        {
            if (SpaceStatus(posX, posY, whitePiece) == "empty" || SpaceStatus(posX, posY, whitePiece) == "opponent's piece")
            {
                allowedMoves.Add(board[posX, posY]);
            }
        }
        private void addMovesToList(ref List<Cell> allowedMoves, Cell c, bool checkforcheck)
        {
            switch (c.OnCell.PieceName)
            {
                case ("pawn"):
                    showLegalPawn(c.OnCell.PosX, c.OnCell.PosY, ref allowedMoves, c.OnCell.IsWhite);
                    break;
                case ("rook"):
                    showLegalRook(c.OnCell.PosX, c.OnCell.PosY, ref allowedMoves, c.OnCell.IsWhite);
                    break;
                case ("knight"):
                    showLegalKnight(c.OnCell.PosX, c.OnCell.PosY, ref allowedMoves, c.OnCell.IsWhite);
                    break;
                case ("bishop"):
                    showLegalBishop(c.OnCell.PosX, c.OnCell.PosY, ref allowedMoves, c.OnCell.IsWhite);
                    break;
                case ("queen"):
                    showLegalQueen(c.OnCell.PosX, c.OnCell.PosY, ref allowedMoves, c.OnCell.IsWhite);
                    break;
                case ("king"):
                    showLegalKing(c.OnCell.PosX, c.OnCell.PosY, ref allowedMoves, c.OnCell.IsWhite);
                    break;
            }
            //removes moves that put the player in check
            if (checkforcheck)
            {
                List<Cell> notAllowedMoves = new List<Cell>();
                //adds not allowed moves to new list
                foreach (Cell allowed in allowedMoves)
                {
                    if (doesThisMovePutInCheck(c.OnCell.IsWhite, c, allowed))
                    {
                        notAllowedMoves.Add(allowed);
                    }
                }
                //removes moves from original list
                foreach (Cell nA in notAllowedMoves)
                {
                    allowedMoves.Remove(nA);
                }
            }
        }
    }
}
