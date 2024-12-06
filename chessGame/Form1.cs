using System.Linq;
using static System.Formats.Asn1.AsnWriter;
namespace chessGame
{
    public partial class Form1 : Form
    {
        Board b = new Board();
        Player human = new Player();
        AI AI;
        Button[,] boardDisplay = new Button[8, 8];
        Button lastClicked = new Button();
        TextBox winMessage = new TextBox();
        Label hScore = new Label();
        Label AIScore = new Label();
        int AIDepth;

        public Form1()
        {
            InitializeComponent();
            human.IsWhite = true;
            for (int i = 1; i <= 3; i++)
            {
                comboBox1.Items.Add(i);
            }
            winMessage.Location = new Point(150, (this.Height / 2)-50);
            winMessage.BringToFront();
            winMessage.Multiline = true;
            winMessage.Font = new Font("Showcard Gothic", 30);
            winMessage.MinimumSize = new Size(500, 70);
            winMessage.Size = new Size(500, 70);
            winMessage.Multiline = false;
            Controls.Add(winMessage);
            winMessage.Hide();
            hScore.Hide();
            AIScore.Hide();
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
                    if (boardDisplay[i, j] == lastClicked)
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
                hScore.Text = human.Score.ToString();
                AIScore.Text = AI.Score.ToString();
                List<Piece> AIPiecesPassIn = AI.MyPieces;
                long AIScorePassIn = AI.Score;
                long humanScorePassIn = human.Score;
                List<Piece> humanPiecesPassIn = human.MyPieces;
                b.changeScores(currentX, currentY, pastX, pastY, ref humanScorePassIn, ref humanPiecesPassIn, ref AIPiecesPassIn, ref AIScorePassIn, true);
                AI.MyPieces = AIPiecesPassIn;
                AI.Score = AIScorePassIn;
                human.MyPieces = humanPiecesPassIn;
                human.Score = humanScorePassIn;
                if (b.board[currentX, currentY].OnCell.PieceName != "empty")
                {
                    human.TakenPieces.Add(b.board[currentX, currentY].OnCell);
                    PictureBox pB = new PictureBox();
                    pB.BackColor = Color.Transparent;
                    pB.Size = new Size(50, 50);
                    pB.Image = b.board[currentX, currentY].OnCell.PieceImage;
                    pB.Location = new Point(350 + human.TakenPieces.Count * 50, 10);
                    Controls.Add(pB);
                }
                b.movePiece(currentX, currentY, pastX, pastY, true);
                hScore.Text = human.Score.ToString();
                AIScore.Text = AI.Score.ToString();
                boardDisplay[currentX, currentY].Image = b.board[currentX, currentY].OnCell.PieceImage;
                boardDisplay[currentX, currentY].Refresh();
                boardDisplay[pastX, pastY].Image = b.board[pastX, pastY].OnCell.PieceImage;
                boardDisplay[pastX, pastY].Refresh();
                resetColours();
                b.resetLegal();
                if (b.isGameOver(false) == true)
                {
                    winMessage.Text = "CHECKMATE - YOU WIN!";
                    winMessage.BackColor = Color.HotPink;
                    winMessage.Height = 50;
                    winMessage.Width = 100;
                    winMessage.Show();
                }
                else
                {
                    b.whiteTurn = false;
                    int pieces = AI.TakenPieces.Count;
                    AI.makeMove(ref human, ref b);
                    if (AI.TakenPieces.Count  > pieces)
                    {
                        PictureBox pB = new PictureBox();
                        pB.BackColor = Color.Transparent;
                        pB.Size = new Size(50, 50);
                        pB.Image = b.board[currentX, currentY].OnCell.PieceImage;
                        pB.Location = new Point(350 + AI.TakenPieces.Count * 50, 100);
                        Controls.Add(pB);
                    }
                    refreshBoard();
                    b.whiteTurn = true;
                    if (b.isGameOver(true) == true)
                    {
                        winMessage.Text = "CHECKMATE - AI WINS!";
                        winMessage.BackColor = Color.HotPink;
                        winMessage.Show();
                    }
                }
            }
            else
            {
                resetColours();
                //assumes player is white
                //changes all legal move position background colours to light green
                if (b.board[currentX, currentY].OnCell.IsWhite == human.IsWhite)
                {
                    b.resetLegal();
                    b.FindLegalMoves(currentX, currentY);
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
                lastClicked = clicked;
            }
        }
        //start game
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Hide();
            button1.Hide();
            label2.Hide();
            comboBox1.Hide();
            winMessage.Hide();
            DrawBoard(b.board);
            hScore.Text = "39";
            AIScore.Text = "39";
            hScore.Show();
            AIScore.Show();
            AI = new AI(b, AIDepth, human);
            b.refreshLists(ref human, ref AI);
        }
        public void refreshBoard()
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    boardDisplay[i, j].Image = b.board[i, j].OnCell.PieceImage;
                    boardDisplay[i, j].Refresh();
                }
            }
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
            hScore.Location = new Point(100, 20);
            hScore.Size = new Size(20, 10);
            AIScore.Location = new Point(100, 80);
            AIScore.Size = new Size(20, 10);
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
        private void promotion(Pawn p, Piece toBecome)
        {
            b.board[p.PosX, p.PosY].OnCell = toBecome;
        }
        private void resetColours()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
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
                    boardDisplay[i, j].Refresh();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem)
            {
                case 1:
                    AIDepth = 1;
                    break;
                case 2:
                    AIDepth = 2;
                    break;
                case 3:
                    AIDepth = 3;
                    break;
            }
        }
    }
}
