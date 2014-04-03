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

        // Data tabControl init
        private void MainTabControlInit()
        {
            // Deklaracja            
            TabPage[] tabPages;
            string[] tabNames;

            // Inicjalizacja
            tabNames = new string[5] { "Inkasenci", "Klienci", "Tereny", "Liczniki", "Adresy" };
            tabControl = new TabControl();
            tabPages = new TabPage[5];
            dataBase = new IDataBase[5] { new Collectors(), new Customers(), new Areas(), new Counters(), new Addresses() };
            listView = new ListView[dataBase.Length];
            timerRefresh.Start();

            // Tworzenie tabControl
            tabControl.Padding = new Point(10, 10);
            tabControl.Location = new Point(10, 10);
            tabControl.Size = new Size(650, 500);

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

        private void SetButtonEnabledProperty(bool btDeleteEnabledProperty, bool btModifyEnabledProperty)
        {
            btDelete.Enabled = btDeleteEnabledProperty;
            btModify.Enabled = btModifyEnabledProperty;
        }

        void listView_DataChanged(object sender, EventArgs e)
        {
            selectedTab = tabControl.SelectedIndex;
        }

        void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTab = tabControl.SelectedIndex;
            SetButtonEnabledProperty(false, false);
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

            switch (listView[selectedTab].SelectedItems.Count)
            {
                case 0:
                    SetButtonEnabledProperty(false, false);
                    break;
                case 1:
                    SetButtonEnabledProperty(true, true);
                    break;
                default:
                    SetButtonEnabledProperty(true, false);
                    break;
            }
        }

        // Initialize DB form
        public Form1()
        {
            InitializeComponent();
            MainTabControlInit();
        }

        // Delete event click
        private void btDelete_Click(object sender, EventArgs e)
        {
            DBManipulator.DeleteFromDB(ids, selectedTab);
            closeForm_Click(sender, e);
            SetButtonEnabledProperty(false, false);
        }

        // Insert event click
        private void btInsert_Click(object sender, EventArgs e)
        {
            var insertForm = new InsertForm(selectedTab);
            insertForm.FormClosing += closeForm_Click;
            insertForm.ShowDialog();
            SetButtonEnabledProperty(false, false);
        }

        // Modify event click
        private void btModify_Click(object sender, EventArgs e)
        {
            var modifyForm = new ModifyForm(ids, selectedTab);
            modifyForm.FormClosing += closeForm_Click;
            modifyForm.ShowDialog();
            SetButtonEnabledProperty(false, false);
        }

        // List wiew refresh ( every tick = 15 min [ 900000 ms ] )
        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            closeForm_Click(sender,e);
        }

        // Data refresh
        private void closeForm_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (var data in dataBase)
            {
                data.RefreshList();
                ListViewConfig.ListViewRefresh(listView[i++], data.itemList);
            }
        }
        
        // Refresh data button
        private void btRefresh_Click(object sender, EventArgs e)
        {
            closeForm_Click(sender, e);
        }
    }
}
