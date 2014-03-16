using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    class Counters : IDataBase
    {
        private List<Counter> counterList;
        public string[] columnList { get; set; }
        public string className { get; set; }

        public Counters()
        {
            counterList = new List<Counter>();
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                foreach (var value in dataBase.Counters)
                    counterList.Add(value);
            }

            columnList = new string[4] {
                "NumerLicznika",
                "NumerUkładu",
                "IdAdresu",
                "IdKlienta"
            };

            className = this.GetType().Name;
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

        private ListViewItem ConvertToItem(Counter counter)
        {
            string[] infoGroup = new string[4]{
                counter.CounterNo.ToString(),
                counter.CircuitNo.ToString(),
                counter.AddressId.ToString(),
                counter.CustomerId
            };

            ListViewItem convertCounter = new ListViewItem(infoGroup);

            return convertCounter;
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

            foreach (var counter in counterList)
                listView.Items.Add(ConvertToItem(counter));

            return listView;
        }
    }
}