using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    /// <summary>
    /// Interfejs łączący poszczególne klasy obsługi bazy danych.
    /// </summary>
    public interface IDataBase
    {
        /// <summary>
        /// Metoda zwracająca ilość rekordów. 
        /// </summary>
        int recordCount { get; }

        /// <summary>
        /// Lista itemów - używana podczas odświeżania listView.
        /// </summary>
        List<string[]> itemList { get; set; }

        /// <summary>
        /// Funkcja odpowiedzialna za odświeżanie listy danych.
        /// </summary>
        void RefreshList();
    }
}
