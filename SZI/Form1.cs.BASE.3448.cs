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
        private int selectedTab = 0;
        private TabControl tabControl;
        private ListView.SelectedIndexCollection indexes; //indeksy zaznaczonych w danym momencie elementów listView w aktywnej zakładce
        private IDataBase[] dataBase;
        List<string> ids;
        ListView[] listView;

        private void MainTabControlInit()
        {
            // Deklaracja            
            TabPage[] tabPages;
            string[] tabNames;

            // Inicjalizacja
            tabNames = new string[4] { "Inkasenci", "Klienci", "Tereny", "Liczniki" };
            tabControl = new TabControl();
            tabPages = new TabPage[4];
            dataBase = new IDataBase[4] { new Collectors(), new Customers(), new Areas(), new Counters() };
            listView = new ListView[dataBase.Length];
            timerRefresh.Start();

            // Tworzenie tabControl
            tabControl.Padding = new Point(10, 10);
            tabControl.Location = new Point(10, 10);
            tabControl.Size = new Size(500, 500);

            // Dodawanie listView
            for (int i = 0; i < tabPages.Length; i++)
            {
                tabPages[i] = new TabPage();
                tabPages[i].Name = tabPages[i].Text = tabNames[i];
                listView[i] = ListViewConfig.ListViewInit(dataBase[i].columnList, dataBase[i].className, dataBase[i].itemList);
                listView[i].SelectedIndexChanged += lv_SelectedIndexChanged;
                tabPages[i].Controls.Add(listView[i]);
            }

            // Aktywacja
            this.Controls.Add(tabControl);
            tabControl.TabPages.AddRange(tabPages);
            tabControl.SelectedTab = tabPages[selectedTab];

            tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
        }

        void listView_DataChanged(object sender, EventArgs e)
        {
            selectedTab = tabControl.SelectedIndex;
        }

        void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTab = tabControl.SelectedIndex;
        }

        void lv_SelectedIndexChanged(object sender, EventArgs e)
        {
            ids = new List<string>();
            ListView activeListView = (ListView)sender;
            indexes = activeListView.SelectedIndices;

            tbTest.Text = "";

            ListView.SelectedListViewItemCollection selectedItems = activeListView.SelectedItems;
            foreach (ListViewItem item in selectedItems)
            //for (int i = 0; i < item.SubItems.Count; i++)
            {
                string s = item.SubItems[0].Text;
                tbTest.Text += item.SubItems[0].Text + " ";
                ids.Add(s);
            }
        }

        public Form1()
        {
            InitializeComponent();
            MainTabControlInit();
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            DBManipulator.DeleteFromDB(ids, selectedTab);
            //listViews[selectedTab].DeleteRowsByID(ids);
        }

        private void btInsert_Click(object sender, EventArgs e)
        {
            var insertForm = new InsertForm();
            insertForm.Show();
        }

        private void btModify_Click(object sender, EventArgs e)
        {
            var modifyForm = new ModifyForm(ids, selectedTab);
            modifyForm.ShowDialog();
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            int i = 0;
            foreach (var data in dataBase)
            {
                data.RefreshList();
                ListViewConfig.ListViewRefresh(listView[i++], data.itemList);
            }
        }
    }
}
