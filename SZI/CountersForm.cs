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

namespace SZI
{
    public partial class CountersForm : Form
    {
        private ListView listView;
        private List<CountersFormClass> ccList;
        private string selectedid;
        private ListView.SelectedIndexCollection index;

        string[] columnList = new string[]
        {
            "Id Inkasenta",
            "Imię",
            "Nazwisko",
            "Liczb odczytów"
        };

        public CountersForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        public void InitializeForm()
        {
            listView = new ListView();
            ccList = new List<CountersFormClass>();

            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                foreach (var value in dataBase.Collectors)
                {
                    
                    var itemCount = (from collector in dataBase.Collectors
                                   join area in dataBase.Areas on collector.CollectorId equals area.CollectorId
                                   join address in dataBase.Addresses on area.AreaId equals address.AreaId
                                   join counter in dataBase.Counters on address.AddressId equals counter.AddressId
                                   where collector.CollectorId == value.CollectorId
                                   select counter).Count();
                    ccList.Add(new CountersFormClass(value.CollectorId, value.Name, value.LastName, itemCount));
                }
            }

            listView = ListViewConfig.ListViewInit(columnList, this.GetType().Name, ConvertToListOfStrings(ccList));
            listView.SelectedIndexChanged += lv_SelectedIndexChanged;
            listView.MultiSelect = false;
            this.Controls.Add(listView);
        }

        void lv_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView activeListView = (ListView)sender;
            index = activeListView.SelectedIndices;

            ListView.SelectedListViewItemCollection selectedItem = activeListView.SelectedItems;

            foreach (ListViewItem item in selectedItem)
                selectedid = item.SubItems[0].Text;
        }

        List<string[]> ConvertToListOfStrings(List<CountersFormClass> list)
        {
            List<string[]> output = new List<string[]>();

            foreach (var element in list)
                output.Add(new string[] {element.CollectorId, element.FirstName, element.LastName, element.CountersCount.ToString() });

            return output;
        }

        private void btCheck_Click(object sender, EventArgs e)
        {
            var checkForm = new CheckCounters(selectedid);
            checkForm.ShowDialog();
        }

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
                StaticXML.XMLImport(openFileDialog.FileName);
        }

    }
}
