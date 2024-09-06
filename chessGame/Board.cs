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
        private Piece pieceBefore;
        private Piece pieceAfter;
        public bool wInCheck = false;
        public bool bInCheck = false;
        private int bKingX = 4;
        private int bKingY = 0;
        private int wKingX = 4;
        private int wKingY = 7;
        private List<Cell> allowedMoves = new List<Cell>();
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
        private void setLastPosition (int x, int y, Piece p)
        {
            
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
        public bool doesTakeOutOfCheck(Piece p, int newX, int newY)
        {
            int pastX = p.PosX;
            int pastY = p.PosY;
            movePiece(newX, newY, pastX, pastY);
            findSpacesCovered();
            if (findIfInCheck(whiteTurn))
            {
                movePiece(pastX, pastY, newX, newY);
                return false;
            }
            return true;
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
            if (board[newX, newY].OnCell.PieceName == "king")
            {
                if (whiteTurn)
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
            board[pastX, pastY].OnCell = new Empty();
        }
        //finds all available moves
        public void FindLegalMoves(int posX, int posY)
        {
            allowedMoves = new List<Cell>();
            switch (board[posX, posY].OnCell.PieceName)
            {
                case ("pawn"):
                    showLegalPawn(posX, posY);
                    break;
                case ("rook"):
                    showLegalRook(posX, posY);
                    break;
                case ("knight"):
                    showLegalKnight(posX, posY);
                    break;
                case ("bishop"):
                    showLegalBishop(posX, posY);
                    break;
                case ("queen"):
                    showLegalQueen(posX, posY);
                    break;
                case ("king"):
                    showLegalKing(posX, posY);
                    break;
            }
            if (whiteTurn && wInCheck)
            {
                foreach (Cell c in allowedMoves)
                {
                    if (!doesTakeOutOfCheck(c))
                    {
                        allowedMoves.Remove(c);
                    }
                    else
                    {
                        c.LegalMove = true;
                    }
                }
            }
            else
            {
                foreach (Cell c in allowedMoves)
                {
                    c.LegalMove = true;
                }
            }
        }
        private void allowMovesThatTakeOutOfCheck()
        {
            if (whiteTurn && wInCheck)
            {
                foreach (Cell c in allowedMoves)
                {
                    if (!doesTakeOutOfCheck(c))
                    {
                        allowedMoves.Remove(c);
                    }
                }
            }
            else
            {
                foreach (Cell c in allowedMoves)
                {
                    c.LegalMove = true;
                }
            }
        }
        private void showLegalPawn(int posX, int posY)
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
        private void showLegalRook(int posX, int posY)
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
        private void showLegalKnight(int posX, int posY)
        {
            checkLegal(posX - 2, posY - 1);
            checkLegal(posX - 1, posY - 2);
            checkLegal(posX + 1, posY - 2);
            checkLegal(posX + 2, posY - 1);
            checkLegal(posX + 2, posY + 1);
            checkLegal(posX + 1, posY + 2);
            checkLegal(posX - 1, posY + 2);
            checkLegal(posX - 2, posY + 1);
        }
        private void showLegalBishop(int posX, int posY)
        {
            //left side up
            for (int i = 1; SpaceStatus(posX - i, posY - i) != "outside"; i++)
            {
                if (SpaceStatus(posX - i, posY - i) == "own piece")
                {
                    board[posX - i, posY - i].LegalMove = false;
                    break;
                }
                else if (SpaceStatus(posX - i, posY - i) == "piece")
                {
                    board[posX - i, posY - i].LegalMove = true;
                    break;
                }
                else
                {
                    board[posX - i, posY - i].LegalMove = true;
                }
            }
            //right side up
            for (int i = 1; SpaceStatus(posX + i, posY - i) != "outside"; i++)
            {
                if (SpaceStatus(posX + i, posY - i) == "own piece")
                {
                    board[posX + i, posY - i].LegalMove = false;
                    break;
                }
                else if (SpaceStatus(posX + i, posY - i) == "piece")
                {
                    board[posX + i, posY - i].LegalMove = true;
                    break;
                }
                else
                {
                    board[posX + i, posY - i].LegalMove = true;
                }
            }
            //left side down
            for (int i = 1; SpaceStatus(posX - i, posY + i) != "outside"; i++)
            {
                if (SpaceStatus(posX - i, posY + i) == "own piece")
                {
                    board[posX - i, posY + i].LegalMove = false;
                    break;
                }
                else if (SpaceStatus(posX - i, posY + i) == "piece")
                {
                    board[posX - i, posY + i].LegalMove = true;
                    break;
                }
                else
                {
                    board[posX - i, posY + i].LegalMove = true;
                }
            }
            //right side down
            for (int i = 1; SpaceStatus(posX + i, posY + i) != "outside"; i++)
            {
                if (SpaceStatus(posX + i, posY + i) == "own piece")
                {
                    board[posX + i, posY + i].LegalMove = false;
                    break;
                }
                else if (SpaceStatus(posX + i, posY + i) == "piece")
                {
                    board[posX + i, posY + i].LegalMove = true;
                    break;
                }
                else
                {
                    board[posX + i, posY + i].LegalMove = true;
                }
            }
        }
        private void showLegalQueen(int posX, int posY)
        {
            showLegalBishop(posX, posY);
            showLegalRook(posX, posY);
        }
        private void showLegalKing(int posX, int posY)
        {

        }
        //to be run at the end of every move
        private void findIfInCheck(bool white)
        {
            if (white)
            {
                if (board[bKingX, bKingY].CoveredByWhite)
                {
                    bInCheck = true;
                }
                else
                {
                    bInCheck = false;
                }
            }
            else
            {
                if (board[wKingX, wKingY].CoveredByBlack)
                {
                    wInCheck = true;
                }
                else
                {
                    wInCheck = false;
                }
            }
        }
        public void endOfMove()
        {
            //checks if king is in check for the next move
            findIfInCheck(true);
            findIfInCheck(false);
            resetLegal();
            //keeps track of whose turn it is
            if (whiteTurn)
            {
                whiteTurn = false;
            }
            else
            {
                whiteTurn = true;
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
        //checks if a player is in check
        private void findSpacesCovered(bool forWhite)
        {
            foreach (Cell c in board)
            {
                if (c.OnCell.PieceName != "empty" && c.OnCell.IsWhite == forWhite)
                {
                    int x = c.OnCell.PosX;
                    int y = c.OnCell.PosY;
                    switch (c.OnCell.PieceName)
                    {
                        case ("pawn"):
                            showLegalPawn(x, y);
                            break;
                        case ("rook"):
                            showLegalRook(x, y);
                            break;
                        case ("knight"):
                            showLegalKnight(x, y);
                            break;
                        case ("bishop"):
                            showLegalBishop(x, y);
                            break;
                        case ("queen"):
                            showLegalQueen(x, y);
                            break;
                        case ("king"):
                            break;
                    }
                }
            }
            foreach (Cell c in board)
            {
                if (forWhite)
                {
                    if (c.LegalMove)
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
                    if (c.LegalMove)
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
        private void checkLegal(int posX, int posY)
        {
            if (SpaceStatus(posX, posY) == "free")
            {
                board[posX, posY].LegalMove = true;
            }
            else if (SpaceStatus(posX, posY) == "own piece")
            {
                board[posX, posY].LegalMove = false;
            }
            else if (SpaceStatus(posX, posY) == "piece")
            {
                board[posX, posY].LegalMove = true;
            }
        }
    }
}
