
namespace WinForms
{
    partial class LogInForm
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
            this.labelLIFUsername = new System.Windows.Forms.Label();
            this.labelLIFPassword = new System.Windows.Forms.Label();
            this.textBoxLIFUsername = new System.Windows.Forms.TextBox();
            this.textBoxLIFPassword = new System.Windows.Forms.TextBox();
            this.buttonLIFLogIn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelLIFUsername
            // 
            this.labelLIFUsername.Font = new System.Drawing.Font("Bahnschrift SemiBold SemiConden", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLIFUsername.Location = new System.Drawing.Point(53, 87);
            this.labelLIFUsername.Name = "labelLIFUsername";
            this.labelLIFUsername.Size = new System.Drawing.Size(121, 35);
            this.labelLIFUsername.TabIndex = 0;
            this.labelLIFUsername.Text = "Username";
            this.labelLIFUsername.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLIFPassword
            // 
            this.labelLIFPassword.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLIFPassword.Location = new System.Drawing.Point(53, 159);
            this.labelLIFPassword.Name = "labelLIFPassword";
            this.labelLIFPassword.Size = new System.Drawing.Size(121, 35);
            this.labelLIFPassword.TabIndex = 1;
            this.labelLIFPassword.Text = "Password";
            this.labelLIFPassword.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxLIFUsername
            // 
            this.textBoxLIFUsername.Location = new System.Drawing.Point(227, 98);
            this.textBoxLIFUsername.Name = "textBoxLIFUsername";
            this.textBoxLIFUsername.Size = new System.Drawing.Size(121, 20);
            this.textBoxLIFUsername.TabIndex = 2;
            // 
            // textBoxLIFPassword
            // 
            this.textBoxLIFPassword.Location = new System.Drawing.Point(227, 170);
            this.textBoxLIFPassword.Name = "textBoxLIFPassword";
            this.textBoxLIFPassword.Size = new System.Drawing.Size(121, 20);
            this.textBoxLIFPassword.TabIndex = 3;
            // 
            // buttonLIFLogIn
            // 
            this.buttonLIFLogIn.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonLIFLogIn.Location = new System.Drawing.Point(177, 239);
            this.buttonLIFLogIn.Name = "buttonLIFLogIn";
            this.buttonLIFLogIn.Size = new System.Drawing.Size(121, 43);
            this.buttonLIFLogIn.TabIndex = 4;
            this.buttonLIFLogIn.Text = "Log In";
            this.buttonLIFLogIn.UseVisualStyleBackColor = true;
            this.buttonLIFLogIn.Click += new System.EventHandler(this.buttonLIFLogIn_Click);
            // 
            // LogInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 304);
            this.Controls.Add(this.buttonLIFLogIn);
            this.Controls.Add(this.textBoxLIFPassword);
            this.Controls.Add(this.textBoxLIFUsername);
            this.Controls.Add(this.labelLIFPassword);
            this.Controls.Add(this.labelLIFUsername);
            this.Name = "LogInForm";
            this.Text = "LogInForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelLIFUsername;
        private System.Windows.Forms.Label labelLIFPassword;
        private System.Windows.Forms.TextBox textBoxLIFUsername;
        private System.Windows.Forms.TextBox textBoxLIFPassword;
        private System.Windows.Forms.Button buttonLIFLogIn;
    }
}