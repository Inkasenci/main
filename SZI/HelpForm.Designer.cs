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
            this.rtbHelp = new System.Windows.Forms.RichTextBox();
            this.tvFAQ = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // rtbHelp
            // 
            this.rtbHelp.Location = new System.Drawing.Point(261, 13);
            this.rtbHelp.Name = "rtbHelp";
            this.rtbHelp.Size = new System.Drawing.Size(506, 487);
            this.rtbHelp.TabIndex = 7;
            this.rtbHelp.Text = "";
            // 
            // tvFAQ
            // 
            this.tvFAQ.Location = new System.Drawing.Point(22, 12);
            this.tvFAQ.Name = "tvFAQ";
            this.tvFAQ.Size = new System.Drawing.Size(217, 488);
            this.tvFAQ.TabIndex = 5;
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 512);
            this.Controls.Add(this.rtbHelp);
            this.Controls.Add(this.tvFAQ);
            this.Name = "HelpForm";
            this.Text = "HelpForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbHelp;
        private System.Windows.Forms.TreeView tvFAQ;
    }
}