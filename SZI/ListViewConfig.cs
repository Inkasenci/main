using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    static class ListViewConfig
    {
        static private ListViewItem ConvertToItem(string[] item)
        {
            return new ListViewItem(item);
        }

        static public ListView ListViewInit(string[] columnList, string className, List<string[]> itemList)
        {
            ListView lv = new ListView();
            lv.View = View.Details;
            lv.FullRowSelect = true;

            foreach (var column in columnList)
                lv.Columns.Add(column, -2);

            lv.Location = new System.Drawing.Point(10, 10);
            lv.Size = new System.Drawing.Size(600, 450);
            lv.Name = className;

            foreach (var item in itemList)
                lv.Items.Add(ConvertToItem(item));

            return lv;
        }

        static public ListView ListViewRefresh(ListView listView, List<string[]> itemList)
        {
            listView.BeginUpdate();
            listView.Items.Clear();
            listView.EndUpdate();

            foreach (var item in itemList)
                listView.Items.Add(ConvertToItem(item));

            AdjustColumnWidth(listView);

            return listView;
        }

        /// <summary>
        /// Dopasowywuje szerokość kolumn listy do najszerszego elementu.
        /// </summary>
        /// <param name="listView">Lista, której kolumny są dopasowywane.</param>
        static private void AdjustColumnWidth(ListView listView)
        {
            foreach (ColumnHeader column in listView.Columns)
                column.Width = -2;
        }
    }
}
