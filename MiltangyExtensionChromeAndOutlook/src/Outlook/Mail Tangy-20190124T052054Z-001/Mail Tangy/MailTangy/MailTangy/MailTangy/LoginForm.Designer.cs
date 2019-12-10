namespace MailTangy
{
    partial class LoginForm
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
            this.loginCustomPaneUserForm = new MailTangy.LoginCustomPane();
            this.SuspendLayout();
            // 
            // loginCustomPaneUserForm
            // 
            this.loginCustomPaneUserForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loginCustomPaneUserForm.Location = new System.Drawing.Point(0, 0);
            this.loginCustomPaneUserForm.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.loginCustomPaneUserForm.Name = "loginCustomPaneUserForm";
            this.loginCustomPaneUserForm.Size = new System.Drawing.Size(500, 609);
            this.loginCustomPaneUserForm.TabIndex = 0;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 609);
            this.Controls.Add(this.loginCustomPaneUserForm);
            this.Font = new System.Drawing.Font("Calibri Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Salesforce Login ";
            this.ResumeLayout(false);

        }

        #endregion

        private LoginCustomPane loginCustomPaneUserForm;
    }
}