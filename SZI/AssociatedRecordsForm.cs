using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    /// <summary>
    /// Forma służąca do wyświetlania rekordów powiązanych z rekordem wybranym w ConfigManagementForm. Tworzona po wybraniu opcji "Wyświetl powiązane rekordy" z menu kontekstowego.
    /// </summary>
    public partial class AssociatedRecordsForm : Form
    {
        /// <summary>
        /// ListView, która zawiera rekordy powiązane z wybranym rekordem w ConfigManagementForm.
        /// </summary>
        private ListView lvRecords;

        /// <summary>
        /// Lista kolumn w listView.
        /// </summary>
        string[] ColumnList;

        /// <summary>
        /// Id wybranego rekordu.
        /// </summary>
        private string selectedId = "0";

        /// <summary>
        /// Id rekordu - dla których wybieramy powiązane pola.
        /// </summary>
        private string choosenId = "0";

        /// <summary>
        /// Kolekcja itemów ContextMenuStrip dla przypadku, gdy zaznaczony jest w ListView co najmniej jeden rekord.
        /// </summary>
        private ToolStripItemCollection Items_ItemsSelected;
        /// <summary>
        /// Kolekcja itemów ContextMenuStrip dla przypadku, gdy nie jest zaznaczony w ListView żaden rekord.
        /// </summary>
        private ToolStripItemCollection Items_NoSelection;

        /// <summary>
        /// Konstruktor formy wyświetlającej listę rekordów powiązanych z wybranym rekordem w ConfigManagementForm.
        /// </summary>
        /// <param name="AssociatedRecords">Rekordy powiązane z wybranym rekordem w ConfigManagementForm.</param>
        /// <param name="Table">Tabela, z której pochodzi wybrany rekord.</param>
        public AssociatedRecordsForm(List<List<string>> AssociatedRecords, Tables Table, string choosenId)
        {
            InitializeComponent();

            ContextMenuStrip ContextMenu;

            this.choosenId = choosenId;
            this.SetDisplayRectLocation(500, 500);
            this.Size = new Size(500, 400);
            this.Text = "Powiązane rekordy";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            List<string[]> ItemList = new List<string[]>(AssociatedRecords.Count);

            for (int i = 0; i < AssociatedRecords.Count; i++)
                ItemList.Add(AssociatedRecords[i].ToArray());

            switch (Table)
            {
                case Tables.Collectors:
                    ColumnList = new string[2] 
                    {
                        "Id terenu",
                        "Ulica"
                    };
                    break;

                case Tables.Customers:
                    ColumnList = new string[3] 
                    {
                        "Numer licznika",
                        "Numer układu",
                        "Id adresu"
                    };
                    break;

                case Tables.Areas:
                    ColumnList = new string[3] 
                    {
                        "Id adresu",
                        "Numer domu",
                        "Numer mieszkania"
                    };
                    break;

                case Tables.Counters:
                    {
                        ColumnList = new string[3] 
                        {
                            "Id odczytu",
                            "Data odczytu",
                            "Wartość"
                        };
                        {
                            this.Text = "Podgląd licznika";
                            Button editButton = new Button();
                            editButton.Size = new Size(157, 29);
                            editButton.Text = "Edycja";
                            editButton.Name = "EditRecord";
                            editButton.Enabled = false;
                            editButton.Location = new Point(10, this.Size.Height - 75);
                            editButton.Click += Event_EditButtonOnClick;
                            this.Controls.Add(editButton);

                            Button removeButton = new Button();
                            removeButton.Size = new Size(157, 29);
                            removeButton.Text = "Usuń";
                            removeButton.Name = "RemoveRecord";
                            removeButton.Enabled = false;
                            removeButton.Location = new Point(177, this.Size.Height - 75);
                            removeButton.Click += Event_RemoveButtonOnClick;
                            this.Controls.Add(removeButton);
                        }
                    }
                    break;

                case Tables.Addresses:
                    ColumnList = new string[3] 
                    {
                        "Numer licznika",
                        "Numer układu",
                        "Id klienta"
                    };
                    break;

                default:
                    ColumnList = new string[0];
                    break;
            }

            ContextMenu = CreateContextMenu();
            lvRecords = ListViewConfig.ListViewInit(ColumnList, "Collectors", ItemList, false, (Table == Tables.Counters) ? 1 : 0);
            Items_ItemsSelected = CreateContextMenuItems_ItemsSelected(ContextMenu);
            Items_NoSelection = CreateContextMenuItems_NoSelection(ContextMenu);
            lvRecords.ContextMenuStrip = ContextMenu;
            if (Table == Tables.Counters)
                lvRecords.SelectedIndexChanged += lv_SelectedIndexChanged;
            lvRecords.Location = new Point(this.Location.X + 15, this.Location.Y + 10);
            lvRecords.Size = new Size(this.Size.Width - 50, this.Size.Height - 100);
            ListViewConfig.AdjustColumnWidth(lvRecords);
            this.Controls.Add(lvRecords);
        }

        /// <summary>
        /// Otwarcie okna do edycji rekordu.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        void Event_EditButtonOnClick(object sender, EventArgs e)
        {
            var readingModifyForm = new ReadingModifyForm(selectedId);
            readingModifyForm.ShowDialog();

            List<List<string>> AssociatedRecords = ConnectionRecordsQuery.ReturnRecordsAssociatedWithCounter(choosenId);

            List<string[]> ItemList = new List<string[]>(AssociatedRecords.Count);

            for (int i = 0; i < AssociatedRecords.Count; i++)
                ItemList.Add(AssociatedRecords[i].ToArray());
            ListViewConfig.ListViewRefresh(lvRecords, ItemList);
        }

        /// <summary>
        /// Usunięcie rekordu.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        void Event_RemoveButtonOnClick(object sender, EventArgs e)
        {
            System.Guid editId;
            if (System.Guid.TryParse(selectedId, out editId))
                DBManipulator.DeleteReadFromDB(editId);

            List<List<string>> AssociatedRecords = ConnectionRecordsQuery.ReturnRecordsAssociatedWithCounter(choosenId);

            List<string[]> ItemList = new List<string[]>(AssociatedRecords.Count);

            for (int i = 0; i < AssociatedRecords.Count; i++)
                ItemList.Add(AssociatedRecords[i].ToArray());
            ListViewConfig.ListViewRefresh(lvRecords, ItemList);
        }

        /// <summary>
        /// Zmiana wybranego indeksu i dostępności buttonów.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        void lv_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView activeListView = (ListView)sender;

            ListView.SelectedListViewItemCollection selectedItem = activeListView.SelectedItems;

            foreach (ListViewItem item in selectedItem)
                selectedId = item.SubItems[0].Text;

            switch (activeListView.SelectedItems.Count)
            {
                case 1:
                    this.Controls.Find("EditRecord", true)[0].Enabled = true;
                    this.Controls.Find("RemoveRecord", true)[0].Enabled = true;
                    break;
                default:
                    this.Controls.Find("EditRecord", true)[0].Enabled = false;
                    this.Controls.Find("RemoveRecord", true)[0].Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// Tworzy ContextMenuStrip, które jest później przypisywane do wszystkich ListView.
        /// </summary>
        /// <returns>Stworzone ContextMenuStrip.</returns>
        private ContextMenuStrip CreateContextMenu()
        {
            ContextMenuStrip ContextMenu = new ContextMenuStrip();
            ContextMenu.Opening += ContextMenu_Opening;
            return ContextMenu;
        }

        /// <summary>
        /// Metoda wywoływana podczas otwierania menu kontekstowego w ListView.
        /// </summary>
        /// <param name="sender">ContextMenuStrip, które się rozwija.</param>
        /// <param name="e">Parametry zdarzenia.</param>
        private void ContextMenu_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip cms = (ContextMenuStrip)sender;

            cms.Items.Clear();
            if (lvRecords.SelectedItems.Count >= 1)
            {
                cms.Items.AddRange(Items_ItemsSelected);
            }
            else
                cms.Items.AddRange(Items_NoSelection);

            e.Cancel = false; //nie mam pojęcia dlaczego, ale dzięki temu menu otworzy się po pierwszym kliknięciu

        }

        /// <summary>
        /// Tworzy kolekcję itemów dla ContextMenuStrip, gdy zaznaczony jest co najmniej jeden item w ListView.
        /// </summary>
        /// <param name="Owner">ContextMenuStrip, do którego kolekcja zostanie przypisana.</param>
        /// <returns>Kolekcja itemów.</returns>
        private ToolStripItemCollection CreateContextMenuItems_ItemsSelected(object Owner)
        {
            ToolStripItemCollection items = new ToolStripItemCollection(Owner as ContextMenuStrip, new ToolStripItem[]
            {
                new ToolStripMenuItem("Kopiuj", null, CopyItemstoClipboard, Keys.Control | Keys.C),
                new ToolStripSeparator(),
                new ToolStripMenuItem("Zaznacz wszystko", null, SelectAllItems, Keys.Control | Keys.A)
            });

            return items;
        }

        /// <summary>
        /// Tworzy kolekcję itemów dla ContextMenuStrip, gdy nie jest zaznaczony żaden item w ListView.
        /// </summary>
        /// <param name="Owner">ContextMenuStrip, do którego zostanie przypisana kolekcja.</param>
        /// <returns>Kolekcja itemów.</returns>
        private ToolStripItemCollection CreateContextMenuItems_NoSelection(object Owner)
        {
            ToolStripItemCollection items = new ToolStripItemCollection(Owner as ContextMenuStrip, new ToolStripItem[]
            {
                new ToolStripMenuItem("Zaznacz wszystko", null, SelectAllItems, Keys.Control | Keys.A)
            });

            return items;
        }

        /// <summary>
        /// Zaznacza wszystkie itemy w ListView.
        /// </summary>
        /// <param name="sender">Nieistotny parametr, niezbędny do przypisania metody do EventHandlera ToolStripItemu.</param>
        /// <param name="e">Nieistotny parametr, niezbędny do przypisania metody do EventHandlera ToolStripItemu.</param>
        private void SelectAllItems(object sender, EventArgs e)
        {
            lvRecords.MultiSelect = true;
            foreach (ListViewItem item in lvRecords.Items)
            {
                item.Selected = true;
            }
        }

        /// <summary>
        /// Metoda wywołująca właściwą metodę kopiującą itemy aktywnego ListView do schowka. Przekazuje do niej jako parametr aktywne ListView.
        /// </summary>
        /// <param name="sender">Element ContextMenuToolStrip, który został naciśnięty.</param>
        /// <param name="e">Parametry zdarzenia.</param>
        private void CopyItemstoClipboard(object sender, EventArgs e)
        {
            Auxiliary.CopyItemstoClipboard(lvRecords, e);
        }
    }
}
