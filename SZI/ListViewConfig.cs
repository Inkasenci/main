using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    /// <summary>
    /// Klasa odpowiedzialna za generowanie listView w aplikacji.
    /// </summary>
    static class ListViewConfig
    {
        /// <summary>
        /// Pozwala na określenie kolejności sortowania.
        /// </summary>
        static public bool orderBy = false;

        /// <summary>
        /// Funkcja pozwalająca na tworzenie elementu listView z tablicy string.
        /// </summary>
        /// <param name="item">Tablica danych tworzących element listView.</param>
        /// <returns>Element w postaci ListViewItem.</returns> 
        static private ListViewItem ConvertToItem(string[] item)
        {
            return new ListViewItem(item);
        }

        /// <summary>
        /// Główna funkcja inicjująca listView.
        /// </summary>
        /// <param name="columnList">Tablica określająca kolumny listView.</param>
        /// <param name="className">Nazwa listy.</param>
        /// <param name="itemList">Lista elementów dodawanych do listView, domyślne null pozwala na generowanie pustej kontrolki.</param>
        /// <param name="columnSort">Określa względem której kolumny sortujemy.</param>
        /// <param name="orderby">Określa sposób sortowania danej tabeli - ASC / DESC.</param>
        /// <returns>Zwracanie utworzonej ListView.</returns> 
        static public ListView ListViewInit(string[] columnList, string className, List<string[]> itemList = null, bool orderby = false, int columnSort = 0)
        {
            ListView lv = new ListView();
            lv.View = View.Details;
            lv.FullRowSelect = true;
            orderBy = orderby;
            lv.ListViewItemSorter = new ListViewSorter(columnSort, orderby);

            foreach (var column in columnList)
                lv.Columns.Add(column, -2);

            lv.Location = new System.Drawing.Point(10, 10);
            lv.Size = new System.Drawing.Size(600, 450);
            lv.Name = className;
            if ( itemList != null )
                foreach (var item in itemList)
                    lv.Items.Add(ConvertToItem(item));

            lv.ColumnClick += (object sender, ColumnClickEventArgs e)=>{
                lv.ListViewItemSorter = new ListViewSorter(e.Column, orderBy);
                orderBy = !orderBy;
            };

            return lv;
        }

        /// <summary>
        /// Główna funkcja inicjująca listView.
        /// </summary>
        /// <param name="columnList">Tablica określająca kolumny listView.</param>
        /// <param name="className">Nazwa listy.</param>
        static public void AddItem(ListView listView, string[] item)
        {
            listView.Items.Add(ConvertToItem(item));
        }

        /// <summary>
        /// Czyszczenie listView.
        /// </summary>
        /// <param name="listView">Element ListView do wyczyszczenie.</param>
        /// <returns>Zwracanie czystej ListView.</returns> 
        static public ListView ClearListView(ListView listView)
        {
            listView.BeginUpdate();
            listView.Items.Clear();
            listView.EndUpdate();
            return listView;
        }

        /// <summary>
        /// Funkcja pozwalająca odświeżyć listView.
        /// </summary>
        /// <param name="listView">Element ListView do odświeżenia.</param>
        /// <param name="itemList">Lista elementów dodawanych do listView, domyślne null pozwala na generowanie pustej kontrolki.</param>
        /// <returns>Zwracanie odświeżonej ListView.</returns> 
        static public ListView ListViewRefresh(ListView listView, List<string[]> itemList)
        {
            if (listView.Name == "CountersForm")
            {
                listView = ClearListView(listView);

                foreach (var item in itemList)
                    listView.Items.Add(ConvertToItem(item));

                AdjustColumnWidth(listView);
            }
            else
                ConfigManagementForm.listViewFilters[LangPL.ListViewNameToPageNumber[listView.Name]].FilterRecords();

            return listView;
        }

        /// <summary>
        /// Dopasowywuje szerokość kolumn listy do najszerszego elementu.
        /// </summary>
        /// <param name="listView">Lista, której kolumny są dopasowywane.</param>
        static public void AdjustColumnWidth(ListView listView)
        {
            foreach (ColumnHeader column in listView.Columns)
                column.Width = -2;
        }
    }
}
