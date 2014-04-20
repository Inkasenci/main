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
        private int selectedTab = 0;
        private TabControl tabControl;
        private ListView.SelectedIndexCollection indexes; //indeksy zaznaczonych w danym momencie elementów listView w aktywnej zakładce
        private IDataBase[] dataBase;
        private List<string> ids;
        private ListView[] listView;
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
            tabControl.SelectedTab = tabPages[selectedTab];

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
        /// Zwraca rekordy powiązane z zaznaczonym w ListView Inkasentem
        /// </summary>
        /// <returns>Rekordy powiązane z zaznaczonym w ListView Inkasentem</returns>
        private string ReturnRecordsAssociatedWithCollector()
        {
            string CollectorID = ids[0];
            string associatedRecords = String.Empty;

            using (var database = new CollectorsManagementSystemEntities())
            {
                var foreignResult = from f in database.Areas 
                                    where f.CollectorId == CollectorID 
                                    select f;

                associatedRecords += "Liczba powiązanych Terenów: " + foreignResult.Count().ToString() + "\n";
                foreach (Area a in foreignResult)
                     associatedRecords += a.AreaId + " " + a.Street + "\n";           

                var foreignResult2 = from f in database.Readings where f.CollectorId == CollectorID select f;

                associatedRecords += "\nLiczba powiązanych Odczytów: " + foreignResult2.Count().ToString() + "\n";
                foreach (Reading r in foreignResult2)
                    associatedRecords += r.ReadingId.ToString() + " " + r.CounterNo.ToString() + r.Date.ToShortDateString() + r.Value.ToString() + "\n";                  
             }

            return associatedRecords;
        }

        /// <summary>
        /// Zwraca rekordy powiązane z zaznaczonym w ListView Klientem
        /// </summary>
        /// <returns>Rekordy powiązane z zaznaczonym w ListView Klientem</returns>
        private string ReturnRecordsAssociatedWithCustomer()
        {
            string CustomerID = ids[0];
            string AssociatedRecords = String.Empty;

            using (var database = new CollectorsManagementSystemEntities())
            {

                var foreignResult = from f in database.Counters
                                    where f.CustomerId == CustomerID 
                                    select f;

                AssociatedRecords += "Liczba powiązanych Liczników: " + foreignResult.Count() + "\n";
                foreach (Counter c in foreignResult)
                    AssociatedRecords += c.CircuitNo.ToString() + " " +
                                        c.CounterNo.ToString() + " " + 
                                        (c.AddressId.HasValue ? Counters.FetchFullAddress(c.AddressId.Value) : string.Empty) + "\n";
                                         
            }

            return AssociatedRecords;
        }

        /// <summary>
        /// Zwraca rekordy powiązane z zaznaczonym w ListView Terenem
        /// </summary>
        /// <returns>Rekordy powiązane z zaznaczonym w ListView Terenem</returns>
        private string ReturnRecordsAssociatedWithArea()
        {
            Guid AreaID = new Guid(ids[0]);
            string AssociatedRecords = String.Empty;

            using (var database = new CollectorsManagementSystemEntities())
            {
                var foreignResult = from f in database.Addresses 
                                    where f.AreaId == AreaID 
                                    select f;

                AssociatedRecords += "Liczba powiązanych Adresów: " + foreignResult.Count() + "\n";
                foreach (Address a in foreignResult)
                    AssociatedRecords += a.AddressId.ToString() + " " +
                                        a.HouseNo.ToString() +
                                        (a.FlatNo != null ? "/" + a.FlatNo.ToString() : String.Empty) + "\n";
            

            }
            return AssociatedRecords;
        }

        /// <summary>
        /// Zwraca rekordy powiązane z zaznaczonym w ListView Licznikiem
        /// </summary>
        /// <returns>Rekordy powiązane z zaznaczonym w ListView Licznikiem</returns>
        private string ReturnRecordsAssociatedWithCounter()
        {
            int CounterID = Convert.ToInt32(ids[0]);
            string AssociatedRecords = String.Empty;

            using (var database = new CollectorsManagementSystemEntities())
            {
                var foreignResult = from f in database.Readings 
                                    where f.CounterNo == CounterID 
                                    select f;

                AssociatedRecords+="Liczba powiązanych odczytów: " + foreignResult.Count().ToString() + "\n";
                foreach (Reading r in foreignResult)
                    AssociatedRecords += r.ReadingId.ToString() + " " +
                                         Areas.FetchCollector(r.CollectorId) + " " +
                                         r.CounterNo.ToString() + " " +
                                         r.Date.ToShortDateString() + " " + 
                                         r.Value.ToString() + "\n";
                }            

            return AssociatedRecords;
        }

        /// <summary>
        /// Zwraca rekordy powiązane z zaznaczonym w ListView Adresem
        /// </summary>
        /// <returns>Rekordy powiązane z zaznaczonym w ListView Adresem</returns>
        private string ReturnRecordsAssociatedWithAddress()
        {
            Guid AddressID = new Guid(ids[0]);
            string AssociatedRecords = String.Empty;

            using (var database = new CollectorsManagementSystemEntities())
            {
                var foreignResult = from f in database.Counters
                                    where f.AddressId == AddressID 
                                    select f;

                AssociatedRecords += "Liczba powiązanych Liczników: " + foreignResult.Count().ToString() + "\n";
                foreach (Counter c in foreignResult)
                    AssociatedRecords += c.CounterNo.ToString() + " " +
                                       c.CircuitNo.ToString() + " " +
                                       Counters.FetchFullAddress(AddressID) + " " +
                                       Counters.FetchCustomer(c.CustomerId) + "\n";
            }

            return AssociatedRecords;
        }

        /// <summary>
        /// Wyświetla rekordy powiązane z zaznaczonym w ListView rekordem
        /// </summary>
        /// <param name="sender">Nieistotny parametr, niezbędny do przypisania metody do EventHandlera ToolStripItemu</param>
        /// <param name="e">Nieistotny parametr, niezbędny do przypisania metody do EventHandlera ToolStripItemu</param>
        private void ShowAssociatedRecords(object sender, EventArgs e)
        {
            string AssociatedRecords = String.Empty;

            if (listView[selectedTab].SelectedIndices.Count == 1)
            {
                switch (selectedTab)
                {
                    case 0:
                        AssociatedRecords = ReturnRecordsAssociatedWithCollector();
                        break;

                    case 1:
                        AssociatedRecords = ReturnRecordsAssociatedWithCustomer();
                        break;

                    case 2:
                        AssociatedRecords = ReturnRecordsAssociatedWithArea();
                        break;

                    case 3:
                        AssociatedRecords = ReturnRecordsAssociatedWithCounter();
                        break;
                        
                    case 4:
                        AssociatedRecords = ReturnRecordsAssociatedWithAddress();
                        break;

                    default:
                        break;
                }

                MessageBox.Show(AssociatedRecords, "Powiązane rekordy");
            }
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
        /// Kopiuje zaznaczone elementy listy do schowka
        /// </summary>
        /// <param name="sender">Nieistotny parametr, niezbędny do przypisania metody do EventHandlera ToolStripItemu</param>
        /// <param name="e">Nieistotny parametr, niezbędny do przypisania metody do EventHandlera ToolStripItemu</param>
        private void CopyItemstoClipboard(object sender, EventArgs e)
        {
            string clipboard = string.Empty;
            ListView lv = listView[selectedTab];

            if (lv.SelectedItems.Count > 0)
            {
                for (int i = 0; i < lv.SelectedItems.Count; i++)
                {
                    for (int j = 0; j < lv.SelectedItems[i].SubItems.Count; j++)
                    {
                        clipboard += lv.SelectedItems[i].SubItems[j].Text;
                        if (j != lv.SelectedItems[i].SubItems.Count - 1)
                            clipboard += '\t';
                    }
                    clipboard += "\n";
                }
                Clipboard.SetText(clipboard);
            }
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
            ListView lv = listView[selectedTab];
            lv.MultiSelect = true;
            foreach (ListViewItem item in lv.Items)
            {
                item.Selected = true;
            }

        }
        
        #region EventHandlery

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

        }

        // List wiew refresh ( every tick = 15 min [ 900000 ms ] )
        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            closeForm_Click(sender, e);
        }

        // Data refresh
        private void closeForm_Click(object sender, EventArgs e)
        {
            IForm form = (IForm)sender;
            
            if (form.Modified) //jeśli dokonano modyfikacji lub dodania rekordu, to odśwież listę
            {
                if (form.GetType() == typeof(InsertForm) || form.GetType() == typeof(ConfigManagementForm)) //jeśli wprowadzono rekord, lub usunięto, lub odświeżenie
                {
                    dataBase[selectedTab].RefreshList();
                    ListViewConfig.ListViewRefresh(listView[selectedTab], dataBase[selectedTab].itemList);
                }
                else //zmodyfikowano jakiś rekord
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

        /// <summary>
        /// Ustawia zmienną selectedTab na liczbę odpowiadającą wybranej zakładce
        /// </summary>
        /// <param name="sender">TabControl w ConfigManagementFormie</param>
        /// <param name="e">Parametry zdarzenia</param>
        void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTab = tabControl.SelectedIndex;
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
                CopyItemstoClipboard(null, null);
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

        void listView_DataChanged(object sender, EventArgs e)
        {
            selectedTab = tabControl.SelectedIndex;
        }

        void lv_SelectedIndexChanged(object sender, EventArgs e)
        {
            ids = new List<string>();
            ListView activeListView = (ListView)sender;
            indexes = activeListView.SelectedIndices;


            ListView.SelectedListViewItemCollection selectedItems = activeListView.SelectedItems;
            foreach (ListViewItem item in selectedItems)
            {
                ids.Add(item.SubItems[0].Text);
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

        #endregion

        #region Buttony

        /// <summary>
        /// Wywoływana po naciścnięciu przycisku "Usuń".
        /// Usuwa zaznaczone rekordy, wcześniej sprawdzając, czy nie ma do nich odniesienia w innych tabelach.
        /// </summary>
        /// <param name="sender">Przycisk "Usuń".</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        private void btDelete_Click(object sender, EventArgs e)
        {
            bool idExists;
            string tableName = string.Empty;
            DialogResult choiceFromMessageBox = DialogResult.Yes;
            switch (selectedTab)
            {
                case 0:
                    tableName = "Collector";
                    break;
                case 1:
                    tableName = "Customer";
                    break;
                case 2:
                    tableName = "Area";
                    break;
                case 4:
                    tableName = "Address";
                    break;
            }

            for (int i = 0; i < ids.Count; i++)
            {
                idExists = DBManipulator.IdExistsInOtherTable(tableName, ids.ElementAt(i));
                if (idExists)
                {
                    tableName = tableName.ToLower();
                    choiceFromMessageBox = MessageBox.Show(LangPL.IntegrityWarnings[tableName + "Removal"], "Ostrzeżenie", MessageBoxButtons.YesNo);
                    break;
                }
            }

            if (choiceFromMessageBox == DialogResult.Yes)
            {
                DBManipulator.DeleteFromDB(ids, selectedTab);
                closeForm_Click(this, e);
                SetButtonEnabledProperty(false, false);
            }
            listView[selectedTab].HideSelection = false;
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
            int selectedIndex = listView[selectedTab].SelectedIndices[0]; //index modyfikowanego itemu
            var modifyForm = new ModifyForm(ids, selectedTab);
            modifyForm.FormClosing += closeForm_Click;
            modifyForm.ShowDialog();
            listView[selectedTab].HideSelection = false;
            listView[selectedTab].Items[selectedIndex].Selected = true;
            SetButtonEnabledProperty(true, true);
        }
        
        // Refresh data button
        private void btRefresh_Click(object sender, EventArgs e)
        {
            closeForm_Click(this, e);
        }

        #endregion

        #endregion
    }        
}
