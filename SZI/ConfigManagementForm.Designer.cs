namespace SZI
{
    partial class ConfigManagementForm
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
            this.components = new System.ComponentModel.Container();
            this.btDelete = new System.Windows.Forms.Button();
            this.btInsert = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btModify = new System.Windows.Forms.Button();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.btRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btDelete
            // 
            this.btDelete.Enabled = false;
            this.btDelete.Location = new System.Drawing.Point(12, 556);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(158, 28);
            this.btDelete.TabIndex = 0;
            this.btDelete.Text = "Usuń";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btInsert
            // 
            this.btInsert.Location = new System.Drawing.Point(12, 521);
            this.btInsert.Name = "btInsert";
            this.btInsert.Size = new System.Drawing.Size(157, 29);
            this.btInsert.TabIndex = 2;
            this.btInsert.Text = "Dodaj";
            this.btInsert.UseVisualStyleBackColor = true;
            this.btInsert.Click += new System.EventHandler(this.btInsert_Click);
            // 
            // btModify
            // 
            this.btModify.Enabled = false;
            this.btModify.Location = new System.Drawing.Point(12, 590);
            this.btModify.Name = "btModify";
            this.btModify.Size = new System.Drawing.Size(157, 29);
            this.btModify.TabIndex = 3;
            this.btModify.Text = "Modyfikuj";
            this.btModify.UseVisualStyleBackColor = true;
            this.btModify.Click += new System.EventHandler(this.btModify_Click);
            // 
            // timerRefresh
            // 
            this.timerRefresh.Interval = 900000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // btRefresh
            // 
            this.btRefresh.Location = new System.Drawing.Point(12, 625);
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(157, 29);
            this.btRefresh.TabIndex = 5;
            this.btRefresh.Text = "Odśwież";
            this.btRefresh.UseVisualStyleBackColor = true;
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // ConfigManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 662);
            this.Controls.Add(this.btRefresh);
            this.Controls.Add(this.btModify);
            this.Controls.Add(this.btInsert);
            this.Controls.Add(this.btDelete);
            this.Name = "ConfigManagementForm";
            this.Text = "System Zarządzania Inkasentami";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button btInsert;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btModify;
        private System.Windows.Forms.Timer timerRefresh;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.Button btRefresh;



    }
}

