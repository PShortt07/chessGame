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
        Form1 currentForm;
        public quitMenu(Form1 CurrentForm)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            currentForm = CurrentForm;
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            currentForm.Close();
            Form1 f1 = new Form1(currentForm.playerUsername);
            f1.Show();
            this.Close();
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            currentForm.Close();
            this.Close();
        }
    }
}
