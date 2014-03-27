﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    class Collectors : IDataBase
    {
        public List<Collector> collectorList;
        public string[] columnList { get; set; }
        public string className { get; set; }
        public List<string[]> itemList { get; set; }

        public Collectors()
        {
            collectorList = new List<Collector>();
            itemList = new List<string[]>();

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

            RefreshList();
        }

        private void GenerateItemList()
        {
            collectorList.Clear();
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                foreach (var value in dataBase.Collectors)
                    collectorList.Add(value);
            }
        }

        private void GenerateStringList()
        {
            itemList.Clear();
            foreach (var item in collectorList)
                itemList.Add(item.GetElements);
        }

        public void RefreshList()
        {
            GenerateItemList();
            GenerateStringList();
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