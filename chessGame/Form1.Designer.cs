using System.Runtime.CompilerServices;

namespace chessGame
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            playButton = new Button();
            titleLabel = new Label();
            difficultySelector = new ComboBox();
            difficultyLabel = new Label();
            leaderboardButton = new Button();
            underline = new PictureBox();
            returnToMenu = new Button();
            ((System.ComponentModel.ISupportInitialize)underline).BeginInit();
            SuspendLayout();
            // 
            // playButton
            // 
            playButton.BackColor = Color.DarkSeaGreen;
            playButton.Font = new Font("Century Schoolbook", 15.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            playButton.Location = new Point(589, 269);
            playButton.Name = "playButton";
            playButton.Size = new Size(178, 84);
            playButton.TabIndex = 1;
            playButton.Text = "Play";
            playButton.UseVisualStyleBackColor = false;
            playButton.Click += playButton_Click;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.BackColor = Color.Thistle;
            titleLabel.Font = new Font("Century Schoolbook", 50F, FontStyle.Italic, GraphicsUnit.Point, 0);
            titleLabel.Location = new Point(441, 131);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(471, 79);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Elegant Chess";
            titleLabel.Click += label1_Click;
            // 
            // difficultySelector
            // 
            difficultySelector.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            difficultySelector.FormattingEnabled = true;
            difficultySelector.Location = new Point(850, 299);
            difficultySelector.Name = "difficultySelector";
            difficultySelector.Size = new Size(121, 33);
            difficultySelector.TabIndex = 2;
            difficultySelector.SelectedIndexChanged += difficultySelector_SelectedIndexChanged;
            // 
            // difficultyLabel
            // 
            difficultyLabel.AutoSize = true;
            difficultyLabel.Font = new Font("Century Schoolbook", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            difficultyLabel.Location = new Point(863, 277);
            difficultyLabel.Name = "difficultyLabel";
            difficultyLabel.Size = new Size(93, 19);
            difficultyLabel.TabIndex = 3;
            difficultyLabel.Text = "Difficulty:";
            // 
            // leaderboardButton
            // 
            leaderboardButton.BackColor = Color.SeaGreen;
            leaderboardButton.Font = new Font("Century Schoolbook", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            leaderboardButton.Location = new Point(375, 281);
            leaderboardButton.Name = "leaderboardButton";
            leaderboardButton.Size = new Size(139, 58);
            leaderboardButton.TabIndex = 5;
            leaderboardButton.Text = "Leaderboard";
            leaderboardButton.UseVisualStyleBackColor = false;
            leaderboardButton.Click += leaderboardButton_Click;
            // 
            // underline
            // 
            underline.BackColor = Color.Transparent;
            underline.BackgroundImage = Properties.Resources.underline;
            underline.Location = new Point(327, 325);
            underline.Name = "underline";
            underline.Size = new Size(706, 302);
            underline.TabIndex = 6;
            underline.TabStop = false;
            // 
            // returnToMenu
            // 
            returnToMenu.Location = new Point(618, 429);
            returnToMenu.Name = "returnToMenu";
            returnToMenu.Size = new Size(126, 42);
            returnToMenu.TabIndex = 7;
            returnToMenu.Text = "Main Menu";
            returnToMenu.UseVisualStyleBackColor = true;
            returnToMenu.Click += returnToMenu_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(192, 255, 192);
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1330, 693);
            Controls.Add(returnToMenu);
            Controls.Add(underline);
            Controls.Add(leaderboardButton);
            Controls.Add(difficultyLabel);
            Controls.Add(difficultySelector);
            Controls.Add(playButton);
            Controls.Add(titleLabel);
            KeyPreview = true;
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)underline).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button playButton;
        private Label titleLabel;
        private ComboBox difficultySelector;
        private Label difficultyLabel;
        private Button leaderboardButton;
        private PictureBox underline;
        private Button returnToMenu;
    }
}
