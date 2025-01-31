using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using static System.Formats.Asn1.AsnWriter;

namespace chessGame
{
    public partial class loginPage : Form
    {
        public loginPage()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(64, 61, 58);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void loginPage_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void logInButton_Click(object sender, EventArgs e)
        {
            //Home: Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\patri\Source\Repos\chessGame2\chessGame\scores.mdf;Integrated Security=True
            //College: Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=H:\CS\chessGameSBID\chessGame\Database1.mdf;Integrated Security=True
            SqlConnection scoresCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=H:\CS\chessGameSBID\chessGame\Database1.mdf;Integrated Security=True");
            scoresCon.Open();
            //fix this
            string toInsert = "SELECT * FROM [userLogins] WHERE Username = @username AND Password = @password";
            SqlCommand cmd = new SqlCommand(toInsert, scoresCon);
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);
            var found = cmd.ExecuteScalar();
            scoresCon.Close();
            if (found != null)
            {
                Form1 f1 = new Form1(username);
                f1.Show();
                this.Hide();
            }
            else
            {
                errorMessage.Text = "Account not found";
            }
        }

        private void signUpButton_Click(object sender, EventArgs e)
        {
            string sChars = @" !#$%&'()*+,-./:;<=>?@[\]^_`{|}~";
            string nums = "0123456789";
            string capLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string proposedPassword = passwordTextBox.Text;
            bool safe = true;
            if (proposedPassword.IndexOfAny(sChars.ToCharArray()) == -1)
            {
                errorMessage.Text = "Password needs at least one special character";
                safe = false;
            }
            if (proposedPassword.IndexOfAny(nums.ToCharArray()) == -1)
            {
                errorMessage.Text = "Password needs at least one number";
                safe = false;
            }
            if (proposedPassword.IndexOfAny(capLetters.ToCharArray()) == -1)
            {
                errorMessage.Text = "Password needs at least one capital letter";
                safe = false;
            }
            if (safe)
            {
                //Home: Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\patri\Source\Repos\chessGame2\chessGame\scores.mdf;Integrated Security=True
                //College: Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=H:\CS\chessGameSBID\chessGame\Database1.mdf;Integrated Security=True
                SqlConnection scoresCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=H:\CS\chessGameSBID\chessGame\Database1.mdf;Integrated Security=True");
                scoresCon.Open();
                string toInsert = "INSERT INTO [userLogins] (Username, Password) VALUES (@u, @p)";
                SqlCommand cmd = new SqlCommand(toInsert, scoresCon);
                string username = usernameTextBox.Text;
                string password = passwordTextBox.Text;
                cmd.Parameters.AddWithValue("u", username);
                cmd.Parameters.AddWithValue("p", password);
                bool goneThrough = true;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    errorMessage.Text = "Username taken. Please try another.";
                    goneThrough = false;
                }
                scoresCon.Close();
                if (goneThrough)
                {
                    Form1 f1 = new Form1(username);
                    f1.Show();
                    this.Hide();
                }
            }
        }
    }
}
