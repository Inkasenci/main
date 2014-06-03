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
        public static string[] columnList = new string[] {
                "Id terenu",
                "Ulica",
                "Id inkasenta"
            };
        public static string className = "Areas";
        public List<string[]> itemList { get; set; }

        public Areas()
        {
            areasList = new List<Area>();
            itemList = new List<string[]>();

            className = this.GetType().Name;

            RefreshList();
        }

        private void GenerateAreasList()
        {
            List<string[]> Areas = null;

            using (var database = new CollectorsManagementSystemEntities())
            {
                areasList = (from area in database.Areas
                             select area).ToList();

                var result = (from area in database.Areas
                              join collector in database.Collectors
                              on area.CollectorId equals collector.CollectorId into gj
                              select new
                              {
                                  areaid = area.AreaId,
                                  street = area.Street,
                                  coll =
                                  (
                                    from subCollector in database.Collectors
                                    where subCollector.CollectorId == area.CollectorId
                                    select subCollector.Name + " " + subCollector.LastName                                    
                                  ).ToList()
                              }).ToList();


                Areas = new List<string[]>(result.Count());
                for (int i = 0; i < result.Count(); i++)
                {
                    Areas.Add(new string[3]);
                    Areas[i][0] = result[i].areaid.ToString();
                    Areas[i][1] = result[i].street;
                    Areas[i][2] = result[i].coll.Count == 0 ? "" : result[i].coll[0];
                }                
            }

            this.itemList = Areas;
        }

        public void RefreshList()
        {
            GenerateAreasList();
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
