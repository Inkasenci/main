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
            listView.BeginUpdate(); //dodane tymczasowo - PZ
            listView.Items.Clear(); //dodane tymczasowo - PZ
            listView.EndUpdate(); //dodane tymczasowo - PZ
            //bool add = true; usuniete tymczasowo - PZ
            foreach (var item in itemList)
                listView.Items.Add(ConvertToItem(item)); //dodane tymczasowo - PZ
            /*{
                add = true;
                foreach (var itemOfList in listView.Items)
                    if (itemOfList.ToString() == ConvertToItem(item).ToString())
                        add = false;
                if(add)
                    listView.Items.Add(ConvertToItem(item));
            } usuniete tymczasowo - PZ */

            AdjustColumnWidth(listView);

            return listView;
        }

        static public void AdjustColumnWidth(ListView listView)
        {
            foreach (ColumnHeader column in listView.Columns)
            {
                column.Width = -1;
                int widthAdjustedToItem = column.Width;
                column.Width = -2;
                int widthAdjustedToHeader = column.Width;
                column.Width = (widthAdjustedToItem > widthAdjustedToHeader) ? -1 : -2;
            }
        }
    }
}
