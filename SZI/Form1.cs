using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace SZI
{
    public partial class Form1 : Form
    {
        private void MainTabControlInit()
        {
            // Deklaracja
            TabControl tabControl;
            TabPage[] tabPages;
            string[] tabNames;
            IDataBase[] listsView;

            // Inicjalizacja
            tabNames = new string[4] { "Inkasenci", "Klienci", "Tereny", "Liczniki" };
            tabControl = new TabControl();
            tabPages = new TabPage[4];
            listsView = new IDataBase[4] { new Collectors(), new Customers(), new Areas(), new Counters() };

            // Tworzenie tabControl
            tabControl.Padding = new Point(10, 10);
            tabControl.Location = new Point(10, 10);
            tabControl.Size = new Size(500, 500);
            tabControl.SelectedTab = tabPages[1];

            // Tworzenie tabPages
            for (int i = 0; i < tabPages.Length; i++)
            {
                tabPages[i] = new TabPage();
                tabPages[i].Name = tabPages[i].Text = tabNames[i];
            }

            // Dodawanie listView
            for (int i = 0; i < tabPages.Length; i++)
            {
                tabPages[i] = new TabPage();
                tabPages[i].Name = tabPages[i].Text = tabNames[i];
                tabPages[i].Controls.Add(listsView[i].ListViewInitiate());
            }

            // Aktywacja
            this.Controls.Add(tabControl);
            tabControl.TabPages.AddRange(tabPages);
        }

        public Form1()
        {
            InitializeComponent();
            MainTabControlInit();
        }
    }
}