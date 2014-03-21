namespace SZI
{
    partial class Form1
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
            this.btDelete = new System.Windows.Forms.Button();
            this.tbTest = new System.Windows.Forms.TextBox();
            this.btInsert = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btModify = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btDelete
            // 
            this.btDelete.Location = new System.Drawing.Point(12, 570);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(158, 28);
            this.btDelete.TabIndex = 0;
            this.btDelete.Text = "Usuń";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // tbTest
            // 
            this.tbTest.Location = new System.Drawing.Point(190, 578);
            this.tbTest.Name = "tbTest";
            this.tbTest.Size = new System.Drawing.Size(323, 20);
            this.tbTest.TabIndex = 1;
            // 
            // btInsert
            // 
            this.btInsert.Location = new System.Drawing.Point(12, 535);
            this.btInsert.Name = "btInsert";
            this.btInsert.Size = new System.Drawing.Size(157, 29);
            this.btInsert.TabIndex = 2;
            this.btInsert.Text = "Dodaj";
            this.btInsert.UseVisualStyleBackColor = true;
            this.btInsert.Click += new System.EventHandler(this.btInsert_Click);
            // 
            // btModify
            // 
            this.btModify.Location = new System.Drawing.Point(12, 604);
            this.btModify.Name = "btModify";
            this.btModify.Size = new System.Drawing.Size(157, 29);
            this.btModify.TabIndex = 3;
            this.btModify.Text = "Modyfikuj";
            this.btModify.UseVisualStyleBackColor = true;
            this.btModify.Click += new System.EventHandler(this.btModify_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 645);
            this.Controls.Add(this.btModify);
            this.Controls.Add(this.btInsert);
            this.Controls.Add(this.tbTest);
            this.Controls.Add(this.btDelete);
            this.Name = "Form1";
            this.Text = "System Zarządzania Inkasentami";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.TextBox tbTest;
        private System.Windows.Forms.Button btInsert;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btModify;



    }
}

