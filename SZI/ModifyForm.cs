using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    public partial class ModifyForm : Form
    {
        private List<string> ids; //identyfikatory zaznaczonych wierszy
        private int selectedTab; //aktualnie otwarta karta
        private string[] labelsTexts; //teksty etykiet
        private string[] textBoxesNames; //nazwy pol tekstowych
        private string[] textBoxesTexts; //teksty pol tekstowych
        private string[] comboBoxesNames; //nazwy rozwijanych list
        private string[] comboBoxesKeys; //klucze obce z danego rekordu, potrzebne do inicjalizacji ComboBoxConfigów
        private string[] TableNames; //nazwy tabel z których wyciągane są klucze obce modyfikowanego rekordu

        private CollectorsManagementSystemEntities dataBase; //odwolanie do bazy

        private Dictionary<Control, ErrorProvider> ControlToEP_Dict;
        private Dictionary<string, ValidatingMethod> NameToMethod_Dict;
        private Dictionary<Control, bool> ControlToBool_Dict;
        private ComboBoxConfig[] CBConfigs;

        public ModifyForm(List<string> ids, int selectedTab)
        /* przyjmuje id zaznaczonych rekordow i aktualnie otwarta karte
         * wstawia kontrolki i inicjalizuje je*/
        {
            InitializeComponent();
            dataBase = new CollectorsManagementSystemEntities();
            ErrorProvider ep;
            int i;

            this.Text = "Modyfikacja rekordu";
            this.ids = ids;
            this.selectedTab = selectedTab;

            switch (selectedTab)
            {
                case 0:
                    labelsTexts = new string[] { "Id inkasenta: ", "Imię: ", "Nazwisko: ", "Kod pocztowy: ", "Miasto: ", "Ulica: ", "Telefon kontaktowy: " };
                    textBoxesNames = new string[] { "CollectorId", "Name", "LastName", "PostalCode", "City", "Address", "PhoneNumber" };
                    Collector modifiedCollector = dataBase.Collectors.SqlQuery("SELECT * FROM Collector WHERE CollectorId={0}", ids.ElementAt(0)).SingleOrDefault();
                    textBoxesTexts = new string[] { modifiedCollector.CollectorId, modifiedCollector.Name, modifiedCollector.LastName, Regex.Replace(modifiedCollector.PostalCode, "([0-9]{2})([0-9]{3})", "${1}-${2}"), modifiedCollector.City, modifiedCollector.Address, modifiedCollector.PhoneNumber };
                    break;
                case 1:
                    labelsTexts = new string[] { "Id klienta: ", "Imię: ", "Nazwisko: ", "Kod pocztowy: ", "Miasto: ", "Ulica: ", "Telefon kontaktowy: " };
                    textBoxesNames = new string[] { "CustomerId", "Name", "LastName", "PostalCode", "City", "Address", "PhoneNumber" };
                    Customer modifiedCustomer = dataBase.Customers.SqlQuery("SELECT * FROM Customer WHERE CustomerId={0}", ids.ElementAt(0)).SingleOrDefault();
                    textBoxesTexts = new string[] { modifiedCustomer.CustomerId, modifiedCustomer.Name, modifiedCustomer.LastName, Regex.Replace(modifiedCustomer.PostalCode, "([0-9]{2})([0-9]{3})", "${1}-${2}"), modifiedCustomer.City, modifiedCustomer.Address, modifiedCustomer.PhoneNumber };
                    break;
                case 2:
                    labelsTexts = new string[] { "Id terenu: ", "Ulica: ", "Id inkasenta: " };
                    textBoxesNames = new string[] { "AreaId", "Street"};
                    Area modifiedArea = dataBase.Areas.SqlQuery("SELECT * FROM Area WHERE AreaId={0}", ids.ElementAt(0)).SingleOrDefault();
                    textBoxesTexts = new string[] { modifiedArea.AreaId.ToString(), modifiedArea.Street};
                    
                    comboBoxesNames = new string[] { "cbCollector" };
                    comboBoxesKeys = new string[] { modifiedArea.CollectorId };
                    TableNames = new string[] {"Collector"};
                    break;
                case 3:
                    labelsTexts = new string[] { "Numer licznika: ", "Numer układu: ", "Id adresu: ", "Id klienta: " };
                    textBoxesNames = new string[] { "CounterNo", "CircuitNo" };
                    Counter modifiedCounter = dataBase.Counters.SqlQuery("SELECT * FROM Counter WHERE CounterNo={0}", ids.ElementAt(0)).SingleOrDefault();
                    textBoxesTexts = new string[] { modifiedCounter.CounterNo.ToString(), modifiedCounter.CircuitNo.ToString() };
                    
                    comboBoxesNames = new string[] { "cbAddress", "cbCustomer" };
                    comboBoxesKeys = new string[] { modifiedCounter.AddressId.ToString(), modifiedCounter.CustomerId };
                    TableNames = new string[] {"Address", "Customer"};
                    
                    break;
                case 4:
                    labelsTexts = new string[] { "Id adresu: ", "Numer domu: ", "Numer mieszkania: ", "Id terenu: " };
                    textBoxesNames = new string[] { "AddressId", "HouseNo", "FlatNo" };
                    Address modifiedAddress = dataBase.Addresses.SqlQuery("SELECT * FROM Address WHERE AddressId={0}", ids.ElementAt(0)).SingleOrDefault();
                    textBoxesTexts = new string[] { modifiedAddress.AddressId.ToString(), modifiedAddress.HouseNo.ToString(), modifiedAddress.FlatNo.ToString()};
                    
                    comboBoxesNames = new string[] { "cbArea" };
                    comboBoxesKeys = new string[] { modifiedAddress.AreaId.ToString() };
                    TableNames = new string[] {"Area"};
                    
                    break;
                default:
                    break;
            }

            NameToMethod_Dict = Auxiliary.Modify_CreateNameToMethodDict();
            Label[] labels = InitializeLabels();
            TextBox[] textBoxes = InitializeTextAndCBConfigs();
            ControlToEP_Dict = new Dictionary<Control, ErrorProvider>();
            ControlToBool_Dict = new Dictionary<Control, bool>();

            for (i = 0; i < textBoxesNames.Length; i++) //inicjowanie labeli które opisują textboxy
            {
                this.Controls.Add(labels[i]);
                this.Controls.Add(textBoxes[i]);
                if (i != 0)
                {
                    ep = Auxiliary.InitializeErrorProvider(textBoxes[i]);
                    ControlToEP_Dict.Add(textBoxes[i], ep);
                    ControlToBool_Dict.Add(textBoxes[i], true);
                    textBoxes[i].Validating += Validation;
                }
            }

            if (comboBoxesNames!=null)                
                for (int j = 0; j < comboBoxesNames.Length; j++)
                {
                    this.Controls.Add(labels[i + j]);
                    this.Controls.Add(CBConfigs[j].comboBox);
                    ep = Auxiliary.InitializeErrorProvider(CBConfigs[j].comboBox);
                    ControlToEP_Dict.Add(CBConfigs[j].comboBox, ep);
                    ControlToBool_Dict.Add(CBConfigs[j].comboBox, true);
                    if (selectedTab != 3)
                        CBConfigs[j].comboBox.Validating += ComboBoxValidation;
                    else //jeśli modyfikowany jest wpis w Counters
                        CBConfigs[j].comboBox.Validating += CountersValidation;
                }
        }

        private Label[] InitializeLabels() //inicjalizuje etykiety
        {
            Label[] labels = new Label[labelsTexts.Length];
            for (int i = 0; i < labelsTexts.Length; i++)
            {
                labels[i] = new Label();
                labels[i].Text = labelsTexts[i];
                labels[i].Location = new Point(10, 30 * (i + 1));
            }
            return labels;
        }

        private TextBox[] InitializeTextAndCBConfigs() //inicjalizuje pola tekstowe i rozwijane listy
        {
            TextBox[] textBoxes = new TextBox[textBoxesTexts.Length];            
            int i;

            for (i = 0; i < textBoxesTexts.Length; i++)
            {                
                textBoxes[i] = new TextBox();                
                textBoxes[i].Name = textBoxesNames[i];
                textBoxes[i].Text = textBoxesTexts[i];
                textBoxes[i].Location = new Point(150, 30 * (i + 1));
            }

            if (comboBoxesNames != null)
            {
                CBConfigs = new ComboBoxConfig[comboBoxesNames.Length];
                for (int j = 0; j < comboBoxesNames.Length; j++)
                {
                    CBConfigs[j] = new ComboBoxConfig(TableNames[j], comboBoxesNames[j], new Point(150, 30 * (i + 1)), comboBoxesKeys[j]);
                    i++;
                }
            }

            textBoxes[0].Enabled = false;

            return textBoxes;
        }

        private void btCancel_Click(object sender, EventArgs e) //po nacisnieciu przycisku Anuluj
        {
            this.Close();
        }

        private void btSave_Click(object sender, EventArgs e) //po nacisnieciu przycisku Zapisz
        {
            int Parse;
            
            switch (selectedTab)
            {
                case 0:
                    if (Auxiliary.IsCurrentValueOK(ControlToBool_Dict))
                    {
                        Collector modifiedCollector = new Collector();
                        modifiedCollector.CollectorId = this.Controls.Find("CollectorId", true)[0].Text;
                        modifiedCollector.Name = this.Controls.Find("Name", true)[0].Text;
                        modifiedCollector.LastName = this.Controls.Find("LastName", true)[0].Text;
                        modifiedCollector.PostalCode = this.Controls.Find("PostalCode", true)[0].Text;
                        modifiedCollector.City = this.Controls.Find("City", true)[0].Text;
                        modifiedCollector.Address = this.Controls.Find("Address", true)[0].Text;
                        modifiedCollector.PhoneNumber = this.Controls.Find("PhoneNumber", true)[0].Text;
                        modifiedCollector.ModifyRecord(ids.ElementAt(0));
                        this.Close();
                    }
                    else
                        MessageBox.Show(LangPL.InsertFormLang["Fill in all fields"]);    
                    break;

                case 1:
                    if (Auxiliary.IsCurrentValueOK(ControlToBool_Dict))
                    {
                        Customer modifiedCustomer = new Customer();
                        modifiedCustomer.CustomerId = this.Controls.Find("CustomerId", true)[0].Text;
                        modifiedCustomer.Name = this.Controls.Find("Name", true)[0].Text;
                        modifiedCustomer.LastName = this.Controls.Find("LastName", true)[0].Text;
                        modifiedCustomer.PostalCode = this.Controls.Find("PostalCode", true)[0].Text;
                        modifiedCustomer.City = this.Controls.Find("City", true)[0].Text;
                        modifiedCustomer.Address = this.Controls.Find("Address", true)[0].Text;
                        modifiedCustomer.PhoneNumber = this.Controls.Find("PhoneNumber", true)[0].Text;
                        modifiedCustomer.ModifyRecord(ids.ElementAt(0));
                        this.Close();
                    }
                    else
                        MessageBox.Show(LangPL.InsertFormLang["Fill in all fields"]);  
                    break;

                case 2:                    
                    if (Auxiliary.IsCurrentValueOK(ControlToBool_Dict))
                    {
                        Area modifiedArea = new Area();
                        modifiedArea.AreaId = new Guid(this.Controls.Find("AreaId", true)[0].Text);
                        modifiedArea.Street = this.Controls.Find("Street", true)[0].Text;
                        if (CBConfigs[0].ReturnForeignKey() == "")
                            modifiedArea.CollectorId = "";
                        else
                            modifiedArea.CollectorId = CBConfigs[0].ReturnForeignKey();
                        modifiedArea.ModifyRecord(ids.ElementAt(0));
                        this.Close();
                    }
                    else
                        MessageBox.Show(LangPL.InsertFormLang["Fill in all fields"]);
                    break;

                case 3:
                    if (Auxiliary.IsCurrentValueOK(ControlToBool_Dict))
                    {
                        Counter modifiedCounter = new Counter();

                        modifiedCounter.CounterNo = Convert.ToInt32(this.Controls.Find("CounterNo", true)[0].Text);
                        Int32.TryParse(this.Controls.Find("CircuitNo", true)[0].Text, out Parse);
                        modifiedCounter.CircuitNo = Parse;

                        if (CBConfigs[0].comboBox.SelectedIndex != 0) //jeśli w jednym comboboxie index jest różny od 0, to w drugim też
                        {
                            modifiedCounter.AddressId = new Guid(CBConfigs[0].ReturnForeignKey());
                            modifiedCounter.CustomerId = CBConfigs[1].ReturnForeignKey();
                        }
                        else
                        {
                            modifiedCounter.AddressId = null;
                            modifiedCounter.CustomerId = null;
                        }

                        modifiedCounter.ModifyRecord(ids.ElementAt(0));
                        this.Close();
                    }
                    else
                        MessageBox.Show(LangPL.InsertFormLang["Fill in all fields"]);  
                    break;

                case 4:     
                    if (Auxiliary.IsCurrentValueOK(ControlToBool_Dict))
                    {
                        Address modifiedAddress = new Address();
                        modifiedAddress.AddressId = new Guid(this.Controls.Find("AddressId", true)[0].Text);
                        Int32.TryParse(this.Controls.Find("HouseNo", true)[0].Text, out Parse);
                        modifiedAddress.HouseNo = Parse;
                        Int32.TryParse(this.Controls.Find("FlatNo", true)[0].Text, out Parse);
                        if (Parse > 0)
                            modifiedAddress.FlatNo = Parse;
                        else
                            modifiedAddress.FlatNo = null;
                        modifiedAddress.AreaId = new Guid(CBConfigs[0].ReturnForeignKey());
                        modifiedAddress.ModifyRecord(ids.ElementAt(0));
                        this.Close();
                    }
                    else
                        MessageBox.Show(LangPL.InsertFormLang["Fill in all fields"]);  
                    break;
            }
        }

        private void CountersValidation(object sender, CancelEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb == CBConfigs[0].comboBox)
            {
                MainValidation.XNOR_ComboBoxValidation(cb, CBConfigs[1].comboBox, ControlToEP_Dict, ControlToBool_Dict);
            }
            else
                MainValidation.XNOR_ComboBoxValidation(cb, CBConfigs[0].comboBox, ControlToEP_Dict, ControlToBool_Dict);
        }

        private void ComboBoxValidation(object sender, CancelEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (NameToMethod_Dict[cb.Name](cb.SelectedIndex.ToString()))
            {
                ControlToEP_Dict[cb].SetError(cb, String.Empty);
                ControlToBool_Dict[cb] = true;
            }
            else
            {
                ControlToEP_Dict[cb].SetError(cb, "Nieprawidłowo wypełnione pole.");
                ControlToBool_Dict[cb] = false;
            }
        }

        private void Validation(object sender, CancelEventArgs e)
        {
            TextBox ValidatedTextBox = (TextBox)sender;
            
            if (NameToMethod_Dict[ValidatedTextBox.Name](ValidatedTextBox.Text))
            {
                ControlToEP_Dict[ValidatedTextBox].SetError(ValidatedTextBox, String.Empty);
                ControlToBool_Dict[ValidatedTextBox] = true;
                ValidatedTextBox.Text = MainValidation.UppercaseFirst(ValidatedTextBox.Text);
            }
            else
            {
                ControlToEP_Dict[ValidatedTextBox].SetError(ValidatedTextBox, "Nieprawidłowo wypełnione pole.");
                ControlToBool_Dict[ValidatedTextBox] = false;
            }
        }
    }
}
