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
    /// Forma służaca do wyświetlania rekordów powiązanych z rekordem wybranym w ConfigManagementForm. Tworzona po wybraniu opcji "Wyświetl powiązane rekordy" z menu kontekstowego.
    /// </summary>
    public partial class AssociatedRecordsForm : Form
    {
        /// <summary>
        /// ListView, która zawiera rekordy powiązane z wybranym rekordem w ConfigManagementForm.
        /// </summary>
        private ListView lvRecords;

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
        /// <param name="AssociatedRecords">Rekordy powiązane z wybranym w ConfigManagementForm rekordzie.</param>
        /// <param name="Table">Tabela z której pochodzi wybrany rekord</param>
        public AssociatedRecordsForm(List<List<string>> AssociatedRecords, Tables Table)
        {
            InitializeComponent();

            ContextMenuStrip ContextMenu;

            this.SetDisplayRectLocation(500, 500);
            this.Size = new Size(500, 350);
            this.Text = "Powiązane rekordy";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            string [] ColumnList;
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
                    ColumnList = new string[3] 
                    {
                        "Id odczytu",
                        "Data odczytu",
                        "Wartość"
                    };
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
            lvRecords = ListViewConfig.ListViewInit(ColumnList, "Collectors", ItemList);
            Items_ItemsSelected = CreateContextMenuItems_ItemsSelected(ContextMenu);
            Items_NoSelection = CreateContextMenuItems_NoSelection(ContextMenu);
            lvRecords.ContextMenuStrip = ContextMenu;
            
            lvRecords.Location = new Point(this.Location.X + 15, this.Location.Y + 10);
            lvRecords.Size = new Size(this.Size.Width - 50, this.Size.Height - 50);
            ListViewConfig.AdjustColumnWidth(lvRecords);
            this.Controls.Add(lvRecords);
        }

        /// <summary>
        /// Tworzy ContextMenuStrip, które jest później przypisywane do wszystkich ListView
        /// </summary>
        /// <returns>Stworzone ContextMenuStrip</returns>
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
        /// Tworzy kolekcję itemów dla ContextMenuStrip gdy zaznaczony jest w ListView co najmniej jeden item.
        /// </summary>
        /// <param name="Owner">ContextMenuStrip do którego kolekcja zostanie przypisana.</param>
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
        /// Zaznacza wszystkie itemy w ListView
        /// </summary>
        /// <param name="sender">Nieistotny parametr, niezbędny do przypisania metody do EventHandlera ToolStripItemu</param>
        /// <param name="e">Nieistotny parametr, niezbędny do przypisania metody do EventHandlera ToolStripItemu</param>
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
        /// <param name="sender">Element ContextMenuToolStrip który został naciśnięty.</param>
        /// <param name="e">Parametry zdarzenia.</param>
        private void CopyItemstoClipboard(object sender, EventArgs e)
        {
            Auxiliary.CopyItemstoClipboard(lvRecords, e);
        }
    }
}
