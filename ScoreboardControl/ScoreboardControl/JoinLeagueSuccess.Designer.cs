namespace ScoreboardControl
{
    partial class JoinLeagueSuccess
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
            this.pbLeagueLogo = new System.Windows.Forms.PictureBox();
            this.lblLeagueName = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblLoginKey = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbLeagueLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pbLeagueLogo
            // 
            this.pbLeagueLogo.Location = new System.Drawing.Point(261, 129);
            this.pbLeagueLogo.Name = "pbLeagueLogo";
            this.pbLeagueLogo.Size = new System.Drawing.Size(258, 134);
            this.pbLeagueLogo.TabIndex = 0;
            this.pbLeagueLogo.TabStop = false;
            // 
            // lblLeagueName
            // 
            this.lblLeagueName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblLeagueName.AutoSize = true;
            this.lblLeagueName.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLeagueName.Location = new System.Drawing.Point(196, 35);
            this.lblLeagueName.Name = "lblLeagueName";
            this.lblLeagueName.Size = new System.Drawing.Size(286, 29);
            this.lblLeagueName.TabIndex = 1;
            this.lblLeagueName.Text = "Now Viewing Page for: ";
            this.lblLeagueName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(349, 316);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblLoginKey
            // 
            this.lblLoginKey.AutoSize = true;
            this.lblLoginKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginKey.Location = new System.Drawing.Point(197, 84);
            this.lblLoginKey.Name = "lblLoginKey";
            this.lblLoginKey.Size = new System.Drawing.Size(141, 20);
            this.lblLoginKey.TabIndex = 3;
            this.lblLoginKey.Text = "Unique Login Key: ";
            // 
            // JoinLeagueSuccess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblLoginKey);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.lblLeagueName);
            this.Controls.Add(this.pbLeagueLogo);
            this.Name = "JoinLeagueSuccess";
            this.Text = "JoinLeagueSuccess";
            this.Load += new System.EventHandler(this.JoinLeagueSuccess_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLeagueLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbLeagueLogo;
        private System.Windows.Forms.Label lblLeagueName;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblLoginKey;
    }
}