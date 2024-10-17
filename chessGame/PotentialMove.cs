using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chessGame
{
    internal class PotentialMove
    {
        public Piece p;
        public Cell newCell;
        public double value;
        public PotentialMove(double value, Piece p, Cell newCell)
        {
            this.value = value;
            this.p = p;
            this.newCell = newCell;
        }
    }
}
