using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace SZI
{
    public partial class InsertForm : Form, IForm
    {
        private Tables selectedTab;

        private bool CollectorEPInitialized = false, CustomerEPInitialized = false, AreaEPInitialized = false, CounterEPInitialized = false, AddressEPInitialized = false;
        private Dictionary<string, ValidatingMethod> NameToMethod_Dict;
        private Dictionary<Control, ErrorProvider> ControltoEP_Collector_Dict, ControltoEP_Customer_Dict, ControltoEP_Area_Dict, ControltoEP_Counter_Dict, ControltoEP_Address_Dict, Current_ControltoEP_Dict;
        private Dictionary<Control, bool> ControlToBool_Collector_Dict, ControlToBool_Customer_Dict, ControlToBool_Area_Dict, ControlToBool_Counter_Dict, ControlToBool_Address_Dict, Current_ControlToBool_Dict;
        private ComboBoxConfig cbcCustomer, cbcCollector, cbcArea, cbcAddress;
        private bool modified = false;
        
        /// <summary>
        /// Tabela do której wstawiono rekord.
        /// </summary>
        public Tables InsertedTo;

        /// <summary>
        /// Zwraca wartość logiczną, która określa czy baza została zmieniona.
        /// </summary>
        public bool Modified
        {
            get
            {
                return this.modified;
            }
        }

        public InsertForm(Tables MainFormSelectedTab)
        {
            InitializeComponent();
            SetupControls();
            selectedTab = MainFormSelectedTab;
            tcInsert.SelectTab((int)MainFormSelectedTab);
            InitializeEP(selectedTab);
        }

        private void SetupControls()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            NameToMethod_Dict = Auxiliary.Insert_CreateNameToMethodDict();
        }


        #region Inicjalizacja słowników i przypisywanie EventHandlerów do TextBoxów

        private void InitializeCollectorDictAndTB()
        {
            ControltoEP_Collector_Dict = new Dictionary<Control, ErrorProvider>();
            ControlToBool_Collector_Dict = new Dictionary<Control, bool>();

            tbCollectorID.Validating += Validation;
            ControltoEP_Collector_Dict.Add(tbCollectorID, Auxiliary.InitializeErrorProvider(tbCollectorID));
            ControlToBool_Collector_Dict.Add(tbCollectorID, false);

            tbCollectorFirstName.Validating += Validation;
            ControltoEP_Collector_Dict.Add(tbCollectorFirstName, Auxiliary.InitializeErrorProvider(tbCollectorFirstName));
            ControlToBool_Collector_Dict.Add(tbCollectorFirstName, false);

            tbCollectorLastName.Validating += Validation;
            ControltoEP_Collector_Dict.Add(tbCollectorLastName, Auxiliary.InitializeErrorProvider(tbCollectorLastName));
            ControlToBool_Collector_Dict.Add(tbCollectorLastName, false);

            tbCollectorPostalCode.Validating += Validation;
            ControltoEP_Collector_Dict.Add(tbCollectorPostalCode, Auxiliary.InitializeErrorProvider(tbCollectorPostalCode));
            ControlToBool_Collector_Dict.Add(tbCollectorPostalCode, false);

            tbCollectorCity.Validating += Validation;
            ControltoEP_Collector_Dict.Add(tbCollectorCity, Auxiliary.InitializeErrorProvider(tbCollectorCity));
            ControlToBool_Collector_Dict.Add(tbCollectorCity, false);

            tbCollectorAddress.Validating += Validation;
            ControltoEP_Collector_Dict.Add(tbCollectorAddress, Auxiliary.InitializeErrorProvider(tbCollectorAddress));
            ControlToBool_Collector_Dict.Add(tbCollectorAddress, false);

            tbCollectorPhoneNumber.Validating += Validation;
            ControltoEP_Collector_Dict.Add(tbCollectorPhoneNumber, Auxiliary.InitializeErrorProvider(tbCollectorPhoneNumber));
            ControlToBool_Collector_Dict.Add(tbCollectorPhoneNumber, false);
        }

        private void InitializeCustomerDictAndTB()
        {
            ControltoEP_Customer_Dict = new Dictionary<Control, ErrorProvider>();
            ControlToBool_Customer_Dict = new Dictionary<Control, bool>();

            tbCustomerID.Validating += Validation;
            ControltoEP_Customer_Dict.Add(tbCustomerID, Auxiliary.InitializeErrorProvider(tbCustomerID));
            ControlToBool_Customer_Dict.Add(tbCustomerID, false);

            tbCustomerFirstName.Validating += Validation;
            ControltoEP_Customer_Dict.Add(tbCustomerFirstName, Auxiliary.InitializeErrorProvider(tbCustomerFirstName));
            ControlToBool_Customer_Dict.Add(tbCustomerFirstName, false);

            tbCustomerLastName.Validating += Validation;
            ControltoEP_Customer_Dict.Add(tbCustomerLastName, Auxiliary.InitializeErrorProvider(tbCustomerLastName));
            ControlToBool_Customer_Dict.Add(tbCustomerLastName, false);

            tbCustomerPostalCode.Validating += Validation;
            ControltoEP_Customer_Dict.Add(tbCustomerPostalCode, Auxiliary.InitializeErrorProvider(tbCustomerPostalCode));
            ControlToBool_Customer_Dict.Add(tbCustomerPostalCode, false);

            tbCustomerCity.Validating += Validation;
            ControltoEP_Customer_Dict.Add(tbCustomerCity, Auxiliary.InitializeErrorProvider(tbCustomerCity));
            ControlToBool_Customer_Dict.Add(tbCustomerCity, false);

            tbCustomerAddress.Validating += Validation;
            ControltoEP_Customer_Dict.Add(tbCustomerAddress, Auxiliary.InitializeErrorProvider(tbCustomerAddress));
            ControlToBool_Customer_Dict.Add(tbCustomerAddress, false);

            tbCustomerPhoneNumber.Validating += Validation;
            ControltoEP_Customer_Dict.Add(tbCustomerPhoneNumber, Auxiliary.InitializeErrorProvider(tbCustomerPhoneNumber));
            ControlToBool_Customer_Dict.Add(tbCustomerPhoneNumber, false);
        }

        private void InitializeAreaDictAndTB()
        {
            cbcCollector = new ComboBoxConfig("Collector", "cbCollector", new Point(86, 29));
            tcInsert.TabPages[2].Controls.Add(cbcCollector.InitializeComboBox());

            ControltoEP_Area_Dict = new Dictionary<Control, ErrorProvider>();
            ControlToBool_Area_Dict = new Dictionary<Control, bool>();

            tbStreet.Validating += Validation;
            ControltoEP_Area_Dict.Add(tbStreet, Auxiliary.InitializeErrorProvider(tbStreet));
            ControlToBool_Area_Dict.Add(tbStreet, false);

            ComboBox cbCollector = (ComboBox)this.Controls.Find("cbCollector", true)[0];
            cbCollector.Validating += ComboBoxValidation;
            ControltoEP_Area_Dict.Add(cbCollector, Auxiliary.InitializeErrorProvider(cbCollector));
            ControlToBool_Area_Dict.Add(cbCollector, true);
        }

        private void InitializeCounterDictAndTB()
        {
            cbcAddress = new ComboBoxConfig("Address", "cbAddress", new Point(89, 55));
            tcInsert.TabPages[3].Controls.Add(cbcAddress.InitializeComboBox());

            cbcCustomer = new ComboBoxConfig("Customer", "cbCustomer", new Point(89, 81));
            tcInsert.TabPages[3].Controls.Add(cbcCustomer.InitializeComboBox());

            ControltoEP_Counter_Dict = new Dictionary<Control, ErrorProvider>();
            ControlToBool_Counter_Dict = new Dictionary<Control, bool>();

            tbCounterNo.Validating += Validation;
            ControltoEP_Counter_Dict.Add(tbCounterNo, Auxiliary.InitializeErrorProvider(tbCounterNo));
            ControlToBool_Counter_Dict.Add(tbCounterNo, false);

            tbCircuitNo.Validating += Validation;
            ControltoEP_Counter_Dict.Add(tbCircuitNo, Auxiliary.InitializeErrorProvider(tbCircuitNo));
            ControlToBool_Counter_Dict.Add(tbCircuitNo, false);

            ComboBox cbAddress = (ComboBox)this.Controls.Find("cbAddress", true)[0];
            cbAddress.Validating += CountersValidation;
            ControltoEP_Counter_Dict.Add(cbAddress, Auxiliary.InitializeErrorProvider(cbAddress));
            ControlToBool_Counter_Dict.Add(cbAddress, true);

            ComboBox cbCustomer = (ComboBox)this.Controls.Find("cbCustomer", true)[0];
            cbCustomer.Validating += CountersValidation;
            ControltoEP_Counter_Dict.Add(cbCustomer, Auxiliary.InitializeErrorProvider(cbCustomer));
            ControlToBool_Counter_Dict.Add(cbCustomer, true);
        }

        private void InitializeAddressDictAndTB()
        {
            cbcArea = new ComboBoxConfig("Area", "cbArea", new Point(117, 55));
            tcInsert.TabPages[4].Controls.Add(cbcArea.InitializeComboBox());

            ControltoEP_Address_Dict = new Dictionary<Control, ErrorProvider>();
            ControlToBool_Address_Dict = new Dictionary<Control, bool>();

            tbHouseNo.Validating += Validation;
            ControltoEP_Address_Dict.Add(tbHouseNo, Auxiliary.InitializeErrorProvider(tbHouseNo));
            ControlToBool_Address_Dict.Add(tbHouseNo, false);

            ComboBox cbArea = (ComboBox)this.Controls.Find("cbArea", true)[0];
            cbArea.Validating += ComboBoxValidation;
            ControltoEP_Address_Dict.Add(cbArea, Auxiliary.InitializeErrorProvider(cbArea));
            ControlToBool_Address_Dict.Add(cbArea, false);
        }

        private void InitializeEP(Tables tabPage)
        {
            switch (tabPage)
            {
                case Tables.Collectors:
                    InitializeCollectorDictAndTB();
                    CollectorEPInitialized = true;
                    Current_ControltoEP_Dict = ControltoEP_Collector_Dict;
                    Current_ControlToBool_Dict = ControlToBool_Collector_Dict;
                    break;

                case Tables.Customers:
                    InitializeCustomerDictAndTB();
                    CustomerEPInitialized = true;
                    Current_ControltoEP_Dict = ControltoEP_Customer_Dict;
                    Current_ControlToBool_Dict = ControlToBool_Customer_Dict;
                    break;

                case Tables.Areas:
                    InitializeAreaDictAndTB();
                    AreaEPInitialized = true;
                    Current_ControltoEP_Dict = ControltoEP_Area_Dict;
                    Current_ControlToBool_Dict = ControlToBool_Area_Dict;
                    break;

                case Tables.Counters:
                    InitializeCounterDictAndTB();
                    CounterEPInitialized = true;
                    Current_ControltoEP_Dict = ControltoEP_Counter_Dict;
                    Current_ControlToBool_Dict = ControlToBool_Counter_Dict;
                    break;

                case Tables.Addresses:
                    InitializeAddressDictAndTB();
                    AddressEPInitialized = true;
                    Current_ControltoEP_Dict = ControltoEP_Address_Dict;
                    Current_ControlToBool_Dict = ControlToBool_Address_Dict;
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Czyszczenie textboxów
        private void ClearTBCollector()
        {
            tbCollectorID.Text = "";
            tbCollectorFirstName.Text = "";
            tbCollectorLastName.Text = "";
            tbCollectorPostalCode.Text = "";
            tbCollectorCity.Text = "";
            tbCollectorAddress.Text = "";
            tbCollectorPhoneNumber.Text = "";
        }

        private void ClearTBCustomer()
        {
            tbCustomerID.Text = "";
            tbCustomerFirstName.Text = "";
            tbCustomerLastName.Text = "";
            tbCustomerPostalCode.Text = "";
            tbCustomerCity.Text = "";
            tbCustomerAddress.Text = "";
            tbCustomerPhoneNumber.Text = "";
        }

        private void ClearTBArea()
        {
            tbStreet.Text = "";
        }

        private void ClearTBCounter()
        {
            tbCounterNo.Text = "";
        }

        private void ClearTBAddress()
        {
            tbHouseNo.Text = "";
            tbFlatNo.Text = "";
        }

        #endregion

        #region Wprowadzanie do bazy
        private bool InsertCollector()
        {
            Collector c = new Collector();
            c.CollectorId = tbCollectorID.Text;
            c.Name = tbCollectorFirstName.Text;
            c.LastName = tbCollectorLastName.Text;
            c.PostalCode = tbCollectorPostalCode.Text;
            c.City = tbCollectorCity.Text;
            c.Address = tbCollectorAddress.Text;
            c.PhoneNumber = tbCollectorPhoneNumber.Text;

            if (Auxiliary.IsCurrentValueOK(Current_ControlToBool_Dict))
            {
                c.InsertIntoDB();
		        modified = true;
                InsertedTo = Tables.Collectors;
                return true;
            }
            else
            {
                MessageBox.Show(LangPL.InsertFormLang["Fill in all fields"]);
                return false;
            }
        }

        private bool InsertCustomer()
        {
            Customer c = new Customer();
            c.CustomerId = tbCustomerID.Text;
            c.Name = tbCustomerFirstName.Text;
            c.LastName = tbCustomerLastName.Text;
            c.PostalCode = tbCustomerPostalCode.Text;
            c.City = tbCustomerCity.Text;
            c.Address = tbCustomerAddress.Text;
            c.PhoneNumber = tbCustomerPhoneNumber.Text;

            if (Auxiliary.IsCurrentValueOK(Current_ControlToBool_Dict))
            {
                c.InsertIntoDB();
                modified = true;
                InsertedTo = Tables.Customers;
                return true;
            }
            else
            {
                MessageBox.Show(LangPL.InsertFormLang["Fill in all fields"]);
                return false;
            }
        }

        private bool InsertArea()
        {
            Area a = new Area();
            a.AreaId = Guid.NewGuid();
            a.CollectorId = cbcCollector.ReturnForeignKey();
            a.Street = tbStreet.Text;

            if (Auxiliary.IsCurrentValueOK(Current_ControlToBool_Dict))
            {
                a.InsertIntoDB();
                modified = true;
                InsertedTo = Tables.Areas;
                return true;
            }
            else
            {
                MessageBox.Show(LangPL.InsertFormLang["Fill in all fields"]);
                return false;
            }
        }

        private bool InsertCounter()
        {
            int Parse;
            Counter c = new Counter();

            Int32.TryParse(tbCounterNo.Text, out Parse);
            c.CounterNo = Parse;
            Int32.TryParse(tbCircuitNo.Text, out Parse);
            c.CircuitNo = Parse;            

            if (Auxiliary.IsCurrentValueOK(Current_ControlToBool_Dict))
            {
                if (cbcAddress.comboBox.SelectedIndex != 0) //jeśli w jednym comboboxie index jest różny od 0, to w drugim też
                {
                    c.AddressId = new Guid(cbcAddress.ReturnForeignKey());
                    c.CustomerId = cbcCustomer.ReturnForeignKey();

                }
                c.InsertIntoDB();
                modified = true;
                InsertedTo = Tables.Counters;
                return true;
            }
            else
            {
                MessageBox.Show(LangPL.InsertFormLang["Fill in all fields"]);
                return false;
            }
        }

        private bool InsertAddress()
        {
            int Parse;
            Address a = new Address();

            a.AddressId = Guid.NewGuid();
            Int32.TryParse(tbHouseNo.Text, out Parse);
            a.HouseNo = Parse;
            Int32.TryParse(tbFlatNo.Text, out Parse);
            if (Parse > 0)
                a.FlatNo = Parse;

            if (Auxiliary.IsCurrentValueOK(Current_ControlToBool_Dict))
            {
                a.AreaId = new Guid(cbcArea.ReturnForeignKey());
                a.InsertIntoDB();
                modified = true;
                InsertedTo = Tables.Addresses;
                return true;
            }
            else
            {
                MessageBox.Show(LangPL.InsertFormLang["Fill in all fields"]);
                return false;
            }
        }

        #endregion

        #region EventHandlery
        private void ComboBoxValidation(object sender, CancelEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (NameToMethod_Dict[cb.Name](cb.SelectedIndex.ToString()))
            {
                Current_ControltoEP_Dict[cb].SetError(cb, String.Empty);
                Current_ControlToBool_Dict[cb] = true;
            }
            else
            {
                Current_ControltoEP_Dict[cb].SetError(cb, "Nieprawidłowo wypełnione pole.");
                Current_ControlToBool_Dict[cb] = false;
            }
        }

        private void CountersValidation(object sender, CancelEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb == cbcAddress.comboBox)           
                MainValidation.XNOR_ComboBoxValidation(cb, cbcCustomer.comboBox, Current_ControltoEP_Dict, Current_ControlToBool_Dict);
           
            else
                MainValidation.XNOR_ComboBoxValidation(cb, cbcAddress.comboBox, Current_ControltoEP_Dict, Current_ControlToBool_Dict);
        }

        private void Validation(object sender, CancelEventArgs e)
        {
            TextBox ValidatedTextBox = (TextBox)sender;

            if (NameToMethod_Dict[ValidatedTextBox.Name](ValidatedTextBox.Text))
            {
                Current_ControltoEP_Dict[ValidatedTextBox].SetError(ValidatedTextBox, String.Empty);
                ValidatedTextBox.Text = MainValidation.UppercaseFirst(ValidatedTextBox.Text);
                Current_ControlToBool_Dict[ValidatedTextBox] = true;
            }
            else
            {
                Current_ControltoEP_Dict[ValidatedTextBox].SetError(ValidatedTextBox, "Nieprawidłowo wypełnione pole.");
                Current_ControlToBool_Dict[ValidatedTextBox] = false;
                //e.Cancel = true;
            }
        }
        private void btOK_Click(object sender, EventArgs e)
        {
            switch (selectedTab)
            {
                case Tables.Collectors:
                    if (InsertCollector())
                        this.Close();
                    break;

                case Tables.Customers:
                    if (InsertCustomer())
                        this.Close();
                    break;

                case Tables.Areas:
                    if (InsertArea())
                        this.Close();
                    break;

                case Tables.Counters:
                    if (InsertCounter())
                        this.Close();
                    break;

                case Tables.Addresses:
                    if (InsertAddress())
                        this.Close();
                    break;

                default:
                    break;
            }
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            switch (selectedTab)
            {
                case Tables.Collectors:
                    ClearTBCollector();
                    break;

                case Tables.Customers:
                    ClearTBCustomer();
                    break;

                case Tables.Areas:
                    ClearTBArea();
                    break;

                case Tables.Counters:
                    ClearTBCounter();
                    break;

                case Tables.Addresses:
                    ClearTBAddress();
                    break;

                default:
                    break;
            }
        }

        private void tcInsert_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTab = (Tables)tcInsert.SelectedIndex;
            switch (selectedTab)
            {
                case Tables.Collectors:
                    if (!CollectorEPInitialized)
                    {
                        InitializeEP(selectedTab);
                        CollectorEPInitialized = true;
                    }
                    Current_ControltoEP_Dict = ControltoEP_Collector_Dict;
                    Current_ControlToBool_Dict = ControlToBool_Collector_Dict;
                    break;

                case Tables.Customers:
                    if (!CustomerEPInitialized)
                    {
                        InitializeEP(selectedTab);
                        CustomerEPInitialized = true;
                    }
                    Current_ControltoEP_Dict = ControltoEP_Customer_Dict;
                    Current_ControlToBool_Dict = ControlToBool_Customer_Dict;
                    break;

                case Tables.Areas:
                    if (!AreaEPInitialized)
                    {
                        InitializeEP(selectedTab);
                        AreaEPInitialized = true;
                    }
                    Current_ControltoEP_Dict = ControltoEP_Area_Dict;
                    Current_ControlToBool_Dict = ControlToBool_Area_Dict;
                    break;

                case Tables.Counters:
                    if (!CounterEPInitialized)
                    {
                        InitializeEP(selectedTab);
                        CounterEPInitialized = true;
                    }
                    Current_ControltoEP_Dict = ControltoEP_Counter_Dict;
                    Current_ControlToBool_Dict = ControlToBool_Counter_Dict;
                    break;

                case Tables.Addresses:
                    if (!AddressEPInitialized)
                    {
                        InitializeEP(selectedTab);
                        AddressEPInitialized = true;
                    }
                    Current_ControltoEP_Dict = ControltoEP_Address_Dict;
                    Current_ControlToBool_Dict = ControlToBool_Address_Dict;
                    break;

                default:
                    break;
            }
        }

        #endregion

        private void InsertForm_FormClosing(object sender, FormClosingEventArgs e)
        {
                  
        }

        private void InsertForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Thread t = new Thread(() => ListViewDataManipulation.RefreshListView(sender));
            t.Start();
        }

    }
}
