using System.Linq;
namespace chessGame
{
    public partial class Form1 : Form
    {
        Board b = new Board();
        Player human = new Player();
        AI AI = new AI();
        Button[,] boardDisplay = new Button[8, 8];
        Button lastClicked = new Button();
        public Form1()
        {
            InitializeComponent();
            human.IsWhite = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //if player clicks board/button
        private void board_Click(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            int currentX = 0;
            int currentY = 0;
            int pastX = 0;
            int pastY = 0;
            //finds location of the last button clicked and the current button
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (boardDisplay[i, j] == clicked)
                    {
                        currentX = i;
                        currentY = j;
                    }
                    if (boardDisplay[i,j] == lastClicked)
                    {
                        pastX = i;
                        pastY = j;
                    }
                }
            }
            //moves piece if one has been selected and the clicked button is a legal move
            Cell location = b.board[currentX, currentY];
            if (location.LegalMove)
            {
                if (location.OnCell.PieceName != "empty" && location.OnCell.IsWhite == !human.IsWhite)
                {
                    human.Score += location.OnCell.Value;
                    human.TakenPieces.Add(location.OnCell);
                }
                else if (location.OnCell.PieceName != "empty" && location.OnCell.IsWhite == !AI.IsWhite)
                {
                    AI.Score += b.board[currentX, currentY].OnCell.Value;
                    AI.TakenPieces.Add(b.board[currentX, currentY].OnCell);
                }
                b.movePiece(currentX, currentY, pastX, pastY);
                boardDisplay[currentX, currentY].Image = b.board[currentX, currentY].OnCell.PieceImage;
                boardDisplay[currentX, currentY].Refresh();
                boardDisplay[pastX, pastY].Image = b.board[pastX, pastY].OnCell.PieceImage;
                boardDisplay[pastX, pastY].Refresh();
                resetColours();
                b.resetLegal();
                //b.endOfTurn();
            }
            else
            {
                resetColours();
                //assumes player is white
                //changes all legal move position background colours to light green
                if (b.board[currentX, currentY].OnCell.IsWhite == human.IsWhite)
                {
                    b.resetLegal();
                    b.FindLegalMoves(currentX, currentY, true);
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (b.board[i, j].LegalMove)
                            {
                                boardDisplay[i, j].BackColor = Color.LightGreen;
                                boardDisplay[i, j].Refresh();
                            }
                        }
                    }
                }
                else if (b.board[currentX, currentY].OnCell.IsWhite == AI.IsWhite)
                {
                    
                }
                lastClicked = clicked;
            }
        }
        //start game
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Hide();
            button1.Hide();
            DrawBoard(b.board);
        }
        //displaying game board
        private void DrawBoard(Cell[,] b)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    boardDisplay[i, j] = new Button();
                    boardDisplay[i, j].Height = 50;
                    boardDisplay[i, j].Width = 50;
                    boardDisplay[i, j].Image = b[i, j].OnCell.PieceImage;
                    //setting colours
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                        {
                            boardDisplay[i, j].BackColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            boardDisplay[i, j].BackColor = System.Drawing.Color.BurlyWood;
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            boardDisplay[i, j].BackColor = System.Drawing.Color.BurlyWood;
                        }
                        else
                        {
                            boardDisplay[i, j].BackColor = System.Drawing.Color.White;
                        }
                    }
                    boardDisplay[i, j].Location = new Point(i * 50, j * 50);
                    boardDisplay[i, j].Show();
                    boardDisplay[i, j].BringToFront();
                    //if button clicked starts board_Click
                    boardDisplay[i, j].Click += new EventHandler(board_Click);
                    this.Controls.Add(boardDisplay[i, j]);
                }
            }
            //timer for the players
            //System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            //t.Interval = 600000;
            //t.Start();
            //t.Tick += new EventHandler(timer_tick);
        }
        //updates text box with timer value each second
        private void timer_tick(object sender, EventArgs e)
        {
            TextBox timerShow = new TextBox();
        }
        private void promotion()
        {

        }
        private void resetColours()
        {
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 0)
                        {
                            boardDisplay[i, j].BackColor = Color.White;
                        }
                        else
                        {
                            boardDisplay[i, j].BackColor = Color.BurlyWood;
                        }
                    }
                    else
                    {
                        if (j % 2 == 0)
                        {
                            boardDisplay[i, j].BackColor = Color.BurlyWood;
                        }
                        else
                        {
                            boardDisplay[i, j].BackColor = Color.White;
                        }
                    }
                    boardDisplay[i,j].Refresh();
                }
            }
        }
    }
}
