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
    public partial class ModifyForm : Form
    {
        private List<string> ids;
        private int selectedTab;
        private string[] labelsTexts;
        private string[] textBoxesNames;
        private string[] textBoxesTexts;
        private CollectorsManagementSystemEntities dataBase = new CollectorsManagementSystemEntities();

        private Dictionary<TextBox, ErrorProvider> TBtoEP_Dict;
        private Dictionary<string, ValidatingMethod> NameToMethod_Dict;
        private int InvalidFieldsCount = 0;

        public ModifyForm(List<string> ids, int selectedTab)
        {
            InitializeComponent();
            ErrorProvider ep;

            this.Text = "Modyfikacja rekordu";
            this.ids = ids;
            this.selectedTab = selectedTab;

            switch (selectedTab)
            {
                case 0:
                    labelsTexts = new string[] { "Id inkasenta: ", "Imię: ", "Nazwisko: ", "Kod pocztowy: ", "Miasto: ", "Ulica: ", "Telefon kontaktowy: " };
                    textBoxesNames = new string[] { "CollectorId", "Name", "LastName", "PostalCode", "City", "Address", "PhoneNumber" };
                    Collector modifiedCollector = dataBase.Collectors.SqlQuery("SELECT * FROM Collector WHERE CollectorId={0}", ids.ElementAt(0)).SingleOrDefault();
                    textBoxesTexts = new string[] { modifiedCollector.CollectorId, modifiedCollector.Name, modifiedCollector.LastName, modifiedCollector.PostalCode, modifiedCollector.City, modifiedCollector.Address, modifiedCollector.PhoneNumber };
                    break;
                case 1:
                    labelsTexts = new string[] { "Id klienta: ", "Imię: ", "Nazwisko: ", "Kod pocztowy: ", "Miasto: ", "Ulica: ", "Telefon kontaktowy: " };
                    textBoxesNames = new string[] { "CustomerId", "Name", "LastName", "PostalCode", "City", "Address", "PhoneNumber" };
                    Customer modifiedCustomer = dataBase.Customers.SqlQuery("SELECT * FROM Customer WHERE CustomerId={0}", ids.ElementAt(0)).SingleOrDefault();
                    textBoxesTexts = new string[] { modifiedCustomer.CustomerId, modifiedCustomer.Name, modifiedCustomer.LastName, modifiedCustomer.PostalCode, modifiedCustomer.City, modifiedCustomer.Address, modifiedCustomer.PhoneNumber };
                    break;
                case 2:
                    labelsTexts = new string[] { "Id terenu: ", "Ulica: ", "Id inkasenta: " };
                    textBoxesNames = new string[] { "AreaId", "Street", "CollectorId" };
                    Area modifiedArea = dataBase.Areas.SqlQuery("SELECT * FROM Area WHERE AreaId={0}", ids.ElementAt(0)).SingleOrDefault();
                    textBoxesTexts = new string[] { modifiedArea.AreaId.ToString(), modifiedArea.Street, modifiedArea.CollectorId };
                    break;
                case 3:
                    labelsTexts = new string[] { "Numer licznika: ", "Numer układu: ", "Id adresu: ", "Id klienta: " };
                    textBoxesNames = new string[] { "CounterNo", "CircuitNo", "AddressId", "CustomerId" };
                    Counter modifiedCounter = dataBase.Counters.SqlQuery("SELECT * FROM Counter WHERE CounterNo={0}", ids.ElementAt(0)).SingleOrDefault();
                    textBoxesTexts = new string[] { modifiedCounter.CounterNo.ToString(), modifiedCounter.CircuitNo.ToString(), modifiedCounter.AddressId.ToString(), modifiedCounter.CustomerId };
                    break;
            }

            NameToMethod_Dict = Auxiliary.Modify_CreateNameToMethodDict();
            Label[] labels = InitializeLabels();
            TextBox[] textBoxes = InitializeTextBoxes();
            TBtoEP_Dict = new Dictionary<TextBox, ErrorProvider>();

            for (int i = 0; i < labelsTexts.Length; i++)
            {
                this.Controls.Add(labels[i]);
                this.Controls.Add(textBoxes[i]);
                ep = Auxiliary.InitializeErrorProvider(textBoxes[i]);
                TBtoEP_Dict.Add(textBoxes[i], ep);
            }
        }

        private Label[] InitializeLabels()
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

        private TextBox[] InitializeTextBoxes()
        {
            TextBox[] textBoxes = new TextBox[textBoxesTexts.Length];

            for (int i = 0; i < textBoxesTexts.Length; i++)
            {                
                textBoxes[i] = new TextBox();                
                textBoxes[i].Name = textBoxesNames[i];
                textBoxes[i].Text = textBoxesTexts[i];
                textBoxes[i].Location = new Point(150, 30 * (i + 1));
                textBoxes[i].Validating += Validation;
            }
            textBoxes[0].Enabled = false;
            return textBoxes;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            string validateString;
            
            switch (selectedTab)
            {
                case 0:
                    Collector modifiedCollector = new Collector();
                    modifiedCollector.CollectorId = this.Controls.Find("CollectorId", true)[0].Text;
                    modifiedCollector.Name = this.Controls.Find("Name", true)[0].Text;
                    modifiedCollector.LastName = this.Controls.Find("LastName", true)[0].Text;
                    modifiedCollector.PostalCode = this.Controls.Find("PostalCode", true)[0].Text;
                    modifiedCollector.City = this.Controls.Find("City", true)[0].Text;
                    modifiedCollector.Address = this.Controls.Find("Address", true)[0].Text;
                    modifiedCollector.PhoneNumber = this.Controls.Find("PhoneNumber", true)[0].Text;

                    if (InvalidFieldsCount <= 0)
                    {
                        modifiedCollector.ModifyRecord(ids.ElementAt(0));
                        this.Close();
                    }
                    else
                        MessageBox.Show(LangPL.InsertFormLang["Fill in all fields"]);                    
                    /*
                    validateString = MainValidation.CollectorValidateString(modifiedCollector);
                    if (validateString == String.Empty)
                    {
                        modifiedCollector.ModifyRecord(ids.ElementAt(0));
                        this.Close();
                    }
                    else
                        MessageBox.Show(validateString);
                     */
                    break;

                case 1:
                    Customer modifiedCustomer = new Customer();
                    modifiedCustomer.CustomerId = this.Controls.Find("CustomerId", true)[0].Text;
                    modifiedCustomer.Name = this.Controls.Find("Name", true)[0].Text;
                    modifiedCustomer.LastName = this.Controls.Find("LastName", true)[0].Text;
                    modifiedCustomer.PostalCode = this.Controls.Find("PostalCode", true)[0].Text;
                    modifiedCustomer.City = this.Controls.Find("City", true)[0].Text;
                    modifiedCustomer.Address = this.Controls.Find("Address", true)[0].Text;
                    modifiedCustomer.PhoneNumber = this.Controls.Find("PhoneNumber", true)[0].Text;

                    if (InvalidFieldsCount == 0)
                    {
                        modifiedCustomer.ModifyRecord(ids.ElementAt(0));
                        this.Close();
                    }
                    else
                        MessageBox.Show(LangPL.InsertFormLang["Fill in all fields"]);  
                    /*
                    validateString = MainValidation.CustomerValidateString(modifiedCustomer);
                    if (validateString == String.Empty)
                    {
                        modifiedCustomer.ModifyRecord(ids.ElementAt(0));
                        this.Close();
                    }
                    else
                        MessageBox.Show(validateString);
                     */
                    break;

                case 2:
                    Area modifiedArea = new Area();
                    modifiedArea.AreaId = new Guid(this.Controls.Find("AreaId", true)[0].Text);
                    modifiedArea.Street = this.Controls.Find("Street", true)[0].Text;
                    modifiedArea.CollectorId = this.Controls.Find("CollectorId", true)[0].Text;

                    validateString = MainValidation.AreaValidateString(modifiedArea);
                    if (validateString == String.Empty || InvalidFieldsCount == 0)
                    {
                        modifiedArea.ModifyRecord(ids.ElementAt(0));
                        this.Close();
                    }
                    else
                        MessageBox.Show(validateString);
                    break;

                case 3:
                    Counter modifiedCounter = new Counter();
                    modifiedCounter.CounterNo = Convert.ToInt32(this.Controls.Find("CounterId", true)[0].Text);
                    modifiedCounter.CircuitNo = Convert.ToInt32(this.Controls.Find("Street", true)[0].Text);
                    modifiedCounter.AddressId = new Guid(this.Controls.Find("AddressId", true)[0].Text);
                    modifiedCounter.CustomerId = this.Controls.Find("CustomerId", true)[0].Text;

                    if (InvalidFieldsCount == 0)
                    {
                        modifiedCounter.ModifyRecord(ids.ElementAt(0));
                        this.Close();
                    }
                    else
                        MessageBox.Show(LangPL.InsertFormLang["Fill in all fields"]);  
                    break;
            }
        }

        private void Validation(object sender, CancelEventArgs e)
        {
            TextBox ValidatedTextBox = (TextBox)sender;
            
            if (NameToMethod_Dict[ValidatedTextBox.Name](ValidatedTextBox.Text))
            {
                TBtoEP_Dict[ValidatedTextBox].SetError(ValidatedTextBox, String.Empty);
                InvalidFieldsCount--;
            }
            else
            {
                TBtoEP_Dict[ValidatedTextBox].SetError(ValidatedTextBox, "Nieprawidłowo wypełnione pole.");
                InvalidFieldsCount++;
                //e.Cancel = true;
            }
        }
    }
}