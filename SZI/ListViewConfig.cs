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
            lv.MultiSelect = false;

            foreach (var column in columnList)
                lv.Columns.Add(column);

            lv.Location = new System.Drawing.Point(10, 10);
            lv.Size = new System.Drawing.Size(450, 450);
            lv.Name = className;

            foreach (var item in itemList)
                lv.Items.Add(ConvertToItem(item));
            return lv;
        }

        static public ListView ListViewRefresh(ListView listView, List<string[]> itemList)
        {
            listView.BeginUpdate(); //dodane tymczasowo
            listView.Items.Clear(); //dodane tymczasowo
            listView.EndUpdate(); //dodane tymczasowo
            //bool add = true; usuniete tymczasowo
            foreach (var item in itemList)
                listView.Items.Add(ConvertToItem(item)); //dodane tymczasowo
            /*{
                add = true;
                foreach (var itemOfList in listView.Items)
                    if (itemOfList.ToString() == ConvertToItem(item).ToString())
                        add = false;
                if(add)
                    listView.Items.Add(ConvertToItem(item));
            } usuniete tymczasowo */

            return listView;
        }
    }
}
