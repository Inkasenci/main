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
    class Areas : IDataBase
    {
        /// <summary>
        /// Lista terenów.
        /// </summary>
        static public List<Area> areasList;

        /// <summary>
        /// Tablica zawierająca listę kolumn.
        /// </summary>
        public static string[] columnList = new string[] {
                "Id terenu",
                "Ulica",
                "Id inkasenta"
            };

        /// <summary>
        /// Nazwa klasy.
        /// </summary>
        public static string className = "Areas";

        /// <summary>
        /// Lista itemów - używana podczas odświeżania listView.
        /// </summary>
        public List<string[]> itemList { get; set; }

        /// <summary>
        /// Konstruktor inicjujący pola związane z klasą.
        /// </summary>
        public Areas()
        {
            areasList = new List<Area>();
            itemList = new List<string[]>();

            className = this.GetType().Name;

            RefreshList();
        }

        /// <summary>
        /// Generuje listę terenów.
        /// </summary>
        private void GenerateAreasList()
        {
            List<string[]> Areas = null;

            using (var database = new CollectorsManagementSystemEntities())
            {
                areasList = (from area in database.Areas
                             select area).ToList();

                var result = (from area in database.Areas
                              join collector in database.Collectors
                              on area.CollectorId equals collector.CollectorId into gj
                              select new
                              {
                                  areaid = area.AreaId,
                                  street = area.Street,
                                  coll =
                                  (
                                    from subCollector in database.Collectors
                                    where subCollector.CollectorId == area.CollectorId
                                    select subCollector.Name + " " + subCollector.LastName                                    
                                  ).ToList()
                              }).ToList();


                Areas = new List<string[]>(result.Count());
                for (int i = 0; i < result.Count(); i++)
                {
                    Areas.Add(new string[3]);
                    Areas[i][0] = result[i].areaid.ToString();
                    Areas[i][1] = result[i].street;
                    Areas[i][2] = result[i].coll.Count == 0 ? "" : result[i].coll[0];
                }                
            }

            this.itemList = Areas;
        }

        /// <summary>
        /// Funkcja odpowiedzialna za odświeżanie listy danych.
        /// </summary>
        public void RefreshList()
        {
            GenerateAreasList();
        }

        /// <summary>
        /// Metoda zwracająca ilość rekordów. 
        /// </summary>
        public int recordCount
        {
            get { return areasList.Count(); }
        }

        /// <summary>
        /// Metoda zwracająca dany rekord, zależnie od podanego indentyfikatora.
        /// </summary>
        /// <param name="id">Id rekordu.</param>
        /// <returns>Rekord w postaci zawartej w bazie danych.</returns>
        public Area this[int id]
        {
            get
            {
                return areasList[id];
            }
        }
    }
}
