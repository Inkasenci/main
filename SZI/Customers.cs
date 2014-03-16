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
        private List<Customer> customerList;
        public string[] columnList { get; set; }
        public string className { get; set; }

        public Customers()
        {
            customerList = new List<Customer>();
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                foreach (var value in dataBase.Customers)
                    customerList.Add(value);
            }

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

        private ListViewItem ConvertToItem(Customer customer)
        {
            string[] infoGroup = new string[7]{
                customer.CustomerId,
                customer.Name,
                customer.LastName,
                customer.PostalCode,
                customer.City,
                customer.Address,
                customer.PhoneNumber
            };

            ListViewItem convertCustomer = new ListViewItem(infoGroup);

            return convertCustomer;
        }

        public ListView ListViewInitiate()
        {
            ListView listView = new ListView();
            listView.View = View.Details;

            foreach (var column in columnList)
                listView.Columns.Add(column);

            listView.Location = new System.Drawing.Point(10, 10);
            listView.Size = new System.Drawing.Size(450, 450);
            listView.Name = className;

            foreach (var customer in customerList)
                listView.Items.Add(ConvertToItem(customer));

            return listView;
        }
    }
}