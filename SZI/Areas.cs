using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    class Areas : IDataBase
    {
        static public List<Area> areasList;
        public string[] columnList { get; set; }
        public string className { get; set; }
        public List<string[]> itemList { get; set; }

        public Areas()
        {
            areasList = new List<Area>();
            itemList = new List<string[]>();

            columnList = new string[3] {
                "Id terenu",
                "Ulica",
                "Id inkasenta"
            };

            className = this.GetType().Name;

            RefreshList();
        }

        private void GenerateItemList()
        {
            areasList.Clear();
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                foreach (var value in dataBase.Areas)                    
                    areasList.Add(value);                
            }
        }

        private string FetchCollector(string CollectorID) //zwraca imię i nazwisko inkasenta na podstawie jego ID
        {
            string FullName = "";

            using (var database = new CollectorsManagementSystemEntities())
            {
                var collector = from c in database.Collectors 
                                where c.CollectorId == CollectorID 
                                select c;

                if (collector.Count() == 1)
                {
                    foreach (Collector c in collector)
                        FullName = c.Name + " " + c.LastName;
                }
            }

            return FullName;
        }

        private void GenerateStringList()
        {
            List<string> convertedItem;

            itemList.Clear();
            foreach (var item in areasList)
            {
                convertedItem = new List<string>();
                convertedItem.Add(item.AreaId.ToString());
                convertedItem.Add(item.Street);
                convertedItem.Add(FetchCollector(item.CollectorId));
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
            get { return areasList.Count(); }
        }

        public Area this[int id]
        {
            get
            {
                return areasList[id];
            }
        }
    }
}
