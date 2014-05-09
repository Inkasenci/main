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
    public partial class ConfigManagementForm : Form, IForm
    {
        private Tables selectedTab = Tables.Collectors;
        private TabControl tabControl;
        public static IDataBase[] dataBase;
        public static ListView[] listView;
        private ToolStripItemCollection Items_SingleSelection;
        private ToolStripItemCollection Items_MultipleSelection;
        private ToolStripItemCollection Items_NoSelection;
        /// <summary>
        /// Pole potrzebne do poprawnego działania metody closeForm_Click. Wartość jest zawsze prawdziwa.
        /// </summary>
        private bool modified = true;

        /// <summary>
        /// Właściwość potrzebna do poprawnego działania metody closeForm_Click.
        /// </summary>
        public bool Modified
        {
            get
            {
                return this.modified;
            }
        }

        // Initialize DB form
        public ConfigManagementForm()
        {
            InitializeComponent();
            MainTabControlInit();
        }

        // Data tabControl init
        private void MainTabControlInit()
        {
            // Deklaracja
            TabPage[] tabPages;
            string[] tabNames;
            ContextMenuStrip contextMenu;


            // Inicjalizacja
            tabNames = new string[5] { "Inkasenci", "Klienci", "Tereny", "Liczniki", "Adresy" };
            tabControl = new TabControl();
            tabPages = new TabPage[5];
            dataBase = new IDataBase[5] { new Collectors(), new Customers(), new Areas(), new Counters(), new Addresses() };
            listView = new ListView[dataBase.Length];
            contextMenu = CreateContextMenu();
            Items_SingleSelection = CreateContextMenuItems_SingleSelection(contextMenu);
            Items_NoSelection = CreateContextMenuItems_NoSelection(contextMenu);
            Items_MultipleSelection = CreateContextMenuItems_MultipleSelection(contextMenu);
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
                listView[i].KeyDown += ListView_KeyDown;
                listView[i].ContextMenuStrip = contextMenu;
                tabPages[i].Controls.Add(listView[i]);
            }

            // Aktywacja
            this.Controls.Add(tabControl);
            tabControl.TabPages.AddRange(tabPages);
            tabControl.SelectedTab = tabPages[(int)selectedTab];

            tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
        }


        /// <summary>
        /// Tworzy ContextMenuStrip, które jest później przypisywane do wszystkich ListView
        /// </summary>
        /// <returns>Stworzone ContextMenuStrip</returns>
        private ContextMenuStrip CreateContextMenu()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Opening += contextMenu_Opening;
            return contextMenu;
        }

        /// <summary>
        /// Tworzy kolekcję itemów dla ContextMenuStrip gdy zaznaczony jest w ListView jeden item
        /// </summary>
        /// <param name="Owner">ContextMenuStrip do którego kolekcja zostanie przypisana</param>
        /// <returns>Kolekcja itemów</returns>
        private ToolStripItemCollection CreateContextMenuItems_SingleSelection(object Owner)
        {
            ToolStripItemCollection items = new ToolStripItemCollection(Owner as ContextMenuStrip, new ToolStripItem[]
            {
                new ToolStripMenuItem("Kopiuj", null, CopyItemstoClipboard, Keys.Control | Keys.C),
                new ToolStripMenuItem("Usuń", null, btDelete_Click, Keys.Delete),
                new ToolStripSeparator(),
                new ToolStripMenuItem("Zaznacz wszystko", null, SelectAllItems, Keys.Control | Keys.A),
                new ToolStripSeparator(),
                new ToolStripMenuItem("Wyświetl powiązane rekordy", null, ShowAssociatedRecords, null)
            });

            return items;
        }

        /// <summary>
        /// Tworzy kolekcję itemów dla ContextMenuStrip gdy zaznaczony jest w ListView więcej niż jeden item
        /// </summary>
        /// <param name="Owner">ContextMenuStrip do którego kolekcja zostanie przypisana</param>
        /// <returns>Kolekcja itemów</returns>
        private ToolStripItemCollection CreateContextMenuItems_MultipleSelection(object Owner)
        {
            ToolStripItemCollection items = new ToolStripItemCollection(Owner as ContextMenuStrip, new ToolStripItem[]
            {
                new ToolStripMenuItem("Kopiuj", null, CopyItemstoClipboard, Keys.Control | Keys.C),
                new ToolStripMenuItem("Usuń", null, btDelete_Click, Keys.Delete),
                new ToolStripSeparator(),
                new ToolStripMenuItem("Zaznacz wszystko", null, SelectAllItems, Keys.Control | Keys.A)
            });

            return items;
        }

        /// <summary>
        /// Wyświetla rekordy powiązane z zaznaczonym w ListView rekordem
        /// </summary>
        /// <param name="sender">Nieistotny parametr, niezbędny do przypisania metody do EventHandlera ToolStripItemu</param>
        /// <param name="e">Nieistotny parametr, niezbędny do przypisania metody do EventHandlera ToolStripItemu</param>
        private void ShowAssociatedRecords(object sender, EventArgs e)
        {
            List<List<string>> AssociatedRecords;
            List<string> ids = Auxiliary.CreateIdList(listView[(int)selectedTab]);

            switch ((Tables)selectedTab)
            {
                case Tables.Collectors:
                    AssociatedRecords = ConnectionRecordsQuery.ReturnRecordsAssociatedWithCollector(ids[0]);
                    break;

                case Tables.Customers:
                    AssociatedRecords = ConnectionRecordsQuery.ReturnRecordsAssociatedWithCustomer(ids[0]);
                    break;

                case Tables.Areas:
                    AssociatedRecords = ConnectionRecordsQuery.ReturnRecordsAssociatedWithArea(ids[0]);
                    break;

                case Tables.Counters:
                    AssociatedRecords = ConnectionRecordsQuery.ReturnRecordsAssociatedWithCounter(ids[0]);
                    break;

                case Tables.Addresses:
                    AssociatedRecords = ConnectionRecordsQuery.ReturnRecordsAssociatedWithAddress(ids[0]);
                    break;

                default:
                    AssociatedRecords = new List<List<string>>();
                    break;
            }
            AssociatedRecordsForm asr = new AssociatedRecordsForm(AssociatedRecords, (Tables)selectedTab, ids[0]);
            asr.ShowDialog();
        }

        /// <summary>
        /// Tworzy kolekcję itemów dla ContextMenuStrip gdy nie jest zaznaczony w ListView żaden item
        /// </summary>
        /// <param name="Owner">ContextMenuStrip do którego kolekcja zostanie przypisana</param>
        /// <returns>Kolekcja itemów</returns>
        private ToolStripItemCollection CreateContextMenuItems_NoSelection(object Owner)
        {
            ToolStripItemCollection items = new ToolStripItemCollection(Owner as ContextMenuStrip, new ToolStripItem[]
            {
                new ToolStripMenuItem("Zaznacz wszystko", null, SelectAllItems, Keys.Control | Keys.A)
            });

            return items;
        }


        /// <summary>
        /// Ustawia właściwość "Enabled" dla przycisków "Usuń" i "Modyfikuj".
        /// </summary>
        /// <param name="btDeleteEnabledProperty">Pożądany stan właściwości "Enabled" dla przycisku "Usuń".</param>
        /// <param name="btDeleteModifyProperty">Pożądany stan właściwości "Enabled" dla przycisku "Modyfikuj".</param>
        private void SetButtonEnabledProperty(bool btDeleteEnabledProperty, bool btModifyEnabledProperty)
        {
            btDelete.Enabled = btDeleteEnabledProperty;
            btModify.Enabled = btModifyEnabledProperty;
        }

        /// <summary>
        /// Zaznacza wszystkie itemy w aktywnej ListView
        /// </summary>
        /// <param name="sender">Nieistotny parametr, niezbędny do przypisania metody do EventHandlera ToolStripItemu</param>
        /// <param name="e">Nieistotny parametr, niezbędny do przypisania metody do EventHandlera ToolStripItemu</param>
        private void SelectAllItems(object sender, EventArgs e)
        {
            ListView lv = listView[(int)selectedTab];
            foreach (ListViewItem item in lv.Items)
            {
                item.Selected = true;
            }

        }

        #region EventHandlery

        /// <summary>
        /// Metoda wywołująca właściwą metodę kopiującą itemy aktywnego ListView do schowka. Przekazuje do niej jako parametr aktywne ListView.
        /// </summary>
        /// <param name="sender">Element ContextMenuToolStrip który został naciśnięty.</param>
        /// <param name="e">Parametry zdarzenia.</param>
        private void CopyItemstoClipboard(object sender, EventArgs e)
        {
            Auxiliary.CopyItemstoClipboard(listView[(int)selectedTab], e);
        }

        /// <summary>
        /// Metoda wywoływana przy otwieraniu ContextToolStripMenu. Przypisuje odpowiednią kolekcję itemów w zależności od liczby zaznaczonych itemów.
        /// </summary>
        /// <param name="sender">ContextToolStripMenu do którego zostanie przypisana kolekcja itemów</param>
        /// <param name="e">Parametry zdarzenia</param>
        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip cms = (ContextMenuStrip)sender;
            ListView SourceListView = (ListView)cms.SourceControl;

            cms.Items.Clear();
            if (SourceListView.SelectedItems.Count == 1)
            {
                cms.Items.AddRange(Items_SingleSelection);
            }
            else if (SourceListView.SelectedItems.Count > 1)
                cms.Items.AddRange(Items_MultipleSelection);
            else
            {
                cms.Items.AddRange(Items_NoSelection);
            }
            e.Cancel = false; //nie mam pojęcia dlaczego, ale dzięki temu menu otworzy się po pierwszym kliknięciu

        }

        // List wiew refresh ( every tick = 15 min [ 900000 ms ] )
        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            ListViewDataManipulation.RefreshListView(this);
        }


        // Data refresh
        private void closeForm_Click(object sender, EventArgs e)
        {
            ListViewDataManipulation.RefreshListView(sender);
        }

        /// <summary>
        /// Ustawia zmienną selectedTab na liczbę odpowiadającą wybranej zakładce
        /// </summary>
        /// <param name="sender">TabControl w ConfigManagementFormie</param>
        /// <param name="e">Parametry zdarzenia</param>
        void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTab = (Tables)tabControl.SelectedIndex;
            listView[(int)selectedTab].HideSelection = false;
            SetButtonEnabledProperty(false, false);
        }

        #region ListView

        /// <summary>
        /// Metoda zajmująca się skrótami klawiszowymi dla ListView
        /// </summary>
        /// <param name="sender">ListView które wywołuje zdarzenie</param>
        /// <param name="e">Parametry zdarzenia</param>
        void ListView_KeyDown(object sender, KeyEventArgs e)
        {
            ListView lv = (ListView)sender;

            if (e.KeyCode == Keys.A && e.Control) //Ctrl + a - zaznaczanie wszystkich itemów listy
            {
                SelectAllItems(null, null);
            }
            else if (e.KeyCode == Keys.C && e.Control)// Ctrl + c - skopiowanie zaznaczonych itemów do schowka
            {
                Auxiliary.CopyItemstoClipboard(listView[(int)selectedTab], null);
            }
            else if (e.KeyCode == Keys.Delete) //Delete - równoznaczne z kliknięciem przycisku
            {
                if (lv.SelectedIndices.Count > 0)
                    btDelete_Click(btDelete, new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, 0, 0, 0));
            }
            else if ((e.KeyCode == Keys.F5) || (e.KeyCode == Keys.R && e.Control)) //wciśnięcie F5 lub Ctrl + r - odświeżenie ListView
            {
                btRefresh_Click(btRefresh, new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, 0, 0, 0));
            }
        }

        void lv_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (listView[(int)selectedTab].SelectedItems.Count)
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

        #endregion

        #region Buttony

        /// <summary>
        /// Wywoływana po naciścnięciu przycisku "Usuń".
        /// Usuwa zaznaczone rekordy, wcześniej sprawdzając, czy nie ma do nich odniesień w innych tabelach.
        /// </summary>
        /// <param name="sender">Przycisk "Usuń".</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        private void btDelete_Click(object sender, EventArgs e)
        {
            if (ListViewDataManipulation.DeleteItems(listView[(int)selectedTab], selectedTab))
            {
                ListViewDataManipulation.RefreshListView(this, selectedTab);
                SetButtonEnabledProperty(false, false);
            }
            listView[(int)selectedTab].HideSelection = false;
        }

        /// <summary>
        /// Wywoływana po naciśnięciu przycisku "Dodaj". Otwiera formularz umożliwiający dodawanie rekordów.
        /// </summary>
        /// <param name="sender">Przycisk "Dodaj".</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        private void btInsert_Click(object sender, EventArgs e)
        {
            var insertForm = new InsertForm(selectedTab);
            insertForm.FormClosing += closeForm_Click;
            insertForm.ShowDialog();
            SetButtonEnabledProperty(false, false);
        }

        /// <summary>
        /// Wywoływana po naciśnięciu przycisku "Modyfikuj". Otwiera formularz umożliwiający modyfikację zaznaczonego rekordu.
        /// </summary>
        /// <param name="sender">Przycisk "Modyfikuj".</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        private void btModify_Click(object sender, EventArgs e)
        {
            ListViewDataManipulation.ModifyRecord(listView[(int)selectedTab], selectedTab);
            SetButtonEnabledProperty(true, true);
        }

        // Refresh data button
        private void btRefresh_Click(object sender, EventArgs e)
        {
            ListViewDataManipulation.RefreshListView(this);
        }

        #endregion

        #endregion
    }
}