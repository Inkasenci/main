namespace SZI
{
    partial class HelpForm
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
            this.tvFAQ = new System.Windows.Forms.TreeView();
            this.btClose = new System.Windows.Forms.Button();
            this.lHelp = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tvFAQ
            // 
            this.tvFAQ.Location = new System.Drawing.Point(12, 12);
            this.tvFAQ.Name = "tvFAQ";
            this.tvFAQ.Size = new System.Drawing.Size(217, 403);
            this.tvFAQ.TabIndex = 0;
            // 
            // btClose
            // 
            this.btClose.Location = new System.Drawing.Point(172, 445);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(277, 43);
            this.btClose.TabIndex = 1;
            this.btClose.Text = "Zamknij Okno";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // lHelp
            // 
            this.lHelp.AutoSize = true;
            this.lHelp.Location = new System.Drawing.Point(248, 12);
            this.lHelp.Name = "lHelp";
            this.lHelp.Size = new System.Drawing.Size(86, 13);
            this.lHelp.TabIndex = 2;
            this.lHelp.Text = "Witaj w Pomocy!";
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 500);
            this.Controls.Add(this.lHelp);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.tvFAQ);
            this.Name = "HelpForm";
            this.Text = "HelpForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvFAQ;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Label lHelp;
    }
}