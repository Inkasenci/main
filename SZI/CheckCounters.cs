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
                "Adres",
                "Ostatni odczyt",
                "Wartość odczytu",
                "Id Inkasenta"
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
                            };

                foreach (var element in items)
                {
                    var reading = from read in dataBase.Readings
                                  where read.CounterNo == element.CounterNo
                                  select read;
                    string date, value, collectorId;

                    if (reading.Count() > 0)
                    {
                        var lastRead = reading.OrderByDescending(x => x.Date).FirstOrDefault();
                        date = lastRead.Date.ToShortDateString() ?? "Brak danych!";
                        value = lastRead.Value.ToString() ?? "Brak danych!";
                        collectorId = lastRead.CollectorId ?? "Brak danych!";
                    }
                    else
                    {
                        date = "Brak danych!";
                        value = "Brak danych!";
                        collectorId = "Brak danych!";
                    }
                    cList.Add(new string[] { i++.ToString(), element.CounterNo.ToString(), element.CircuitNo.ToString(), element.CounterAddress, date, value, collectorId });
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
                MessageBox.Show("Błędna nazwa pliku!");
            else if (check == DialogResult.OK)
                StaticXML.XMLExport(saveFileDialog.FileName, this.collectorId, this.cList);
        }
    }
}
