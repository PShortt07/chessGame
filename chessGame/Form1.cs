namespace chessGame
{
    public partial class Form1 : Form
    {
        int height = 8;
        int width = 8;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Hide();
            button1.Hide();
            DrawBoard();
        }
        private void DrawBoard()
        {
            PictureBox[,] boardDisplay = new PictureBox[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    boardDisplay[i, j] = new PictureBox();
                    boardDisplay[i, j].Height = 50;
                    boardDisplay[i, j].Width = 50;
                    boardDisplay[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                    boardDisplay[i, j].Image = new Bitmap(@"C:\Users\AQ231513\Downloads\pawn.png");
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
                        boardDisplay[i, j].BackColor = System.Drawing.Color.BurlyWood;
                    }
                    {
                        boardDisplay[i, j].BackColor = System.Drawing.Color.White;
                    }
                    boardDisplay[i, j].Location = new Point(i * 50, j * 50);
                    boardDisplay[i, j].Show();
                    boardDisplay[i, j].BringToFront();
                    this.Controls.Add(boardDisplay[i, j]);
                }
            }
        }
    }
}
