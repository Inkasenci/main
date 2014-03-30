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
        int selectedTab = 0;

        bool CollectorEPInitialized = false, CustomerEPInitialized = false, AreaEPInitialized = false, CounterEPInitialized = false;
        Dictionary<string, ValidatingMethod> NameToMethod_Dict;
        ErrorProvider errorProvider;
    
        public InsertForm(int MainFormSelectedTab)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
            selectedTab = MainFormSelectedTab;
            tcInsert.SelectTab(MainFormSelectedTab);
            NameToMethod_Dict = Auxiliary.Insert_CreateNameToMethodDict();
            InitializeEP(selectedTab);
            errorProvider = Auxiliary.InitializeErrorProvider();
        }

        #region Inicjalizacja słowników i przypisywanie event handlerów do TextBoxów
        private void InitializeCollectorDictAndTB()
        {
            tbCollectorID.Validating += Validation;
            tbCollectorFirstName.Validating += Validation;
            tbCollectorLastName.Validating += Validation;
            tbCollectorPostalCode.Validating += Validation;
            tbCollectorCity.Validating += Validation;
            tbCollectorAddress.Validating += Validation;
            tbCollectorPhoneNumber.Validating += Validation;
        }

        private void InitializeCustomerDictAndTB()
        {
            tbCustomerID.Validating += Validation;
            tbCustomerFirstName.Validating += Validation;
            tbCustomerLastName.Validating += Validation;
            tbCustomerPostalCode.Validating += Validation;
            tbCustomerCity.Validating += Validation;
            tbCustomerAddress.Validating += Validation;
            tbCustomerPhoneNumber.Validating += Validation;
        }

        private void InitializeAreaDictAndTB()
        {
            tbStreet.Validating += Validation;
            tbAreaCollectorID.Validating += Validation;
        }

        private void InitializeCounterDictAndTB()
        {
            tbCounterNo.Validating += Validation;
            tbCircuitNo.Validating += Validation;
            tbCounterAddressID.Validating += Validation;
            tbCounterCustomerID.Validating += Validation;
        }

        private void InitializeEP(int tabPage)
        {
            switch (tabPage)
            {
                case 0:
                    InitializeCollectorDictAndTB();
                    break;

                case 1:
                    InitializeCustomerDictAndTB();
                    break;

                case 2:
                    InitializeAreaDictAndTB();
                    break;

                case 3:
                    InitializeCounterDictAndTB();
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
            tbCounterCustomerID.Text = "";
            tbCounterAddressID.Text = "";
        }

        #endregion

        #region Wprowadzanie do bazy
        private void InsertCollector()
        {
            Collector c = new Collector();
            c.CollectorId = tbCollectorID.Text;
            c.Name = tbCollectorFirstName.Text;
            c.LastName = tbCollectorLastName.Text;
            c.PostalCode = tbCollectorPostalCode.Text;
            c.City = tbCollectorCity.Text;
            c.Address = tbCollectorAddress.Text;
            c.PhoneNumber = tbCollectorPhoneNumber.Text;
            c.InsertIntoDB();
        }

        private void InsertCustomer()
        {
            Customer c = new Customer();
            c.CustomerId = tbCustomerID.Text;
            c.Name = tbCustomerFirstName.Text;
            c.LastName = tbCustomerLastName.Text;
            c.PostalCode = tbCustomerPostalCode.Text;
            c.City = tbCustomerCity.Text;
            c.Address = tbCustomerAddress.Text;
            c.PhoneNumber = tbCustomerPhoneNumber.Text;
            c.InsertIntoDB();
        }

        private void InsertArea()
        {
            Area a = new Area();
            a.AreaId = Guid.NewGuid();
            a.CollectorId = tbAreaCollectorID.Text;
            a.Street = tbStreet.Text;
            a.InsertIntoDB();
        }

        private void InsertCounter()
        {
            int Parse;
            Counter c = new Counter();

            if (Int32.TryParse(tbCounterNo.Text, out Parse))
                c.CounterNo = Parse;
            if (Int32.TryParse(tbCircuitNo.Text, out Parse))
                c.CircuitNo = Parse;
            if (Int32.TryParse(tbCounterAddressID.Text, out Parse))
                c.AddressId = Auxiliary.ToGuid(Parse);
            else
                c.AddressId = Auxiliary.ToGuid(-1);
            c.CustomerId = tbCounterCustomerID.Text;
            c.InsertIntoDB();
        }

        #endregion

        #region EventHandlery
        private void Validation(object sender, CancelEventArgs e)
        {
            TextBox ValidatedTextBox = (TextBox)sender;
            Auxiliary.SetErrorProvider(errorProvider, ValidatedTextBox);

            if (NameToMethod_Dict[ValidatedTextBox.Name](ValidatedTextBox.Text))
            {
                errorProvider.SetError(ValidatedTextBox, String.Empty);
            }
            else
            {
                errorProvider.SetError(ValidatedTextBox, "Nieprawidłowo wypełnione pole.");
                e.Cancel = true;
            }
        }
        private void btOK_Click(object sender, EventArgs e)
        {
            switch (selectedTab)
            {
                case 0:
                    InsertCollector();
                    break;

                case 1:
                    InsertCustomer();
                    break;

                case 2:
                    InsertArea();
                    break;

                case 3:
                    InsertCounter();
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
                    break;

                case 1:
                    if (!CustomerEPInitialized)
                    {
                        InitializeEP(selectedTab);
                        CustomerEPInitialized = true;
                    }  
                    break;

                case 2:
                    if (!AreaEPInitialized)
                    {
                        InitializeEP(selectedTab);
                        AreaEPInitialized = true;
                    } 
                    break;

                case 3:
                    if (!CounterEPInitialized)
                    {
                        InitializeEP(selectedTab);
                        CounterEPInitialized = true;
                    } 
                    break;

                default:
                    break;
            }
        }

        #endregion

    }
}
