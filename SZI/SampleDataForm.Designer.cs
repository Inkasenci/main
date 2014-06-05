namespace SZI
{
    partial class SampleDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleDataForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.collectorsCount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.customersCount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.areasCount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.countersCount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.addressesCount = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.readingsCount = new System.Windows.Forms.TextBox();
            this.generationButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(273, 169);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 194);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ilość inkasentów:";
            // 
            // collectorsCount
            // 
            this.collectorsCount.Location = new System.Drawing.Point(107, 191);
            this.collectorsCount.Name = "collectorsCount";
            this.collectorsCount.Size = new System.Drawing.Size(100, 20);
            this.collectorsCount.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 220);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Ilość klientów:";
            // 
            // customersCount
            // 
            this.customersCount.Location = new System.Drawing.Point(107, 217);
            this.customersCount.Name = "customersCount";
            this.customersCount.Size = new System.Drawing.Size(100, 20);
            this.customersCount.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 246);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Ilość terenów:";
            // 
            // areasCount
            // 
            this.areasCount.Location = new System.Drawing.Point(107, 243);
            this.areasCount.Name = "areasCount";
            this.areasCount.Size = new System.Drawing.Size(100, 20);
            this.areasCount.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 272);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Ilość liczników:";
            // 
            // countersCount
            // 
            this.countersCount.Location = new System.Drawing.Point(107, 269);
            this.countersCount.Name = "countersCount";
            this.countersCount.Size = new System.Drawing.Size(100, 20);
            this.countersCount.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 298);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Ilość adresów:";
            // 
            // addressesCount
            // 
            this.addressesCount.Location = new System.Drawing.Point(107, 295);
            this.addressesCount.Name = "addressesCount";
            this.addressesCount.Size = new System.Drawing.Size(100, 20);
            this.addressesCount.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 324);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Ilość odczytów:";
            // 
            // readingsCount
            // 
            this.readingsCount.Location = new System.Drawing.Point(107, 321);
            this.readingsCount.Name = "readingsCount";
            this.readingsCount.Size = new System.Drawing.Size(100, 20);
            this.readingsCount.TabIndex = 17;
            // 
            // generationButton
            // 
            this.generationButton.Location = new System.Drawing.Point(107, 354);
            this.generationButton.Name = "generationButton";
            this.generationButton.Size = new System.Drawing.Size(75, 23);
            this.generationButton.TabIndex = 18;
            this.generationButton.Text = "Generuj";
            this.generationButton.UseVisualStyleBackColor = true;
            this.generationButton.Click += new System.EventHandler(this.generationButton_Click);
            // 
            // SampleDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 389);
            this.Controls.Add(this.generationButton);
            this.Controls.Add(this.readingsCount);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.addressesCount);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.countersCount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.areasCount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.customersCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.collectorsCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SampleDataForm";
            this.Text = "Generowanie przykładowej bazy danych";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox collectorsCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox customersCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox areasCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox countersCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox addressesCount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox readingsCount;
        private System.Windows.Forms.Button generationButton;
    }
}