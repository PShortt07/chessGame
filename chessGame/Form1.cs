using System.Linq;
namespace chessGame
{
    public partial class Form1 : Form
    {
        bool gameActive = false;
        Board b = new Board();
        Button[,] boardDisplay = new Button[8, 8];
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        //if player clicks board/button
        private void board_Click(object sender, EventArgs e)
        {
            resetColours();
            int xPos = 0;
            int yPos = 0;
            Button clicked = (Button)sender;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (boardDisplay[i, j] == clicked)
                    {
                        xPos = i;
                        yPos = j;
                        break;
                    }
                }
            }
            //fix later
            if (b.board[xPos, yPos].OnCell.IsWhite)
            {
                b.LegalMoves(xPos, yPos);
                for (int i = 0;i < 8;i++)
                {
                    for(int j = 0;j < 8;j++)
                    {
                        if (b.board[i,j].LegalMove)
                        {
                            boardDisplay[i,j].BackColor = Color.LightGreen;
                            boardDisplay[i, j].Refresh();
                        }
                    }
                }
            }
        }
        //start game
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Hide();
            button1.Hide();
            DrawBoard(b.board);
            gameActive = true;
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
                    boardDisplay[i, j].Image = new Bitmap(b[i, j].OnCell.ImageLocation);
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
