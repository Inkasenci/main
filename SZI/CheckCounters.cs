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
    /// <summary>
    /// Klasa obsługująca liczniki dla wybranego inkasenta.
    /// </summary>
    public partial class CheckCounters : Form
    {
        /// <summary>
        /// Id inkasenta, dla którego szukamy odczytów.
        /// </summary>
        private string collectorId;
        /// <summary>
        /// Obsługa listView.
        /// </summary>
        private ListView listViewC;
        /// <summary>
        /// Kolekcja rekordów odpowiadających inkasentowi.
        /// </summary>
        private CountersCollection cCollection;
        /// <summary>
        /// Ilość odczytów do wykonania.
        /// </summary>
        private int nrOfCounters = 0;
        /// <summary>
        /// Lista kolumn w listView.
        /// </summary>
        string[] columnList = new string[]
        {
                "Id",
                "Numer licznika",
                "Numer układu",
                "Właściciel",
                "Adres",
                "Data odczytu",
                "Inkasent",
                "Wartość odczytu"
        };

        /// <summary>
        /// Konstruktor formy.
        /// </summary>
        /// <param name="collectorId">Pobranie danych odbywa się względem danego w argumencie rekordu.</param>
        public CheckCounters(string collectorId)
        {
            this.collectorId = collectorId;
            InitializeComponent();
        }

        /// <summary>
        /// Metoda inicjalizująca wyświetlanie listy rekordów.
        /// </summary>
        public void InitializeForm()
        {
            listViewC = new ListView();
            cCollection = new CountersCollection();
            cCollection.collectorId = this.collectorId;
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
                    var dateSub = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0));

                    var firstMethod = from read in dataBase.Readings
                                      where read.Date > dateSub
                                      where read.CounterNo == element.CounterNo
                                      select read;

                    if (firstMethod.Count() == 0)
                    {

                        string date, value, collectorId, collectorName, customerName;

                        date = value = collectorName = String.Empty;

                        var lastReading = from read in dataBase.Readings
                                          where read.CounterNo == element.CounterNo
                                          select read;

                        if (lastReading.Count() > 0)
                        {
                            var lastRead = lastReading.OrderByDescending(x => x.Date).FirstOrDefault();

                            date = lastRead.Date.ToShortDateString() ?? String.Empty;
                            value = lastRead.Value.ToString() ?? String.Empty;
                            collectorId = lastRead.CollectorId ?? String.Empty;

                            try
                            {
                                var collectorRead = (from collector in dataBase.Collectors
                                                     where collector.CollectorId == collectorId
                                                     select new { collector.Name, collector.LastName }).FirstOrDefault();

                                collectorName = collectorId + " " + collectorRead.Name + " " + collectorRead.LastName;
                            }
                            catch
                            {
                                collectorName = "---";
                            }
                        }
                        else
                            date = value = collectorName = String.Empty;

                        var cutomerRead = (from customer in dataBase.Customers
                                           where element.CustomerId == customer.CustomerId
                                           select new { customer.Name, customer.LastName }).FirstOrDefault();

                        if (cutomerRead != null)
                            customerName = cutomerRead.Name + " " + cutomerRead.LastName;
                        else
                            customerName = String.Empty;

                        {
                            CounterXML newItem = new CounterXML();
                            newItem.ReadId = i++.ToString();
                            newItem.CounterNo = element.CounterNo.ToString();
                            newItem.CircuitNo = element.CircuitNo.ToString();
                            newItem.Customer = customerName;
                            newItem.Address = element.CounterAddress;
                            newItem.LastReadDate = date;
                            newItem.LastValue = value;
                            newItem.LastCollector = collectorName;
                            newItem.NewValue = String.Empty;
                            cCollection.AddNewElement(newItem);
                            nrOfCounters++;
                        }
                    }
                }
            }

            listViewC = ListViewConfig.ListViewInit(columnList, this.GetType().Name);

            if ( cCollection.RecordsCount > 0 )
                foreach (var element in cCollection.counter)
                    ListViewConfig.AddItem(listViewC, element.PrintStringArray);

            listViewC.MultiSelect = false;
            listViewC.Size = new System.Drawing.Size(750, 450);
            if (nrOfCounters == 0)
                btExport.Enabled = false;
            this.Controls.Add(listViewC);
        }

        /// <summary>
        /// Export (wysłanie) danych do pliku XML.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void btExport_Click(object sender, EventArgs e)
        {
            CountersCollection collection = new CountersCollection();
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
            {
                bool test;
                StaticXML.WriteToXml(saveFileDialog.FileName, cCollection, out test);
            }
        }
    }
}
