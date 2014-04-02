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
        public string[] columnList { get; set; }
        public string className { get; set; }
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

        private void GenerateStringList()
        {
            itemList.Clear();
            foreach (var item in addressesList)
                itemList.Add(item.GetElements);
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
