namespace chessGame
{
    partial class quitMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            quitButton = new Button();
            menuButton = new Button();
            SuspendLayout();
            // 
            // quitButton
            // 
            quitButton.BackColor = Color.MediumSeaGreen;
            quitButton.Location = new Point(126, 86);
            quitButton.Name = "quitButton";
            quitButton.Size = new Size(84, 23);
            quitButton.TabIndex = 0;
            quitButton.Text = "Quit";
            quitButton.UseVisualStyleBackColor = false;
            quitButton.Click += quitButton_Click;
            // 
            // menuButton
            // 
            menuButton.BackColor = Color.MediumSeaGreen;
            menuButton.Location = new Point(126, 43);
            menuButton.Name = "menuButton";
            menuButton.Size = new Size(84, 23);
            menuButton.TabIndex = 1;
            menuButton.Text = "Main Menu";
            menuButton.UseVisualStyleBackColor = false;
            menuButton.Click += menuButton_Click;
            // 
            // quitMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkSeaGreen;
            ClientSize = new Size(330, 152);
            Controls.Add(menuButton);
            Controls.Add(quitButton);
            Name = "quitMenu";
            Text = "quitMenu";
            ResumeLayout(false);
        }

        #endregion

        private Button quitButton;
        private Button menuButton;
    }
}