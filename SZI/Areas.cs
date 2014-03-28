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
                "IdTerenu",
                "Ulica",
                "IdInkasenta"
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

        private void GenerateStringList()
        {
            itemList.Clear();
            foreach (var item in areasList)
                itemList.Add(item.GetElements);
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
