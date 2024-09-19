using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Board
    {
        //all code assumes player is white side
        private bool whiteTurn = true;
        public Cell[,] board = new Cell[8, 8];
        public bool wInCheck = false;
        public bool bInCheck = false;
        private int bKingX;
        private int bKingY;
        private int wKingX;
        private int wKingY;
        //constructor - sets up chess board
        public Board()
        {
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
        //checks occupation status of a cell
        public string SpaceStatus(int posX, int posY)
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
        public bool doesTakeOutOfCheck(Cell c, int currentX, int currentY)
        {
            int newX = c.OnCell.PosX;
            int newY = c.OnCell.PosY;
            bool result = false;
            //records past coordinates so the move can be reverted
            //makes the move, finds which spaces are covered by which colour, then reverts the move
            movePiece(newX, newY, currentX, currentY);
            //checks if the player whose turn it is has been taken out of check
            if (whiteTurn)
            {
                wInCheck = findIfInCheck(whiteTurn);
            }
            else
            {
                bInCheck = findIfInCheck(!whiteTurn);
            }
            if (whiteTurn && !wInCheck)
            {
                result = true;
            }
            else if (!whiteTurn && !bInCheck)
            {
                result = true;
            }
            movePiece(currentX, currentY, newX, newY);
            return result;
        }
        //updates board array when a piece is moved
        //needs to be updated for taking pieces
        //keeps track of kings
        public void movePiece(int newX, int newY, int pastX, int pastY)
        {
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
            board[pastX, pastY].OnCell.PosX = newX;
            board[pastX, pastY].OnCell.PosY = newY;
            board[newX, newY].OnCell = board[pastX, pastY].OnCell;
            if (!board[newX, newY].OnCell.HasMoved)
            {
                board[newX, newY].OnCell.HasMoved = true;
            }
            board[pastX, pastY].OnCell = new Empty();
            wInCheck = findIfInCheck(true);
            bInCheck = findIfInCheck(false);
        }
        //finds all available moves
        public void FindLegalMoves(int posX, int posY, bool whitePiece)
        {
            List<Cell> allowedMoves = new List<Cell>();
            switch (board[posX, posY].OnCell.PieceName)
            {
                case ("pawn"):
                    showLegalPawn(posX, posY, ref allowedMoves, whitePiece);
                    break;
                case ("rook"):
                    showLegalRook(posX, posY, ref allowedMoves);
                    break;
                case ("knight"):
                    showLegalKnight(posX, posY, ref allowedMoves);
                    break;
                case ("bishop"):
                    showLegalBishop(posX, posY, ref allowedMoves);
                    break;
                case ("queen"):
                    showLegalQueen(posX, posY, ref allowedMoves);
                    break;
                case ("king"):
                    showLegalKing(posX, posY, ref allowedMoves);
                    break;
            }
            if (wInCheck || bInCheck)
            {
                allowMovesThatTakeOutOfCheck(board[posX, posY], ref allowedMoves);
            }
            else
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
        private void allowMovesThatTakeOutOfCheck(Cell current, ref List<Cell> allowedMoves)
        {
            foreach (Cell newPos in allowedMoves)
            {
                if (doesTakeOutOfCheck(newPos, current.OnCell.PosX, current.OnCell.PosY))
                {
                    newPos.LegalMove = true;
                }
                else
                {
                    newPos.LegalMove = false;
                    allowedMoves.Remove(newPos);
                }
            }
        }
        private void showLegalPawn(int posX, int posY, ref List<Cell> allowedMoves, bool whitePiece)
        {
            if (whitePiece)
            {
                if (!board[posX, posY].OnCell.HasMoved)
                {
                    if (SpaceStatus(posX, posY - 2) == "free" && SpaceStatus(posX, posY - 1) == "free")
                    {
                        allowedMoves.Add(board[posX, posY - 2]);
                    }
                }
                if (SpaceStatus(posX, posY - 1) == "free")
                {
                    allowedMoves.Add(board[posX, posY - 1]);
                }
                if (SpaceStatus(posX - 1, posY - 1) == "piece")
                {
                    allowedMoves.Add(board[posX - 1, posY - 1]);
                }
                if (SpaceStatus(posX + 1, posY - 1) == "piece")
                {
                    allowedMoves.Add(board[posX + 1, posY - 1]);
                }
            }
            else
            {
                if (!board[posX, posY].OnCell.HasMoved)
                {
                    if (SpaceStatus(posX, posY + 2) == "free" && SpaceStatus(posX, posY + 1) == "free")
                    {
                        allowedMoves.Add(board[posX, posY + 2]);
                    }
                }
                if (SpaceStatus(posX, posY + 1) == "free")
                {
                    allowedMoves.Add(board[posX, posY + 1]);
                }
                if (SpaceStatus(posX + 1, posY + 1) == "piece")
                {
                    allowedMoves.Add(board[posX + 1, posY + 1]);
                }
                if (SpaceStatus(posX - 1, posY + 1) == "piece")
                {
                    allowedMoves.Add(board[posX - 1, posY + 1]);
                }
            }
        }
        private void showLegalRook(int posX, int posY, ref List<Cell> allowedMoves)
        {
            //up
            for (int i = posY - 1; i >= 0; i--)
            {
                if (SpaceStatus(posX, i) == "free")
                {
                    allowedMoves.Add(board[posX, i]);
                }
                else if (SpaceStatus(posX, i) == "piece")
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
                if (SpaceStatus(posX, i) == "free")
                {
                    allowedMoves.Add(board[posX, i]);
                }
                else if (SpaceStatus(posX, i) == "piece")
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
                if (SpaceStatus(i, posY) == "free")
                {
                    allowedMoves.Add(board[i, posY]);
                }
                else if (SpaceStatus(i, posY) == "piece")
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
                if (SpaceStatus(i, posY) == "free")
                {
                    allowedMoves.Add(board[i, posY]);
                }
                else if (SpaceStatus(i, posY) == "piece")
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
        private void showLegalKnight(int posX, int posY, ref List<Cell> allowedMoves)
        {
            checkLegal(posX - 2, posY - 1, ref allowedMoves);
            checkLegal(posX - 1, posY - 2, ref allowedMoves);
            checkLegal(posX + 1, posY - 2, ref allowedMoves);
            checkLegal(posX + 2, posY - 1, ref allowedMoves);
            checkLegal(posX + 2, posY + 1, ref allowedMoves);
            checkLegal(posX + 1, posY + 2, ref allowedMoves);
            checkLegal(posX - 1, posY + 2, ref allowedMoves);
            checkLegal(posX - 2, posY + 1, ref allowedMoves);
        }
        private void showLegalBishop(int posX, int posY, ref List<Cell> allowedMoves)
        {
            //left side up
            for (int i = 1; SpaceStatus(posX - i, posY - i) != "outside"; i++)
            {
                if (SpaceStatus(posX - i, posY - i) == "piece")
                {
                    allowedMoves.Add(board[posX - i, posY - i]);
                    break;
                }
                else if (SpaceStatus(posX - i, posY - i) == "empty")
                {
                    allowedMoves.Add(board[posX - i, posY - i]);
                }
            }
            //right side up
            for (int i = 1; SpaceStatus(posX + i, posY - i) != "outside"; i++)
            {
                if (SpaceStatus(posX + i, posY - i) == "piece")
                {
                    allowedMoves.Add(board[posX + i, posY - i]);
                    break;
                }
                else if (SpaceStatus(posX + i, posY - i) == "empty")
                {
                    allowedMoves.Add(board[posX + i, posY - i]);
                }
            }
            //left side down
            for (int i = 1; SpaceStatus(posX - i, posY + i) != "outside"; i++)
            {
                if (SpaceStatus(posX - i, posY + i) == "piece")
                {
                    allowedMoves.Add(board[posX - i, posY + i]);
                    break;
                }
                else if (SpaceStatus(posX - i, posY + i) == "empty")
                {
                    allowedMoves.Add(board[posX - i, posY + i]);
                }
            }
            //right side down
            for (int i = 1; SpaceStatus(posX + i, posY + i) != "outside"; i++)
            {
                if (SpaceStatus(posX + i, posY + i) == "piece")
                {
                    allowedMoves.Add(board[posX + i, posY + i]);
                    break;
                }
                else if (SpaceStatus(posX + i, posY + i) == "empty")
                {
                    allowedMoves.Add(board[posX + i, posY + i]);
                }
            }
        }
        private void showLegalQueen(int posX, int posY, ref List<Cell> allowedMoves)
        {
            showLegalBishop(posX, posY, ref allowedMoves);
            showLegalRook(posX, posY, ref allowedMoves);
        }
        private void showLegalKing(int posX, int posY, ref List<Cell> allowedMoves)
        {
            checkLegal(posX - 1, posY, ref allowedMoves);
            checkLegal(posX - 1, posY + 1, ref allowedMoves);
            checkLegal(posX - 1, posY - 1, ref allowedMoves);
            checkLegal(posX, posY - 1, ref allowedMoves);
            checkLegal(posX, posY + 1, ref allowedMoves);
            checkLegal(posX + 1, posY, ref allowedMoves);
            checkLegal(posX + 1, posY - 1, ref allowedMoves);
            checkLegal(posX + 1, posY + 1, ref allowedMoves);
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
                    int x = c.OnCell.PosX;
                    int y = c.OnCell.PosY;
                    switch (c.OnCell.PieceName)
                    {
                        case ("pawn"):
                            showLegalPawn(x, y, ref allowedMoves, c.OnCell.IsWhite);
                            break;
                        case ("rook"):
                            showLegalRook(x, y, ref allowedMoves);
                            break;
                        case ("knight"):
                            showLegalKnight(x, y, ref allowedMoves);
                            break;
                        case ("bishop"):
                            showLegalBishop(x, y, ref allowedMoves);
                            break;
                        case ("queen"):
                            showLegalQueen(x, y, ref allowedMoves);
                            break;
                        case ("king"):
                            showLegalKing(x, y, ref allowedMoves);
                            break;
                    }
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
        private void checkLegal(int posX, int posY, ref List<Cell> allowedMoves)
        {
            if (SpaceStatus(posX, posY) == "free" || SpaceStatus(posX, posY) == "piece")
            {
                allowedMoves.Add(board[posX, posY]);
            }
        }
    }
}
