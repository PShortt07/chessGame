using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    abstract class Piece
    {
        private int value;
        private int posX;
        private int posY;
        private bool isWhite;
        private string pieceName;
        private bool hasMoved;
        private Image pieceImage;
        private bool empty;
        //stack idea inspired by https://github.com/apostolisv/chess-ai
        private Stack<Piece> lastTaken;
        private Stack<int> lastX;
        private Stack<int> lastY;

        public int Value { get => value; set => this.value = value; }
        public int PosX { get => posX; set => posX = value; }
        public int PosY { get => posY; set => posY = value; }
        public bool IsWhite { get => isWhite; set => isWhite = value; }
        public string PieceName { get => pieceName; set => pieceName = value; }
        public bool HasMoved { get => hasMoved; set => hasMoved = value; }
        public Image PieceImage { get => pieceImage; set => pieceImage = value; }
        public bool Empty { get => empty; set => empty = value; }
        internal Stack<Piece> LastTaken { get => lastTaken; set => lastTaken = value; }
        public Stack<int> LastX { get => lastX; set => lastX = value; }
        public Stack<int> LastY { get => lastY; set => lastY = value; }

        public Piece(bool white, int x, int y)
        {
            IsWhite = white;
            PosX = x;
            PosY = y;
            HasMoved = false;
            Empty = false;
            LastTaken = new Stack<Piece>();
            LastX = new Stack<int>();
            LastY = new Stack<int>();
            LastX.Push(x);
            LastY.Push(y);
        }
        public void updateLastX(int x)
        {
            LastX.Push(x);
        }
        public void updateLastY(int y)
        {
            LastY.Push(y);
        }
        public int retrieveLastX()
        {
            return LastX.Pop();
        }
        public int retrieveLastY()
        {
            return LastY.Pop();
        }
    }
}
