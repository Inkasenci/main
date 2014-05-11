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
        public static List<Customer> customerList;
        public static string[] columnList = new string[] {
                "Id klienta",
                "Imię",
                "Nazwisko",
                "Kod pocztowy",
                "Miasto",
                "Adres",
                "Telefon kontaktowy"
            };
        public static string className = "Customers";
        public List<string[]> itemList { get; set; }

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

        private void GenerateItemList()
        {
            customerList.Clear();
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                foreach (var value in dataBase.Customers)
                    customerList.Add(value);
            }
        }

        private void GenerateStringList()
        {
            itemList.Clear();
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
