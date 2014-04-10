using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    public partial class CheckCounters : Form
    {
        private string collectorId;
        private ListView listViewC;
        private List<string[]> cList;

        string[] columnList = new string[]
        {
                "Id",
                "Numer licznika",
                "Numer układu",
                "Właściciel",
                "Adres",
                "Data odczyt",
                "Wartość odczytu",
                "Id Inkasenta",
                "Imię i Nazwisko Inkasenta",
        };

        public CheckCounters( string collectorId )
        {
            this.collectorId = collectorId;
            InitializeComponent();
            InitializeForm();
        }

        public void InitializeForm()
        {
            listViewC = new ListView();
            cList = new List<string[]>();

            int i = 1;

            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                var items = from collector in dataBase.Collectors
                            join area in dataBase.Areas on collector.CollectorId equals area.CollectorId
                            join address in dataBase.Addresses on area.AreaId equals address.AreaId
                            join counter in dataBase.Counters on address.AddressId equals counter.AddressId
                            where collector.CollectorId == this.collectorId
                            select new
                            {
                                CounterNo = counter.CounterNo,
                                CircuitNo = counter.CircuitNo,
                                CounterAddress = area.Street + " " + address.HouseNo + "/" + address.FlatNo,
                                CustomerId = counter.CustomerId
                            };

                foreach (var element in items)
                {
                    var reading = from read in dataBase.Readings
                                  where read.CounterNo == element.CounterNo
                                  select read;

                    string date, value, collectorId, collectorName, customerName;

                    if (reading.Count() > 0)
                    {
                        var lastRead = reading.OrderByDescending(x => x.Date).FirstOrDefault();
                        date = lastRead.Date.ToShortDateString() ?? LangPL.CountersWarnings["noRecord"];
                        value = lastRead.Value.ToString() ?? LangPL.CountersWarnings["noRecord"];
                        collectorId = lastRead.CollectorId ?? LangPL.CountersWarnings["noRecord"];

                        var collectorRead = ( from collector in dataBase.Collectors
                                        where collector.CollectorId == collectorId
                                        select new { collector.Name, collector.LastName }).FirstOrDefault();

                        collectorName = collectorRead.Name + " " + collectorRead.LastName;
                    }
                    else
                    {
                        date = LangPL.CountersWarnings["noRecord"];
                        value = LangPL.CountersWarnings["noRecord"];
                        collectorId = LangPL.CountersWarnings["noRecord"];
                        collectorName = LangPL.CountersWarnings["noRecord"];
                    }

                    var cutomerRead = (from customer in dataBase.Customers
                                       where element.CustomerId == customer.CustomerId
                                       select new { customer.Name, customer.LastName }).FirstOrDefault();

                    if ( cutomerRead != null )
                        customerName = cutomerRead.Name + " " + cutomerRead.LastName;
                    else
                        customerName = LangPL.CountersWarnings["noRecord"];

                    cList.Add(new string[] { i++.ToString(), element.CounterNo.ToString(), element.CircuitNo.ToString(), customerName, element.CounterAddress, date, value, collectorId, collectorName });
                }
            }

            listViewC = ListViewConfig.ListViewInit(columnList, this.GetType().Name, cList);
            listViewC.MultiSelect = false;
            listViewC.Size = new System.Drawing.Size(750, 450);
            this.Controls.Add(listViewC);
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = @"C:\";
            saveFileDialog.DefaultExt = "xml";
            saveFileDialog.Filter = "XML file|*.xml";
            saveFileDialog.FilterIndex = 1; 
            saveFileDialog.Title = "Zapisz dane";
            DialogResult check = saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName == String.Empty && check == DialogResult.OK)
                MessageBox.Show(LangPL.CountersWarnings["wrongFileName"]);
            else if (check == DialogResult.OK)
                StaticXML.XMLExport(saveFileDialog.FileName, this.collectorId, this.cList);
        }
    }
}
