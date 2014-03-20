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
        public ListView lv { get; set; }

        private List<Area> areasList;
        public string[] columnList { get; set; }
        public string className { get; set; }

        public Areas()
        {
            areasList = new List<Area>();
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                foreach (var value in dataBase.Areas)
                    areasList.Add(value);
            }

            columnList = new string[3] {
                "IdTerenu",
                "Ulica",
                "IdInkasenta"
            };

            className = this.GetType().Name;
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

        private ListViewItem ConvertToItem(Area area)
        {
            string[] infoGroup = new string[3]{
                area.AreaId.ToString(),
                area.CollectorId,
                area.Street
            };

            ListViewItem convertArea = new ListViewItem(infoGroup);

            return convertArea;
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

            foreach (var counter in areasList)
                lv.Items.Add(ConvertToItem(counter));

            return lv;
        }
    }
}
