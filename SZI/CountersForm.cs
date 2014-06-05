using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace SZI
{
    /// <summary>
    /// Formularz obsługujący liczniki.
    /// </summary>
    public partial class CountersForm : Form
    {
        /// <summary>
        /// ListView wyświetlające inkasentów i odpowiadające im liczniki.
        /// </summary>
        private ListView listView;
        /// <summary>
        /// Lista elementów wyświetalnych w listView.
        /// </summary>
        private List<CountersFormClass> ccList;
        /// <summary>
        /// Wybrany z listy indeks.
        /// </summary>
        private string selectedid = "0";
        /// <summary>
        /// Zaznaczone na liście elementy.
        /// </summary>
        private ListView.SelectedIndexCollection index;
        /// <summary>
        /// Lista kolumn w listView.
        /// </summary>
        string[] columnList = new string[]
        {
            "Id Inkasenta",
            "Imię",
            "Nazwisko",
            "Liczb odczytów"
        };

        /// <summary>
        /// Konstruktor formy.
        /// </summary>
        public CountersForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        /// <summary>
        /// Zwraca aktualne dane dotyczące odczytów.
        /// </summary>
        /// <returns>Lista tablic stringów zawierająca dane.</returns>
        public List<string[]> ReturnListViewData()
        {
            ccList = new List<CountersFormClass>();

            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                foreach (var value in dataBase.Collectors)
                {
                    int i = 0;
                    var items = (from collector in dataBase.Collectors
                                 join area in dataBase.Areas on collector.CollectorId equals area.CollectorId
                                 join address in dataBase.Addresses on area.AreaId equals address.AreaId
                                 join counter in dataBase.Counters on address.AddressId equals counter.AddressId
                                 where collector.CollectorId == value.CollectorId
                                 select counter);

                    foreach (var element in items)
                    {
                        var date = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0));
                        var firstMethod = from read in dataBase.Readings
                                           where read.Date > date
                                           where read.CounterNo == element.CounterNo
                                           select read;

                        if (firstMethod.Count() == 0)
                            i++;
                    }

                    ccList.Add(new CountersFormClass(value.CollectorId, value.Name, value.LastName, i));
                }
            }

            return ConvertToListOfStrings(ccList);
        }

        /// <summary>
        /// Inicjacja listy - pobranie rekordów i umieszczenie ich w ListView.
        /// </summary>
        public void InitializeForm()
        {
            listView = new ListView();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            listView = ListViewConfig.ListViewInit(columnList, this.GetType().Name, ReturnListViewData());
            listView.SelectedIndexChanged += lv_SelectedIndexChanged;
            listView.MultiSelect = false;
            btCheck.Enabled = false;

            this.Controls.Add(listView);
        }

        /// <summary>
        /// Konwersja listy elementów do stringów.
        /// </summary>
        /// <param name="list">Lista dostępnych elementów.</param>
        /// <returns>Lista tablic stringów (format pozwalający wyświetlić rekordy).</returns>
        List<string[]> ConvertToListOfStrings(List<CountersFormClass> list)
        {
            List<string[]> output = new List<string[]>();

            foreach (var element in list)
                output.Add(new string[] {element.CollectorId, element.FirstName, element.LastName, element.CountersCount.ToString() });

            return output;
        }

        /// <summary>
        /// Zmiana wybranego indeksu.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        void lv_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView activeListView = (ListView)sender;
            index = activeListView.SelectedIndices;

            ListView.SelectedListViewItemCollection selectedItem = activeListView.SelectedItems;

            foreach (ListViewItem item in selectedItem)
                selectedid = item.SubItems[0].Text;

            switch (activeListView.SelectedItems.Count)
            {
                case 1:
                    btCheck.Enabled = true;
                    btGenerateMission.Enabled = true;
                    break;
                default:
                    btGenerateMission.Enabled = false;
                    btCheck.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// Uruchomienie okna dla danego inkasenta, które jest zależne od argumentu.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void btCheck_Click(object sender, EventArgs e)
        {
            var checkForm = new CheckCounters(selectedid);
            checkForm.ShowDialog();
        }

        /// <summary>
        /// Import (wczytanie) danych z pliku - odczytów.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void btImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.DefaultExt = "xml";
            openFileDialog.Filter = "XML file|*.xml";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Title = "Wczytaj dane";
            DialogResult check = openFileDialog.ShowDialog();

            if (openFileDialog.FileName == String.Empty && check == DialogResult.OK && File.Exists(openFileDialog.FileName))
                MessageBox.Show("Błąd! Brak pliku!");
            else if (check == DialogResult.OK)
            {
                CountersCollection cCollection;
                StaticXML.ReadFromXml(openFileDialog.FileName, true, out cCollection);
                if (cCollection != null)
                {
                    ListViewConfig.ListViewRefresh(listView, ReturnListViewData());
                }
            }
        }

        /// <summary>
        /// Event handler przycisku tworzącego misję dla zaznaczonego inkasenta.
        /// </summary>
        /// <param name="sender">Przycisk btGenerateMission.</param>
        /// <param name="e">Parametry zdarzenia.</param>
        private void btGenerateMission_Click(object sender, EventArgs e)
        {
            String id = Auxiliary.CreateIdList(listView)[0];
            PrintPreviewDialog printPreviewDialog = Reports.Mission.CreateMission(id);

            try
            {
                printPreviewDialog.Show();
            }
            catch (InvalidPrinterException ex)
            {
                ExceptionHandling.ShowException(ex);
            }
        }
    }
}
