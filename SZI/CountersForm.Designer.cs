namespace SZI
{
    partial class CountersForm
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
            this.btCheck = new System.Windows.Forms.Button();
            this.btImport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btCheck
            // 
            this.btCheck.Location = new System.Drawing.Point(12, 515);
            this.btCheck.Name = "btCheck";
            this.btCheck.Size = new System.Drawing.Size(157, 29);
            this.btCheck.TabIndex = 3;
            this.btCheck.Text = "Sprawdź odczyty";
            this.btCheck.UseVisualStyleBackColor = true;
            this.btCheck.Click += new System.EventHandler(this.btCheck_Click);
            // 
            // btImport
            // 
            this.btImport.Location = new System.Drawing.Point(12, 480);
            this.btImport.Name = "btImport";
            this.btImport.Size = new System.Drawing.Size(157, 29);
            this.btImport.TabIndex = 5;
            this.btImport.Text = "Import";
            this.btImport.UseVisualStyleBackColor = true;
            this.btImport.Click += new System.EventHandler(this.btImport_Click);
            // 
            // CountersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 556);
            this.Controls.Add(this.btImport);
            this.Controls.Add(this.btCheck);
            this.Name = "CountersForm";
            this.Text = "CountersForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btCheck;
        private System.Windows.Forms.Button btImport;
    }
}