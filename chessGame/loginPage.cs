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
        private string hashPassword(string password)
        {
            //mid square hasing algorithm
            char[] passwordChars = password.ToCharArray();
            string toHash = "";
            //converts each character to its character code and adds them to the toHash string
            for (int i = 0; i < passwordChars.Count(); i++)
            {
                int code = (int)passwordChars[i];
                toHash += code;
            }
            bool canContinue = true;
            string hashed = "";
            do
            {
                try
                {
                    canContinue = true;
                    //squares the number and converts it into a string
                    hashed = (long.Parse(toHash) ^ 2).ToString();
                }
                catch
                {
                    //accounts for when the number too large to be parsed
                    toHash = toHash.Substring(1, toHash.Length - 1);
                    canContinue = false;
                }
            } while (!canContinue);
            //takes the middle of the string
            hashed = hashed.Substring(3, hashed.Length - 4);
            //ensures the hashed string is not too long to be stored in the database
            if (hashed.Length > 100)
            {
                hashed = hashed.Substring(0, 100);
            }
            return hashed;
        }
        private void logInButton_Click(object sender, EventArgs e)
        {
            //Home: Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\patri\Source\Repos\chessGame2\chessGame\scores.mdf;Integrated Security=True
            //College: Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=H:\CS\chessGameSBID\chessGame\Database1.mdf;Integrated Security=True
            SqlConnection scoresCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=H:\CS\chessGameSBID\chessGame\Database1.mdf;Integrated Security=True");
            scoresCon.Open();
            //checks for account in database
            string toInsert = "SELECT * FROM [userLogins] WHERE Username = @username AND Password = @password";
            SqlCommand cmd = new SqlCommand(toInsert, scoresCon);
            string username = usernameTextBox.Text;
            string password = hashPassword(passwordTextBox.Text);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);
            var found = cmd.ExecuteScalar();
            scoresCon.Close();
            if (found != null)
            {
                //opens main menu and hides login page
                Game f1 = new Game(username);
                f1.Show();
                this.Hide();
            }
            else
            {
                //accounts for there being no match found in the database
                errorMessage.Text = "Account not found";
            }
        }

        private void signUpButton_Click(object sender, EventArgs e)
        {
            //sets of characters to check for in password
            string sChars = @" !#$%&'()*+,-./:;<=>?@[\]^_`{|}~";
            string nums = "0123456789";
            string capLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string proposedPassword = passwordTextBox.Text;
            string username = usernameTextBox.Text;
            bool safe = true;
            //password requirements
            if (username.Length > 25)
            {
                errorMessage.Text = "Username too long - must be less than 25 characters";
                safe = false;
            }
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
            if (proposedPassword.Length < 8)
            {
                errorMessage.Text = "Password too short - must be at least 8 characters";
                safe = false;
            }
            if (safe)
            {
                //Home: Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\patri\Source\Repos\chessGame2\chessGame\scores.mdf;Integrated Security=True
                //College: Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=H:\CS\chessGameSBID\chessGame\Database1.mdf;Integrated Security=True
                //hashes password then stores it in the database
                proposedPassword = hashPassword(proposedPassword);
                SqlConnection scoresCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=H:\CS\chessGameSBID\chessGame\Database1.mdf;Integrated Security=True");
                scoresCon.Open();
                string toInsert = "INSERT INTO [userLogins] (Username, Password) VALUES (@u, @p)";
                SqlCommand cmd = new SqlCommand(toInsert, scoresCon);
                string password = proposedPassword;
                cmd.Parameters.AddWithValue("u", username);
                cmd.Parameters.AddWithValue("p", password);
                bool goneThrough = true;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    //checks if username is taken
                    errorMessage.Text = "Username taken. Please try another.";
                    goneThrough = false;
                }
                scoresCon.Close();
                if (goneThrough)
                {
                    //opens main menu and hides login page
                    Game f1 = new Game(username);
                    f1.Show();
                    this.Hide();
                }
            }
        }
    }
}
