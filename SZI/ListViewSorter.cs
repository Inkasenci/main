using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    /// <summary>
    /// Klasa sortująca względem kolumny dane w listView.
    /// </summary>
    class ListViewSorter : IComparer
    {
        /// <summary>
        /// Numer kolumny, względem której sortujemy.
        /// </summary>
        private int sortColum;

        /// <summary>
        /// Porządek sortowania ASC / DESC.
        /// </summary>
        private bool orderBy;

        /// <summary>
        /// Domyślny parametr (konstruktor) w przypadku rozruchu listView.
        /// </summary>
        public ListViewSorter()
            : this(0, false)
        {
        }

        /// <summary>
        /// Konstruktor sortera ListView.
        /// </summary>
        /// <param name="column">Numer kolumny, względem której będziemy sortować dane.</param>
        /// <param name="order">Określa porządek sortowania.</param>
        public ListViewSorter(int column, bool order)
        {
            this.sortColum = column;
            this.orderBy = order;
        }

        /// <summary>
        /// Funkcja wskazująca (porównująca ciągi znaków) położenie danego obiektu.
        /// </summary>
        /// <param name="first">Określa pierwszy porównywany obiekt.</param>
        /// <param name="second">Określa drugi porównywany rekord.</param>
        /// <returns>Zwraca liczbę całkowitą, która wskazuje położenia rekordów w porządku sortowania.</returns>
        public int Compare(object first, object second)
        {
            string firstText = ((ListViewItem)first).SubItems[sortColum].Text;
            string secondText = ((ListViewItem)second).SubItems[sortColum].Text;
            return ((this.orderBy) ? 1 : -1 ) * String.Compare(firstText, secondText);
        }
    }
}
