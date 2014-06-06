using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    /// <summary>
    /// Klasa obsługująca - wczytująca - tereny z bazy danych.
    /// </summary>
    public class Counters : IDataBase
    {
        /// <summary>
        /// Lista liczników.
        /// </summary>
        public List<Counter> counterList;

        /// <summary>
        /// Tablica zawierająca listę kolumn.
        /// </summary>
        public static string[] columnList = new string[] {
                "Numer licznika",
                "Numer układu",
                "Id adresu",
                "Id klienta"
            };

        /// <summary>
        /// Nazwa klasy.
        /// </summary>
        public static string className = "Counters";

        /// <summary>
        /// Lista itemów - używana podczas odświeżania listView.
        /// </summary>
        public List<string[]> itemList { get; set; }

        /// <summary>
        /// Konstruktor inicjujący pola związane z klasą.
        /// </summary>
        public Counters()
        {
            counterList = new List<Counter>();
            itemList = new List<string[]>();

            className = this.GetType().Name;

            RefreshList();
        }

        /// <summary>
        /// Pobieranie listy liczników z bazy danych i konwertowanie do formatu używanego w programie.
        /// </summary>
        private void GenerateCountersList()
        {
            List<string[]> Counters = null;
            using (var database = new CollectorsManagementSystemEntities())
            {
                counterList = (from counter in database.Counters
                               select counter).ToList();

                var result = (from counter in database.Counters
                              join address in database.Addresses on counter.AddressId equals address.AddressId into gj
                              join customer in database.Customers on counter.CustomerId equals customer.CustomerId into test
                              select new
                              {
                                  circuitno = counter.CircuitNo,
                                  counterno = counter.CounterNo,
                                  address =
                                  (
                                    from subAddress in database.Addresses
                                    where subAddress.AddressId == counter.AddressId
                                    join area in database.Areas on subAddress.AreaId equals area.AreaId
                                    select area.Street + " " + subAddress.HouseNo + (subAddress.FlatNo.HasValue ? "/" + subAddress.FlatNo.Value : "")
                                  ).ToList(),
                                  customer =
                                  (
                                     from subCustomer in database.Customers
                                     where subCustomer.CustomerId == counter.CustomerId
                                     select subCustomer.Name + " " + subCustomer.LastName
                                  ).ToList()
                              }).ToList();

                Counters = new List<string[]>(result.Count());
                for (int i = 0; i < result.Count(); i++)
                {
                    Counters.Add(new string[4]);
                    Counters[i][0] = result[i].counterno.ToString();
                    Counters[i][1] = result[i].circuitno.ToString();
                    Counters[i][2] = result[i].address.Count == 0 ? "" : result[i].address[0];
                    Counters[i][3] = result[i].customer.Count == 0 ? "" : result[i].customer[0];
                }
            }
            this.itemList = Counters;
        }

        /// <summary>
        /// Zwraca imię i nazwisko klienta na podstawie jego ID.
        /// </summary>
        /// <param name="CustomerID">Id klienta.</param>
        /// <returns>Imię i nazwisko klienta.</returns>
        public static string FetchCustomer(string CustomerID)
        {
            string FullName = "";

            using (var database = new CollectorsManagementSystemEntities())
            {
                var customer = from c in database.Customers
                               where c.CustomerId == CustomerID
                               select c;

                if (customer.Count() == 1)
                {
                    foreach (Customer c in customer)
                        FullName = c.Name + " " + c.LastName;
                }
            }

            return FullName;
        }

        /// <summary>
        /// Zwraca pełny adres (ulica, numer domu i mieszkania) na podstawie AddressID.
        /// </summary>
        /// <param name="AddressID">Id adresu.</param>
        /// <returns>Pełny adres.</returns>
        static public string FetchFullAddress(Guid AddressID) //zwraca pełny adres (ulica, numer domu i mieszkania) na podstawie AddressID
        {
            string FullAddressString = "";

            using (var database = new CollectorsManagementSystemEntities())
            {
                var FullAddress = from address in database.Addresses
                                  join area in database.Areas on address.AreaId equals area.AreaId
                                  where address.AddressId == AddressID
                                  select new { Address = address, Area = area };

                if (FullAddress.Count() == 1)
                {
                    foreach (var fa in FullAddress)
                    {
                        FullAddressString = fa.Area.Street + " " + fa.Address.HouseNo.ToString();
                        if (fa.Address.FlatNo != null)
                            FullAddressString += "/" + fa.Address.FlatNo.ToString();
                    }
                }
            }

            return FullAddressString;
        }

        /// <summary>
        /// Funkcja odpowiedzialna za odświeżanie listy danych.
        /// </summary>
        public void RefreshList()
        {
            GenerateCountersList();
        }

        /// <summary>
        /// Metoda zwracająca ilość rekordów.
        /// </summary>
        public int recordCount
        {
            get { return counterList.Count(); }
        }

        /// <summary>
        /// Metoda zwracająca dany rekord, zależnie od podanego indentyfikatora.
        /// </summary>
        /// <param name="id">Id rekordu.</param>
        /// <returns>Rekord w postaci zawartej w bazie danych.</returns>
        public Counter this[int id]
        {
            get
            {
                return counterList[id];
            }
        }
    }
}
