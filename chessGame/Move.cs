using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Move
    {
        public int fromX, fromY, toX, toY;
        public bool promoted;
        public Piece pieceMoving;
        public Piece capturedPiece;
        public Piece promotedPiece;

        public Move(int FromX, int FromY, int ToX, int ToY, bool Promoted, Piece PieceMoving, Piece CapturedPiece)
        {
            fromX = FromX;
            fromY = FromY;
            toX = ToX;
            toY = ToY;
            promoted = Promoted;
            capturedPiece = CapturedPiece;
            pieceMoving = PieceMoving;
        }
        public void setPromotion(Piece PromotedPiece)
        {
            promotedPiece = PromotedPiece;
        }
    }
}
