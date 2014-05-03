namespace SZI
{
    partial class ConfigurationForm
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
            this.btClearDataBase = new System.Windows.Forms.Button();
            this.btSampleData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btClearDataBase
            // 
            this.btClearDataBase.Location = new System.Drawing.Point(164, 116);
            this.btClearDataBase.Name = "btClearDataBase";
            this.btClearDataBase.Size = new System.Drawing.Size(157, 29);
            this.btClearDataBase.TabIndex = 4;
            this.btClearDataBase.Text = "Wyczyść bazę danych";
            this.btClearDataBase.UseVisualStyleBackColor = true;
            this.btClearDataBase.Click += new System.EventHandler(this.btClearDataBase_Click);
            // 
            // btSampleData
            // 
            this.btSampleData.Location = new System.Drawing.Point(164, 81);
            this.btSampleData.Name = "btSampleData";
            this.btSampleData.Size = new System.Drawing.Size(157, 29);
            this.btSampleData.TabIndex = 5;
            this.btSampleData.Text = "Generuj przykładowe dane";
            this.btSampleData.UseVisualStyleBackColor = true;
            this.btSampleData.Click += new System.EventHandler(this.btSampleData_Click);
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 462);
            this.Controls.Add(this.btSampleData);
            this.Controls.Add(this.btClearDataBase);
            this.Name = "ConfigurationForm";
            this.Text = "ConfigurationForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btClearDataBase;
        private System.Windows.Forms.Button btSampleData;
    }
}