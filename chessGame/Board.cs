using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class Board
    {
        int height = 8;
        int width = 8;
        public Board()
        {

        }
        public void SetBoard()
        {
            Piece[,] board = new Piece[height, width];
        }
        public void DrawBoard(Piece[,] board)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; i < width; i++)
                {

                }
            }
        }
    }
}
