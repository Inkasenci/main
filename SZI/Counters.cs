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
                "NumerLicznika",
                "NumerUkładu",
                "IdAdresu",
                "IdKlienta"
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

        private void GenerateStringList()
        {
            itemList.Clear();
            foreach (var item in counterList)
                itemList.Add(item.GetElements);
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
