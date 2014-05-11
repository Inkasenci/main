using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    class Addresses : IDataBase
    {
        static public List<Address> addressesList;
        public static string[] columnList = new string[] {
                "Id adresu",
                "Numer domu",
                "Numer mieszkania",
                "Id terenu"
            };
        public static string className = "Addresses";
        public List<string[]> itemList { get; set; }

        public Addresses()
        {
            addressesList = new List<Address>();
            itemList = new List<string[]>();

            columnList = new string[4] {
                "Id adresu",
                "Numer domu",
                "Numer mieszkania",
                "Id terenu"
            };

            className = this.GetType().Name;

            RefreshList();
        }

        private void GenerateItemList()
        {
            addressesList.Clear();
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                foreach (var value in dataBase.Addresses)
                    addressesList.Add(value);
            }
        }

        static public string FetchAreaAndCollector(Guid AreaID) //zwraca przypisany do adresu teren i przypisanego do niego inkasenta
        {
            string AreaAndCollector = "";

            using (var database = new CollectorsManagementSystemEntities())
            {
                var Result = from area in database.Areas
                             join collector in database.Collectors on area.CollectorId equals collector.CollectorId into areaJoinedCollector
                             where area.AreaId == AreaID
                             from collector in areaJoinedCollector.DefaultIfEmpty()
                             select new { Area = area, Collector = collector == null ? String.Empty : ": " + collector.Name + " " + collector.LastName };

                if (Result.Count() == 1)
                {
                    foreach (var item in Result)
                        AreaAndCollector = item.Area.Street + item.Collector;             
                }
            }

            return AreaAndCollector;
        }

        private void GenerateStringList()
        {
            List<string> convertedItem;

            itemList.Clear();

            foreach (var item in addressesList)
            {
                convertedItem = new List<string>();
                convertedItem.Add(item.AddressId.ToString());
                convertedItem.Add(item.HouseNo.ToString());
                convertedItem.Add(item.FlatNo.ToString());
                convertedItem.Add(FetchAreaAndCollector(item.AreaId));
                itemList.Add(convertedItem.ToArray());
            }
        }

        public void RefreshList()
        {
            GenerateItemList();
            GenerateStringList();
        }

        public int recordCount
        {
            get { return addressesList.Count(); }
        }

        public Address this[int id]
        {
            get
            {
                return addressesList[id];
            }
        }
    }
}
