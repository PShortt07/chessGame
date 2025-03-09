using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chessGame
{
    public partial class quitMenu : Form
    {
        Game currentGame;
        public quitMenu(Game CurrentGame)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            currentGame = CurrentGame;
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            //closes current game and opens a new window for the main menu
            currentGame.Close();
            Game f1 = new Game(currentGame.playerUsername);
            f1.Show();
            this.Close();
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            //closes all windows
            Application.Exit();
        }
    }
}
