using chessGame.Properties;
using System.Linq;
using System.Resources;
using System.Threading;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.Data.SqlClient;
using System.Data;
namespace chessGame
{
    public partial class Form1 : Form
    {
        Board b = new Board();
        AI AI;
        Player human = new Player();
        Button[,] boardDisplay = new Button[8, 8];
        Button lastClicked = new Button();
        TextBox winMessage = new TextBox();
        Label hScore = new Label();
        Label AIScore = new Label();
        int AIDepth;
        int numOfMoves = 0;
        string playerUsername;

        public Form1(string uName)
        {
            playerUsername = uName;
            InitializeComponent();
            human.IsWhite = true;
            for (int i = 1; i <= 3; i++)
            {
                comboBox1.Items.Add(i);
            }
            //winMessage
            winMessage.BringToFront();
            winMessage.Multiline = true;
            winMessage.Font = new Font("Century Schoolbook", 30);
            winMessage.MinimumSize = new Size(500, 70);
            winMessage.Size = new Size(500, 70);
            winMessage.Multiline = false;
            winMessage.TextAlign = HorizontalAlignment.Center;
            winMessage.Height = 50;
            winMessage.Width = 100;
            Controls.Add(winMessage);
            winMessage.Hide();
            //player score
            hScore.Location = new Point(100, 300);
            hScore.MinimumSize = new Size(50, 30);
            hScore.Font = new Font("Century Schoolbook", 20);
            Controls.Add(hScore);
            hScore.Hide();
            //AI score
            AIScore.BackColor = System.Drawing.Color.BurlyWood;
            AIScore.MinimumSize = new Size(50, 30);
            AIScore.Font = new Font("Century Schoolbook", 20);
            Controls.Add(AIScore);
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
                numOfMoves++;
                if (location.OnCell.CanPromote)
                {
                    promotion(location.OnCell);
                }
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
                    updateTakenPieces(currentX, currentY, true);
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
                hScore.Text = (human.Score / 10).ToString();
                AIScore.Text = (AI.Score / 10).ToString();
                if (b.isGameOver(false) == true)
                {
                    if (b.bInCheck)
                    {
                        winMessage.Text = "CHECKMATE - YOU WIN!";
                        //find currently recorded high score for the user
                        string name = playerUsername;
                        string toInsert = "SELECT Moves FROM [highScores] WHERE Username = @name";
                        SqlConnection scoresCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\patri\Source\Repos\chessGame2\chessGame\scores.mdf;Integrated Security=True");
                        scoresCon.Open();
                        SqlCommand cmd = new SqlCommand(toInsert, scoresCon);
                        cmd.Parameters.AddWithValue("name", name);
                        var hS = cmd.ExecuteScalar();
                        //updates score if lower than previous high score
                        if (hS != null && numOfMoves < int.Parse(hS.ToString()))
                        {
                            toInsert = "UPDATE [highScores] SET Moves = @score WHERE Username = @name";
                            int score = numOfMoves;
                            cmd = new SqlCommand(toInsert, scoresCon);
                            cmd.Parameters.AddWithValue("name", name);
                            cmd.Parameters.AddWithValue("score", score);
                            cmd.ExecuteNonQuery();
                        }
                        else if (hS == null)
                        {
                            toInsert = "INSERT INTO [highScores] (Username, Moves) VALUES (@name, @score)";
                            int score = numOfMoves;
                            cmd = new SqlCommand(toInsert, scoresCon);
                            cmd.Parameters.AddWithValue("name", name);
                            cmd.Parameters.AddWithValue("score", score);
                            cmd.ExecuteNonQuery();
                        }
                        scoresCon.Close();
                    }
                    else
                    {
                        winMessage.Text = "STALEMATE - DRAW";
                    }
                    winMessage.Show();
                }
                else
                {
                    b.whiteTurn = false;
                    int pieces = AI.TakenPieces.Count;
                    AI.makeMove(ref human, ref b, this);
                    refreshBoard();
                    b.whiteTurn = true;
                    if (b.isGameOver(true) == true)
                    {
                        if (b.wInCheck)
                        {
                            winMessage.Text = "CHECKMATE - AI WINS!";
                        }
                        else
                        {
                            winMessage.Text = "STALEMATE - DRAW";
                        }
                        winMessage.Show();
                    }
                }
                hScore.Text = (human.Score / 10).ToString();
                AIScore.Text = (AI.Score / 10).ToString();
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
        public void updateTakenPieces(int newX, int newY, bool player)
        {
            PictureBox pB = new PictureBox();
            pB.BackColor = Color.Transparent;
            pB.Size = new Size(50, 50);
            pB.Image = b.board[newX, newY].OnCell.PieceImage;
            if (player)
            {
                pB.Location = new Point(350 + human.TakenPieces.Count * 50, 550);
            }
            else
            {
                pB.Location = new Point(350 + AI.TakenPieces.Count * 50, 20);
            }
            Controls.Add(pB);
        }
        //start game
        private void button1_Click(object sender, EventArgs e)
        {
            leaderboardButton.Hide();
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
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
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
                    boardDisplay[i, j].Location = new Point(470 + i * 50, 120 + j * 50);
                    boardDisplay[i, j].Show();
                    boardDisplay[i, j].BringToFront();
                    //if button clicked starts board_Click
                    boardDisplay[i, j].Click += new EventHandler(board_Click);
                    Controls.Add(boardDisplay[i, j]);
                }
            }
            hScore.Location = new Point(100, 20);
            hScore.Size = new Size(20, 10);
            AIScore.Location = new Point(100, 80);
            AIScore.Size = new Size(20, 10);
        }
        private void promotion(Piece p)
        {
            Button opQ = new Button();
            opQ.Image = Resources.queen;
            Button opR = new Button();
            Button opK = new Button();
            Button opB = new Button();
            Button[] options = new Button[4];
            for (int i = 0; i < options.Length; i++)
            {
                options[i].Height = 50;
                options[i].Width = 50;
                options[i].BringToFront();
                options[i].Location = new Point(450, 150 + i * 50);
                Controls.Add(options[i]);
            }
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
                    AIDepth = 4;
                    break;
            }
        }

        private void leaderboardButton_Click(object sender, EventArgs e)
        {
            SqlConnection scoresCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\patri\source\repos\chessGame2\chessGame\scores.mdf;Integrated Security=True");
            scoresCon.Open();
            string sql = "SELECT * FROM [highScores]";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, scoresCon);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            DataGridView leaderboardDisplay = new DataGridView();
            leaderboardDisplay.DataSource = dt;
            leaderboardDisplay.Width = 400;
            leaderboardDisplay.Height = 500;
            leaderboardDisplay.AutoGenerateColumns = true;
            Controls.Add(leaderboardDisplay);
            scoresCon.Close();
        }
    }
}
