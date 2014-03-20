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
        public ListView lv { get; set; }
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
            lv = new ListView();
            lv.View = View.Details;
            lv.FullRowSelect = true;

            foreach (var column in columnList)
                lv.Columns.Add(column);

            lv.Location = new System.Drawing.Point(10, 10);
            lv.Size = new System.Drawing.Size(450, 450);
            lv.Name = className;

            foreach (var counter in customerList)
                lv.Items.Add(ConvertToItem(counter));

            return lv;
        }
    }
}
