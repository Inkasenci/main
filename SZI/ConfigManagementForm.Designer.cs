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
            this.btModify = new System.Windows.Forms.Button();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.btRefresh = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collectorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.XMLeditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stronaGłównaProgramuToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.mainPageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.progressStatusMain = new SZI.ProgressStatusStrip();
            this.statusLabelMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.progressStatusMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btDelete
            // 
            this.btDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btDelete.Enabled = false;
            this.btDelete.Location = new System.Drawing.Point(12, 569);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(158, 28);
            this.btDelete.TabIndex = 0;
            this.btDelete.Text = "Usuń";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btInsert
            // 
            this.btInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btInsert.Enabled = false;
            this.btInsert.Location = new System.Drawing.Point(12, 534);
            this.btInsert.Name = "btInsert";
            this.btInsert.Size = new System.Drawing.Size(157, 29);
            this.btInsert.TabIndex = 2;
            this.btInsert.Text = "Dodaj";
            this.btInsert.UseVisualStyleBackColor = true;
            this.btInsert.Click += new System.EventHandler(this.btInsert_Click);
            // 
            // btModify
            // 
            this.btModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btModify.Enabled = false;
            this.btModify.Location = new System.Drawing.Point(12, 603);
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
            this.btRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btRefresh.Location = new System.Drawing.Point(12, 638);
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(157, 29);
            this.btRefresh.TabIndex = 5;
            this.btRefresh.Text = "Wczytaj dane";
            this.btRefresh.UseVisualStyleBackColor = true;
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reportsToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.XMLeditorToolStripMenuItem,
            this.readingsToolStripMenuItem,
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(936, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collectorsToolStripMenuItem,
            this.customersToolStripMenuItem});
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.reportsToolStripMenuItem.Text = "Raporty";
            // 
            // collectorsToolStripMenuItem
            // 
            this.collectorsToolStripMenuItem.Name = "collectorsToolStripMenuItem";
            this.collectorsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.collectorsToolStripMenuItem.Text = "Inkasenci";
            this.collectorsToolStripMenuItem.Click += new System.EventHandler(this.inkasenciToolStripMenuItem_Click);
            // 
            // customersToolStripMenuItem
            // 
            this.customersToolStripMenuItem.Name = "customersToolStripMenuItem";
            this.customersToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.customersToolStripMenuItem.Text = "Klienci";
            this.customersToolStripMenuItem.Click += new System.EventHandler(this.klienciToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateDataToolStripMenuItem,
            this.clearDataToolStripMenuItem,
            this.backupToolStripMenuItem,
            this.restoreToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.settingsToolStripMenuItem.Text = "Ustawienia";
            // 
            // generateDataToolStripMenuItem
            // 
            this.generateDataToolStripMenuItem.Name = "generateDataToolStripMenuItem";
            this.generateDataToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.generateDataToolStripMenuItem.Text = "Generuj przykładowe dane";
            this.generateDataToolStripMenuItem.Click += new System.EventHandler(this.generateDataToolStripMenuItem_Click);
            // 
            // clearDataToolStripMenuItem
            // 
            this.clearDataToolStripMenuItem.Name = "clearDataToolStripMenuItem";
            this.clearDataToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.clearDataToolStripMenuItem.Text = "Wyczyść dane";
            this.clearDataToolStripMenuItem.Click += new System.EventHandler(this.clearDataToolStripMenuItem_Click);
            // 
            // backupToolStripMenuItem
            // 
            this.backupToolStripMenuItem.Name = "backupToolStripMenuItem";
            this.backupToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.backupToolStripMenuItem.Text = "Backup";
            this.backupToolStripMenuItem.Click += new System.EventHandler(this.backupToolStripMenuItem_Click);
            // 
            // restoreToolStripMenuItem
            // 
            this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            this.restoreToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.restoreToolStripMenuItem.Text = "Restore";
            this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
            // 
            // XMLeditorToolStripMenuItem
            // 
            this.XMLeditorToolStripMenuItem.Name = "XMLeditorToolStripMenuItem";
            this.XMLeditorToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.XMLeditorToolStripMenuItem.Text = "Edytor XML";
            this.XMLeditorToolStripMenuItem.Click += new System.EventHandler(this.XMLeditorToolStripMenuItem_Click);
            // 
            // readingsToolStripMenuItem
            // 
            this.readingsToolStripMenuItem.Name = "readingsToolStripMenuItem";
            this.readingsToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.readingsToolStripMenuItem.Text = "Odczyty";
            this.readingsToolStripMenuItem.Click += new System.EventHandler(this.readingsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.stronaGłównaProgramuToolStripMenuItem,
            this.mainPageToolStripMenuItem1});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(24, 20);
            this.toolStripMenuItem1.Text = "?";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.helpToolStripMenuItem.Text = "Pomoc";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // stronaGłównaProgramuToolStripMenuItem
            // 
            this.stronaGłównaProgramuToolStripMenuItem.Name = "stronaGłównaProgramuToolStripMenuItem";
            this.stronaGłównaProgramuToolStripMenuItem.Size = new System.Drawing.Size(203, 6);
            // 
            // mainPageToolStripMenuItem1
            // 
            this.mainPageToolStripMenuItem1.Name = "mainPageToolStripMenuItem1";
            this.mainPageToolStripMenuItem1.Size = new System.Drawing.Size(206, 22);
            this.mainPageToolStripMenuItem1.Text = "Strona główna programu";
            this.mainPageToolStripMenuItem1.Click += new System.EventHandler(this.mainPageToolStripMenuItem1_Click);
            // 
            // progressStatusMain
            // 
            this.progressStatusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabelMain});
            this.progressStatusMain.Location = new System.Drawing.Point(0, 679);
            this.progressStatusMain.Name = "progressStatusMain";
            this.progressStatusMain.ProgressColor = System.Drawing.Color.ForestGreen;
            this.progressStatusMain.ProgressShade = System.Drawing.Color.LightGreen;
            this.progressStatusMain.Size = new System.Drawing.Size(936, 22);
            this.progressStatusMain.TabIndex = 7;
            this.progressStatusMain.Text = "progressStatusStrip1";
            // 
            // statusLabelMain
            // 
            this.statusLabelMain.Name = "statusLabelMain";
            this.statusLabelMain.Size = new System.Drawing.Size(0, 17);
            // 
            // ConfigManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 701);
            this.Controls.Add(this.progressStatusMain);
            this.Controls.Add(this.btRefresh);
            this.Controls.Add(this.btModify);
            this.Controls.Add(this.btInsert);
            this.Controls.Add(this.btDelete);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ConfigManagementForm";
            this.Text = "System Zarządzania Inkasentami";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.progressStatusMain.ResumeLayout(false);
            this.progressStatusMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button btInsert;
        private System.Windows.Forms.Button btModify;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.Button btRefresh;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collectorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem XMLeditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator stronaGłównaProgramuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mainPageToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem backupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
        private ProgressStatusStrip progressStatusMain;
        private System.Windows.Forms.ToolStripStatusLabel statusLabelMain;



    }
}

