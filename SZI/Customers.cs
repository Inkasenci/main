using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SZI
{
    class Customers : IDataBase
    {
        public List<Customer> customerList;
        public string[] columnList { get; set; }
        public string className { get; set; }
        public List<string[]> itemList { get; set; }

        public Customers()
        {
            customerList = new List<Customer>();
            itemList = new List<string[]>();

            columnList = new string[7] {
                "IdKlienta",
                "Imię",
                "Nazwisko",
                "KodPocztowy",
                "Miasto",
                "Adres",
                "TelefonKontaktowy"
            };

            className = this.GetType().Name;

            RefreshList();
        }

        private void GenerateItemList()
        {
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                foreach (var value in dataBase.Customers)
                    customerList.Add(value);
            }
        }

        private void GenerateStringList()
        {
            foreach (var item in customerList)
                itemList.Add(item.GetElements);
        }

        public void RefreshList()
        {
            GenerateItemList();
            GenerateStringList();
        }

        public int recordCount
        {
            get { return customerList.Count(); }
        }

        public Customer this[int id]
        {
            get
            {
                return customerList[id];
            }
        }
    }
}
