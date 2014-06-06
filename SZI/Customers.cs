using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SZI
{
    /// <summary>
    /// Klasa obsługująca - wczytująca - klientów z bazy danych.
    /// </summary>
    class Customers : IDataBase
    {
        /// <summary>
        /// Lista klientów.
        /// </summary>
        public static List<Customer> customerList;

        /// <summary>
        /// Tablica zawierająca listę kolumn.
        /// </summary>
        public static string[] columnList = new string[] {
                "Id klienta",
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
        public static string className = "Customers";

        /// <summary>
        /// Lista itemów - używana podczas odświeżania listView.
        /// </summary>
        public List<string[]> itemList { get; set; }

        /// <summary>
        /// Konstruktor inicjujący pola związane z klasą.
        /// </summary>
        public Customers()
        {
            customerList = new List<Customer>();
            itemList = new List<string[]>();

            columnList = new string[7] {
                "Id klienta",
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
        /// Pobieranie listy klientów z bazy danych i konwertowanie do formatu używanego w programie.
        /// </summary>
        private void GenerateItemList()
        {
            customerList.Clear();
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                foreach (var value in dataBase.Customers)
                    customerList.Add(value);
            }
        }

        /// <summary>
        /// Tworzenie tablicy stringów potrzebnej podczas wyświetlania.
        /// </summary>
        private void GenerateStringList()
        {
            itemList.Clear();
            foreach (var item in customerList)
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
            get { return customerList.Count(); }
        }

        /// <summary>
        /// Metoda zwracająca dany rekord, zależnie od podanego indentyfikatora.
        /// </summary>
        /// <param name="id">Id rekordu.</param>
        /// <returns>Rekord w postaci zawartej w bazie danych.</returns>
        public Customer this[int id]
        {
            get
            {
                return customerList[id];
            }
        }
    }
}
