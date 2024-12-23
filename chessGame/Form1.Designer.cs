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
            button1 = new Button();
            label1 = new Label();
            comboBox1 = new ComboBox();
            label2 = new Label();
            enterName = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(599, 258);
            button1.Name = "button1";
            button1.Size = new Size(133, 57);
            button1.TabIndex = 1;
            button1.Text = "PLAY";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Thistle;
            label1.Font = new Font("Century Schoolbook", 50F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(441, 131);
            label1.Name = "label1";
            label1.Size = new Size(471, 79);
            label1.TabIndex = 0;
            label1.Text = "Elegant Chess";
            label1.Click += label1_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(1073, 163);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(121, 23);
            comboBox1.TabIndex = 2;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(1107, 145);
            label2.Name = "label2";
            label2.Size = new Size(58, 15);
            label2.TabIndex = 3;
            label2.Text = "Difficulty:";
            // 
            // enterName
            // 
            enterName.Location = new Point(632, 403);
            enterName.Name = "enterName";
            enterName.Size = new Size(75, 23);
            enterName.TabIndex = 4;
            enterName.Text = "Done";
            enterName.UseVisualStyleBackColor = true;
            enterName.Click += enterName_Click;
            // 
            // button2
            // 
            button2.Location = new Point(475, 275);
            button2.Name = "button2";
            button2.Size = new Size(85, 23);
            button2.TabIndex = 5;
            button2.Text = "Leaderboard";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(192, 255, 192);
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1330, 693);
            Controls.Add(button2);
            Controls.Add(enterName);
            Controls.Add(label2);
            Controls.Add(comboBox1);
            Controls.Add(button1);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private ComboBox comboBox1;
        private Label label2;
        private Button enterName;
        private Button button2;
    }
}
