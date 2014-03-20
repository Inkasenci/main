using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    class Collectors : IDataBase
    {
        public ListView lv { get; set; }

        private List<Collector> collectorList;
        public string[] columnList { get; set; }
        public string className { get; set; }

        public Collectors()
        {
            collectorList = new List<Collector>();
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                foreach (var value in dataBase.Collectors)
                    collectorList.Add(value);
            }

            columnList = new string[7] { 
                "IdInkasenta", 
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
            get { return collectorList.Count(); }
        }

        public Collector this[int id]
        {
            get
            {
                return collectorList[id];
            }
        }

        private ListViewItem ConvertToItem(Collector collector)
        {
            string[] infoGroup = new string[7]{
                collector.CollectorId,
                collector.Name,
                collector.LastName,
                collector.PostalCode,
                collector.City,
                collector.Address,
                collector.PhoneNumber
            };

            ListViewItem convertCollector = new ListViewItem(infoGroup);

            return convertCollector;
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

            foreach (var counter in collectorList)
                lv.Items.Add(ConvertToItem(counter));
            return lv;
        }

        public void DeleteRowsByID(List<string> ids)
        {
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    dataBase.Collectors.SqlQuery("delete from Collectors where CollectorId={0}", ids[i]);
                }
            }
        }
    }
}
