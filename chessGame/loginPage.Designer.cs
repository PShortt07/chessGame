namespace chessGame
{
    partial class loginPage
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
            mainMessage = new Label();
            logInButton = new Button();
            signUpButton = new Button();
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // mainMessage
            // 
            mainMessage.AutoSize = true;
            mainMessage.Font = new Font("Arial Rounded MT Bold", 30F, FontStyle.Regular, GraphicsUnit.Point, 0);
            mainMessage.ForeColor = Color.White;
            mainMessage.Location = new Point(294, 62);
            mainMessage.Name = "mainMessage";
            mainMessage.Size = new Size(213, 46);
            mainMessage.TabIndex = 0;
            mainMessage.Text = "Welcome!";
            // 
            // logInButton
            // 
            logInButton.BackColor = Color.SeaGreen;
            logInButton.Font = new Font("Arial Rounded MT Bold", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            logInButton.Location = new Point(356, 291);
            logInButton.Name = "logInButton";
            logInButton.Size = new Size(75, 23);
            logInButton.TabIndex = 1;
            logInButton.Text = " Log In";
            logInButton.UseVisualStyleBackColor = false;
            logInButton.Click += logInButton_Click;
            // 
            // signUpButton
            // 
            signUpButton.BackColor = Color.SeaGreen;
            signUpButton.Font = new Font("Arial Rounded MT Bold", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            signUpButton.Location = new Point(356, 332);
            signUpButton.Name = "signUpButton";
            signUpButton.Size = new Size(75, 23);
            signUpButton.TabIndex = 2;
            signUpButton.Text = "Sign Up";
            signUpButton.UseVisualStyleBackColor = false;
            signUpButton.Click += signUpButton_Click;
            // 
            // usernameTextBox
            // 
            usernameTextBox.BackColor = Color.DarkSeaGreen;
            usernameTextBox.Location = new Point(316, 164);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.Size = new Size(166, 23);
            usernameTextBox.TabIndex = 3;
            // 
            // passwordTextBox
            // 
            passwordTextBox.BackColor = Color.DarkSeaGreen;
            passwordTextBox.Location = new Point(316, 226);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.Size = new Size(166, 23);
            passwordTextBox.TabIndex = 4;
            passwordTextBox.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(316, 146);
            label2.Name = "label2";
            label2.Size = new Size(63, 15);
            label2.TabIndex = 5;
            label2.Text = "Username:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.White;
            label3.Location = new Point(316, 208);
            label3.Name = "label3";
            label3.Size = new Size(60, 15);
            label3.TabIndex = 6;
            label3.Text = "Password:";
            label3.Click += label3_Click;
            // 
            // loginPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.WindowFrame;
            ClientSize = new Size(800, 450);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(passwordTextBox);
            Controls.Add(usernameTextBox);
            Controls.Add(signUpButton);
            Controls.Add(logInButton);
            Controls.Add(mainMessage);
            Name = "loginPage";
            Text = "loginPage";
            Load += loginPage_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label mainMessage;
        private Button logInButton;
        private Button signUpButton;
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private Label label2;
        private Label label3;
    }
}