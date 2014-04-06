using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    public class Counters : IDataBase
    {
        public List<Counter> counterList;
        public string[] columnList { get; set; }
        public string className { get; set; }
        public List<string[]> itemList { get; set; }

        public Counters()
        {
            counterList = new List<Counter>();
            itemList = new List<string[]>();

            columnList = new string[4] {
                "Numer licznika",
                "Numer układu",
                "Id adresu",
                "Id klienta"
            };

            className = this.GetType().Name;

            RefreshList();
        }

        private void GenerateItemList()
        {
            counterList.Clear();
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                foreach (var value in dataBase.Counters)
                    counterList.Add(value);
            }
        }

        private string FetchCustomer(string CustomerID) //zwraca imię i nazwisko klienta na podstawie jego ID
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

        private string FetchFullAddress(Guid AddressID) //zwraca imię i nazwisko klienta na podstawie jego ID
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
                        if (fa.Address.FlatNo != 0)
                            FullAddressString += "/" + fa.Address.FlatNo.ToString();
                    }
                }
            }

            return FullAddressString;
        }

        private void GenerateStringList()
        {
            List<string> convertedItem;

            itemList.Clear();
            foreach (var item in counterList)
            {
                convertedItem = new List<string>();
                convertedItem.Add(item.CounterNo.ToString());
                convertedItem.Add(item.CircuitNo.ToString());
                convertedItem.Add(FetchFullAddress(item.AddressId.Value));
                convertedItem.Add(FetchCustomer(item.CustomerId));
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
            get { return counterList.Count(); }
        }

        public Counter this[int id]
        {
            get
            {
                return counterList[id];
            }
        }
    }
}
