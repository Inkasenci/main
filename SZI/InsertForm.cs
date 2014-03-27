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
        public InsertForm()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
        }

        public InsertForm(int MainFormSelectedTab)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
            tcInsert.SelectTab(MainFormSelectedTab);
        }

        #region Czyszczenie textboxów
        private void ClearTBCollector()
        {
            tbID.Text = "";
            tbFirstName.Text = "";
            tbLastName.Text = "";
            tbPostal.Text = "";
            tbCity.Text = "";
            tbAddress.Text = "";
            tbPhone.Text = "";
        }

        private void ClearTBCustomer()
        {
            tbCustomerID.Text = "";
            tbCustomerName.Text = "";
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
            c.CollectorId = tbID.Text;
            c.Name = tbFirstName.Text;
            c.LastName = tbLastName.Text;
            c.PostalCode = tbPostal.Text;
            c.City = tbCity.Text;
            c.Address = tbAddress.Text;
            c.PhoneNumber = tbPhone.Text;
            string validateString = MainValidation.CollectorValidateString(c);
            if (validateString == String.Empty)
                c.InsertIntoDB();
            else
                MessageBox.Show(validateString);
        }

        private void InsertCustomer()
        {
            Customer c = new Customer();
            c.CustomerId = tbCustomerID.Text;
            c.Name = tbCustomerName.Text;
            c.LastName = tbCustomerLastName.Text;
            c.PostalCode = tbCustomerPostalCode.Text;
            c.City = tbCustomerCity.Text;
            c.Address = tbCustomerAddress.Text;
            c.PhoneNumber = tbCustomerPhoneNumber.Text;

            string validateString = MainValidation.CustomerValidateString(c);
            if (validateString == String.Empty)
                c.InsertIntoDB();
            else
                MessageBox.Show(validateString);
        }

        private void InsertArea()
        {
            Area a = new Area();
            a.AreaId = Guid.NewGuid();
            a.CollectorId = tbAreaCollectorID.Text;
            a.Street = tbStreet.Text;

            string validateString = MainValidation.AreaValidateString(a);
            if (validateString == String.Empty)
                a.InsertIntoDB();
            else
                MessageBox.Show(validateString);
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

            string validateString = MainValidation.CounterValidateString(c);
            if (validateString == String.Empty)
                c.InsertIntoDB();
            else
                MessageBox.Show(validateString);
        }

        #endregion

        #region event handlery
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
        }

        #endregion

    }
}
