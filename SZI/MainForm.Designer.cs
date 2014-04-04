namespace SZI
{
    partial class MainForm
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
            this.btDataManagement = new System.Windows.Forms.Button();
            this.btCounters = new System.Windows.Forms.Button();
            this.btConfig = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.btHelp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btDataManagement
            // 
            this.btDataManagement.Location = new System.Drawing.Point(205, 95);
            this.btDataManagement.Name = "btDataManagement";
            this.btDataManagement.Size = new System.Drawing.Size(268, 53);
            this.btDataManagement.TabIndex = 0;
            this.btDataManagement.Text = "Zarządzanie Danymi";
            this.btDataManagement.UseVisualStyleBackColor = true;
            this.btDataManagement.Click += new System.EventHandler(this.btDataManagement_Click);
            // 
            // btCounters
            // 
            this.btCounters.Location = new System.Drawing.Point(205, 154);
            this.btCounters.Name = "btCounters";
            this.btCounters.Size = new System.Drawing.Size(268, 53);
            this.btCounters.TabIndex = 1;
            this.btCounters.Text = "Odczyty";
            this.btCounters.UseVisualStyleBackColor = true;
            this.btCounters.Click += new System.EventHandler(this.btCounters_Click);
            // 
            // btConfig
            // 
            this.btConfig.Location = new System.Drawing.Point(205, 213);
            this.btConfig.Name = "btConfig";
            this.btConfig.Size = new System.Drawing.Size(268, 53);
            this.btConfig.TabIndex = 2;
            this.btConfig.Text = "Ustawienia";
            this.btConfig.UseVisualStyleBackColor = true;
            this.btConfig.Click += new System.EventHandler(this.btConfig_Click);
            // 
            // btClose
            // 
            this.btClose.Location = new System.Drawing.Point(205, 331);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(268, 53);
            this.btClose.TabIndex = 3;
            this.btClose.Text = "Zakończ Program";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // btHelp
            // 
            this.btHelp.Location = new System.Drawing.Point(205, 272);
            this.btHelp.Name = "btHelp";
            this.btHelp.Size = new System.Drawing.Size(268, 53);
            this.btHelp.TabIndex = 4;
            this.btHelp.Text = "Pomoc";
            this.btHelp.UseVisualStyleBackColor = true;
            this.btHelp.Click += new System.EventHandler(this.btHelp_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 489);
            this.Controls.Add(this.btHelp);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.btConfig);
            this.Controls.Add(this.btCounters);
            this.Controls.Add(this.btDataManagement);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btDataManagement;
        private System.Windows.Forms.Button btCounters;
        private System.Windows.Forms.Button btConfig;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Button btHelp;
    }
}