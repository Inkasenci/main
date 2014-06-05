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

        public void RefreshList()
        {
            GenerateAddressesList();
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
