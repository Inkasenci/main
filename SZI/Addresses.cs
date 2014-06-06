using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    /// <summary>
    /// Klasa obsługująca - wczytująca - adresy z bazy danych.
    /// </summary>
    class Addresses : IDataBase
    {
        /// <summary>
        /// Lista adresów.
        /// </summary>
        static public List<Address> addressesList;

        /// <summary>
        /// Tablica zawierająca listę kolumn.
        /// </summary>
        public static string[] columnList = new string[] {
                "Id adresu",
                "Numer domu",
                "Numer mieszkania",
                "Id terenu"
            };

        /// <summary>
        /// Nazwa klasy.
        /// </summary>
        public static string className = "Addresses";

        /// <summary>
        /// Lista itemów - używana podczas odświeżania listView.
        /// </summary>
        public List<string[]> itemList { get; set; }

        /// <summary>
        /// Konstruktor inicjujący pola związane z klasą.
        /// </summary>
        public Addresses()
        {
            addressesList = new List<Address>();
            itemList = new List<string[]>();

            className = this.GetType().Name;

            RefreshList();
        }

        /// <summary>
        /// Generuje listę adresów.
        /// </summary>
        private void GenerateAddressesList()
        {
            List<string[]> Addresses = null;

            using (var database=new CollectorsManagementSystemEntities())
            {
                addressesList = (from address in database.Addresses
                                 select address).ToList();

                var result = (from address in database.Addresses
                             join area in database.Areas
                             on address.AreaId equals area.AreaId into gj
                             select new
                             {
                                 addressid = address.AddressId,
                                 house = address.HouseNo,
                                 flat = address.FlatNo,
                                 area =
                                 (
                                      from subArea in database.Areas
                                      where subArea.AreaId == address.AreaId
                                      join collector in database.Collectors
                                      on subArea.CollectorId equals collector.CollectorId into wp
                                      select new
                                      {
                                          areaid = subArea.Street,
                                          collector =
                                          (
                                              from subCollector in database.Collectors
                                              where subCollector.CollectorId == subArea.CollectorId
                                              select subCollector.Name + " " + subCollector.LastName
                                          ).ToList()
                                      }
                                 ).ToList()
                             }).ToList();

                Addresses = new List<string[]>();
                for (int i = 0; i < result.Count; i++)
                {
                    Addresses.Add(new string[4]);
                    Addresses[i][0] = result[i].addressid.ToString();
                    Addresses[i][1] = result[i].house.ToString();
                    Addresses[i][2] = result[i].flat.HasValue ? result[i].flat.ToString() : "";
                    Addresses[i][3] = result[i].area[0].collector.Count == 0 ? result[i].area[0].areaid.ToString() : result[i].area[0].areaid.ToString() + ": " + result[i].area[0].collector[0];
                }
            }
            this.itemList = Addresses;
        }        

        /// <summary>
        /// Funkcja odpowiedzialna za odświeżanie listy danych.
        /// </summary>
        public void RefreshList()
        {
            GenerateAddressesList();
        }

        /// <summary>
        /// Metoda zwracająca ilość rekordów. 
        /// </summary>
        public int recordCount
        {
            get { return addressesList.Count(); }
        }

        /// <summary>
        /// Metoda zwracająca dany rekord, zależnie od podanego indentyfikatora.
        /// </summary>
        /// <param name="id">Id rekordu.</param>
        /// <returns>Rekord w postaci zawartej w bazie danych.</returns>
        public Address this[int id]
        {
            get
            {
                return addressesList[id];
            }
        }
    }
}
