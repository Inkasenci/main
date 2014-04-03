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
    public partial class InsertForm : Form
    {
        private int selectedTab = 0;

        private bool CollectorEPInitialized = false, CustomerEPInitialized = false, AreaEPInitialized = false, CounterEPInitialized = false;
        private Dictionary<string, ValidatingMethod> NameToMethod_Dict;
        private Dictionary<Control, ErrorProvider> TBtoEP_Collector_Dict, TBtoEP_Customer_Dict, TBtoEP_Area_Dict, TBtoEP_Counter_Dict, Current_TBtoEP_Dict;
        private Dictionary<Control, bool> TBtoBool_Collector_Dict, TBtoBool_Customer_Dict, TBtoBool_Area_Dict, TBtoBool_Counter_Dict, Current_TBtoBool_Dict;
        private ComboBoxConfig cbcCustomer;

        public InsertForm(int MainFormSelectedTab)
        {            
            InitializeComponent();
            SetupControls(); 
            selectedTab = MainFormSelectedTab;
            tcInsert.SelectTab(MainFormSelectedTab); 
            InitializeEP(selectedTab);                      
        }

        private void SetupControls()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            NameToMethod_Dict = Auxiliary.Insert_CreateNameToMethodDict();
            cbcCustomer = new ComboBoxConfig("Customer", "cbCustomer", new Point(89, 81));
            tcInsert.TabPages[3].Controls.Add(cbcCustomer.InitializeComboBox());                       
        }


        #region Inicjalizacja słowników i przypisywanie EventHandlerów do TextBoxów
        
        private void InitializeCollectorDictAndTB()
        {
            TBtoEP_Collector_Dict = new Dictionary<Control, ErrorProvider>();
            TBtoBool_Collector_Dict = new Dictionary<Control, bool>();

            tbCollectorID.Validating += Validation;
            TBtoEP_Collector_Dict.Add(tbCollectorID, Auxiliary.InitializeErrorProvider(tbCollectorID));
            TBtoBool_Collector_Dict.Add(tbCollectorID, false);
            
            tbCollectorFirstName.Validating += Validation;
            TBtoEP_Collector_Dict.Add(tbCollectorFirstName, Auxiliary.InitializeErrorProvider(tbCollectorFirstName));
            TBtoBool_Collector_Dict.Add(tbCollectorFirstName, false);

            tbCollectorLastName.Validating += Validation;
            TBtoEP_Collector_Dict.Add(tbCollectorLastName, Auxiliary.InitializeErrorProvider(tbCollectorLastName));
            TBtoBool_Collector_Dict.Add(tbCollectorLastName, false);

            tbCollectorPostalCode.Validating += Validation;
            TBtoEP_Collector_Dict.Add(tbCollectorPostalCode, Auxiliary.InitializeErrorProvider(tbCollectorPostalCode));
            TBtoBool_Collector_Dict.Add(tbCollectorPostalCode, false);

            tbCollectorCity.Validating += Validation;
            TBtoEP_Collector_Dict.Add(tbCollectorCity, Auxiliary.InitializeErrorProvider(tbCollectorCity));
            TBtoBool_Collector_Dict.Add(tbCollectorCity, false);

            tbCollectorAddress.Validating += Validation;
            TBtoEP_Collector_Dict.Add(tbCollectorAddress, Auxiliary.InitializeErrorProvider(tbCollectorAddress));
            TBtoBool_Collector_Dict.Add(tbCollectorAddress, false);

            tbCollectorPhoneNumber.Validating += Validation;
            TBtoEP_Collector_Dict.Add(tbCollectorPhoneNumber, Auxiliary.InitializeErrorProvider(tbCollectorPhoneNumber));
            TBtoBool_Collector_Dict.Add(tbCollectorPhoneNumber, false);
        }

        private void InitializeCustomerDictAndTB()
        {
            TBtoEP_Customer_Dict = new Dictionary<Control, ErrorProvider>();
            TBtoBool_Customer_Dict = new Dictionary<Control, bool>();

            tbCustomerID.Validating += Validation;
            TBtoEP_Customer_Dict.Add(tbCustomerID, Auxiliary.InitializeErrorProvider(tbCustomerID));
            TBtoBool_Customer_Dict.Add(tbCustomerID, false);

            tbCustomerFirstName.Validating += Validation;
            TBtoEP_Customer_Dict.Add(tbCustomerFirstName, Auxiliary.InitializeErrorProvider(tbCustomerFirstName));
            TBtoBool_Customer_Dict.Add(tbCustomerFirstName, false);

            tbCustomerLastName.Validating += Validation;
            TBtoEP_Customer_Dict.Add(tbCustomerLastName, Auxiliary.InitializeErrorProvider(tbCustomerLastName));
            TBtoBool_Customer_Dict.Add(tbCustomerLastName, false);

            tbCustomerPostalCode.Validating += Validation;
            TBtoEP_Customer_Dict.Add(tbCustomerPostalCode, Auxiliary.InitializeErrorProvider(tbCustomerPostalCode));
            TBtoBool_Customer_Dict.Add(tbCustomerPostalCode, false);

            tbCustomerCity.Validating += Validation;
            TBtoEP_Customer_Dict.Add(tbCustomerCity, Auxiliary.InitializeErrorProvider(tbCustomerCity));
            TBtoBool_Customer_Dict.Add(tbCustomerCity, false);

            tbCustomerAddress.Validating += Validation;
            TBtoEP_Customer_Dict.Add(tbCustomerAddress, Auxiliary.InitializeErrorProvider(tbCustomerAddress));
            TBtoBool_Customer_Dict.Add(tbCustomerAddress, false);

            tbCustomerPhoneNumber.Validating += Validation;
            TBtoEP_Customer_Dict.Add(tbCustomerPhoneNumber, Auxiliary.InitializeErrorProvider(tbCustomerPhoneNumber));
            TBtoBool_Customer_Dict.Add(tbCustomerPhoneNumber, false);
        }

        private void InitializeAreaDictAndTB()
        {
            TBtoEP_Area_Dict = new Dictionary<Control, ErrorProvider>();
            TBtoBool_Area_Dict = new Dictionary<Control, bool>();

            tbStreet.Validating += Validation;
            TBtoEP_Area_Dict.Add(tbStreet, Auxiliary.InitializeErrorProvider(tbStreet));
            TBtoBool_Area_Dict.Add(tbStreet, false);

            tbAreaCollectorID.Validating += Validation;
            TBtoEP_Area_Dict.Add(tbAreaCollectorID, Auxiliary.InitializeErrorProvider(tbAreaCollectorID));
            TBtoBool_Area_Dict.Add(tbAreaCollectorID, false);
        }

        private void InitializeCounterDictAndTB()
        {
            TBtoEP_Counter_Dict = new Dictionary<Control, ErrorProvider>();
            TBtoBool_Counter_Dict = new Dictionary<Control, bool>();

            tbCounterNo.Validating += Validation;
            TBtoEP_Counter_Dict.Add(tbCounterNo, Auxiliary.InitializeErrorProvider(tbCounterNo));
            TBtoBool_Counter_Dict.Add(tbCounterNo, false);

            tbCircuitNo.Validating += Validation;
            TBtoEP_Counter_Dict.Add(tbCircuitNo, Auxiliary.InitializeErrorProvider(tbCircuitNo));
            TBtoBool_Counter_Dict.Add(tbCircuitNo, false);

            tbCounterAddressID.Validating += Validation;
            TBtoEP_Counter_Dict.Add(tbCounterAddressID, Auxiliary.InitializeErrorProvider(tbCounterAddressID));
            TBtoBool_Counter_Dict.Add(tbCounterAddressID, false);

            ComboBox cbCustomer = (ComboBox)this.Controls.Find("cbCustomer", true)[0];
            cbCustomer.Validating += ComboBoxValidation;
            TBtoEP_Counter_Dict.Add(cbCustomer, Auxiliary.InitializeErrorProvider(cbCustomer));
            TBtoBool_Counter_Dict.Add(cbCustomer, false);
        }

        private void InitializeEP(int tabPage)
        {
            switch (tabPage)
            {
                case 0:
                    InitializeCollectorDictAndTB();
                    Current_TBtoEP_Dict = TBtoEP_Collector_Dict;
                    Current_TBtoBool_Dict = TBtoBool_Collector_Dict;
                    break;

                case 1:
                    InitializeCustomerDictAndTB();
                    Current_TBtoEP_Dict = TBtoEP_Customer_Dict;
                    Current_TBtoBool_Dict = TBtoBool_Customer_Dict;
                    break;

                case 2:
                    InitializeAreaDictAndTB();
                    Current_TBtoEP_Dict = TBtoEP_Area_Dict;
                    Current_TBtoBool_Dict = TBtoBool_Area_Dict;
                    break;

                case 3:
                    InitializeCounterDictAndTB();
                    Current_TBtoEP_Dict = TBtoEP_Counter_Dict;
                    Current_TBtoBool_Dict = TBtoBool_Counter_Dict;
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
            tbAreaCollectorID.Text = "";
        }

        private void ClearTBCounter()
        {
            tbCounterNo.Text = "";
            tbCounterAddressID.Text = "";
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

            if (Auxiliary.IsCurrentValueOK(Current_TBtoBool_Dict))
            {
                c.InsertIntoDB();
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

            if (Auxiliary.IsCurrentValueOK(Current_TBtoBool_Dict))
            {
                c.InsertIntoDB();
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
            a.CollectorId = tbAreaCollectorID.Text;
            a.Street = tbStreet.Text;

            if (Auxiliary.IsCurrentValueOK(Current_TBtoBool_Dict))
            {
                a.InsertIntoDB();
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
            c.AddressId = new Guid();//new Guid(tbCounterAddressID.Text);
            c.CustomerId = cbcCustomer.ReturnForeignKey();

            if (Auxiliary.IsCurrentValueOK(Current_TBtoBool_Dict))
            {
                c.InsertIntoDB();
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
                Current_TBtoEP_Dict[cb].SetError(cb, String.Empty);
                Current_TBtoBool_Dict[cb] = true;
            }
            else
            {
                Current_TBtoEP_Dict[cb].SetError(cb, "Nieprawidłowo wypełnione pole.");
                Current_TBtoBool_Dict[cb] = false;
            }
        }

        private void Validation(object sender, CancelEventArgs e)
        {
            TextBox ValidatedTextBox = (TextBox)sender;

            if (NameToMethod_Dict[ValidatedTextBox.Name](ValidatedTextBox.Text))
            {
                Current_TBtoEP_Dict[ValidatedTextBox].SetError(ValidatedTextBox, String.Empty);
                ValidatedTextBox.Text = MainValidation.UppercaseFirst(ValidatedTextBox.Text);
                Current_TBtoBool_Dict[ValidatedTextBox] = true;
            }
            else
            {
                Current_TBtoEP_Dict[ValidatedTextBox].SetError(ValidatedTextBox, "Nieprawidłowo wypełnione pole.");
                Current_TBtoBool_Dict[ValidatedTextBox] = false;
                //e.Cancel = true;
            }
        }
        private void btOK_Click(object sender, EventArgs e)
        {
            switch (selectedTab)
            {
                case 0:
                    if (InsertCollector())
                        this.Close();
                    break;

                case 1:
                    if (InsertCustomer())
                        this.Close();
                    break;

                case 2:
                    if (InsertArea())
                        this.Close();
                    break;

                case 3:
                    if (InsertCounter())
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
                case 0:
                    ClearTBCollector();
                    break;

                case 1:
                    ClearTBCustomer();
                    break;

                case 2:
                    ClearTBArea();
                    break;

                case 3:
                    ClearTBCounter();
                    break;

                default:
                    break;
            }
        }

        private void tcInsert_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTab = tcInsert.SelectedIndex;
            switch (selectedTab)
            {
                case 0:
                    if (!CollectorEPInitialized)
                    {
                        InitializeEP(selectedTab);
                        CollectorEPInitialized = true;
                    }
                    Current_TBtoEP_Dict = TBtoEP_Collector_Dict;
                    Current_TBtoBool_Dict = TBtoBool_Collector_Dict;
                    break;

                case 1:
                    if (!CustomerEPInitialized)
                    {
                        InitializeEP(selectedTab);
                        CustomerEPInitialized = true;
                    }
                    Current_TBtoEP_Dict = TBtoEP_Customer_Dict;
                    Current_TBtoBool_Dict = TBtoBool_Area_Dict;
                    break;

                case 2:
                    if (!AreaEPInitialized)
                    {
                        InitializeEP(selectedTab);
                        AreaEPInitialized = true;
                    }
                    Current_TBtoEP_Dict = TBtoEP_Area_Dict;
                    Current_TBtoBool_Dict = TBtoBool_Area_Dict;
                    break;

                case 3:
                    if (!CounterEPInitialized)
                    {
                        InitializeEP(selectedTab);
                        CounterEPInitialized = true;
                    }
                    Current_TBtoEP_Dict = TBtoEP_Counter_Dict;
                    Current_TBtoBool_Dict = TBtoBool_Counter_Dict;
                    break;

                default:
                    break;
            }
        }

        #endregion

    }
}
