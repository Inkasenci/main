using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    /// <summary>
    /// Klasa obsługująca - wczytująca - inkasenów z bazy danych.
    /// </summary>
    class Collectors : IDataBase
    {
        /// <summary>
        /// Lista inkasentów.
        /// </summary>
        static public List<Collector> collectorList;

        /// <summary>
        /// Tablica zawierająca listę kolumn.
        /// </summary>
        public static string[] columnList = new string[] {
                "Id inkasenta", 
                "Imię",
                "Nazwisko",
                "Kod pocztowy",
                "Miasto",
                "Adres", 
                "Telefon kontaktowy"
            };

        /// <summary>
        /// Nazwa klasy.
        /// </summary>
        public static string className = "Collectors";

        /// <summary>
        /// Lista itemów - używana podczas odświeżania listView.
        /// </summary>
        public List<string[]> itemList { get; set; }

        /// <summary>
        /// Konstruktor inicjujący pola związane z klasą.
        /// </summary>
        public Collectors()
        {
            collectorList = new List<Collector>();
            itemList = new List<string[]>();

            columnList = new string[7] { 
                "Id inkasenta", 
                "Imię",
                "Nazwisko",
                "Kod pocztowy",
                "Miasto",
                "Adres", 
                "Telefon kontaktowy"
            };

            className = this.GetType().Name;

            RefreshList();
        }

        /// <summary>
        /// Pobieranie listy inkasentów z bazy danych i konwertowanie do formatu używanego w programie.
        /// </summary>
        private void GenerateItemList()
        {
            collectorList.Clear();
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                collectorList = (from collector in dataBase.Collectors
                                 select collector).ToList();
            }
        }

        /// <summary>
        /// Tworzenie tablicy stringów potrzebnej podczas wyświetlania.
        /// </summary>
        private void GenerateStringList()
        {
            itemList.Clear();
            foreach (var item in collectorList)
                itemList.Add(item.GetElements);
        }

        /// <summary>
        /// Funkcja odpowiedzialna za odświeżanie listy danych.
        /// </summary>
        public void RefreshList()
        {
            GenerateItemList();
            GenerateStringList();
        }

        /// <summary>
        /// Metoda zwracająca ilość rekordów.
        /// </summary>
        public int recordCount
        {
            get { return collectorList.Count(); }
        }

        /// <summary>
        /// Metoda zwracająca dany rekord, zależnie od podanego indentyfikatora.
        /// </summary>
        /// <param name="id">Id rekordu.</param>
        /// <returns>Rekord w postaci zawartej w bazie danych.</returns>
        public Collector this[int id]
        {
            get
            {
                return collectorList[id];
            }
        }
    }
}
