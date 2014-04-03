using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SZI
{
    static class MainValidation
    {
        static public bool OptionalChoice_ComboBox(string index) //z Comboboxa nie musi być wybrany żaden item
        {
            int i = Convert.ToInt32(index);
            if (i < 0)
                return false;
            else 
                return true;
        }
        static public bool MandatoryChoice_ComboBox(string index) //z Comboboxa musi zostać wybrany item
        {
            int i = Convert.ToInt32(index);
            if (i <= 0)
                return false;
            else
                return true;
        }

        static public bool OptionalCollector(string s)
        {
            if (EmptyString(s))
                return true;
            else
                return CollectorExists(s);
        }

        static public bool IDValidation(string ID)
        {
            Int64 Parse;
            if (!Int64.TryParse(ID, out Parse) || IdentityValidation.CheckId(ID))
                return false;
            else
                return true;
        }

        static public string UppercaseFirst(string s)
        {
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        static public bool CircuitAndCounterAndHouseAndFlatNumberValidation(string Number)
        {
            int Parse;

            if (Int32.TryParse(Number, out Parse))
                return CheckCircuitAndCounterAndHouseAndFlatNumber(Parse);
            else
                return false;
        }

        static private bool CheckCircuitAndCounterAndHouseAndFlatNumber(int No)
        {
            if (No > 0)
                return true;
            else 
                return false;
        }

        static private bool ContainsNumbers(string S)
        {
            return S.Any(char.IsDigit);
        }

        static public bool ContainsLetters(string S)
        {
            return S.Any(char.IsLetter);
        }

        static public bool CityNameValidation(string Name)
        {
            if (Name.Length < 2 || ContainsNumbers(Name))
                return false;
            else
                return true;                
        }

        static public bool StreetValidation(string Name)
        {
            if (Name.Length < 2)
                return false;
            else
                return true;  
        }
        static public bool PostalCodeValidation(string PostalCode)
        {
            Regex regex = new Regex(@"[0-9]{2}-[0-9]{3}");
            Match match = regex.Match(PostalCode);
            if (!match.Success)
                return false;
            else
                return true;
        }

        static public bool PhoneValidation(string Phone)
        {
            if (Phone.Length != 9 || ContainsLetters(Phone))
                return false;
            else
                return true;
        }

        static public bool CustomerExists(string ID)
        {
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                var Customers = from c in dataBase.Customers
                                select c.CustomerId;

                foreach (string customerID in Customers)
                    if (customerID == ID)
                        return true;
            }

            return false;
        }

        static public bool CollectorExists(string ID)
        {
            foreach (Collector c in Collectors.collectorList)
                if (c.CollectorId == ID)
                    return true;

            return false;
        }

        static private bool AddressExists(Guid ID)
        {
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                var Addresses = from a in dataBase.Addresses
                                select a.AddressId;

                foreach (Guid addressID in Addresses)
                    if (addressID == ID)
                        return true;
            }

            return false;
        }

        static public bool EmptyString(string stringToValidate)
        {
            return (stringToValidate == String.Empty) ? false : true;
        }

        static public string CollectorValidateString(Collector collector)
        {
            string checkBug = String.Empty;
            bool IsOK;

            if (!ContainsLetters(collector.CollectorId))
                checkBug += IdentityValidation.CheckId(collector.CollectorId) ? LangPL.InsertFormLang["textBoxCCID"] : String.Empty;
            else
                checkBug += LangPL.InsertFormLang["textBoxCCID"];
            

            checkBug += (IsOK = CityNameValidation(collector.Name)) ? String.Empty : LangPL.InsertFormLang["textBoxName"];
            if (IsOK)
                collector.Name = UppercaseFirst(collector.Name);

            checkBug += (IsOK = CityNameValidation(collector.LastName)) ? String.Empty : LangPL.InsertFormLang["textBoxLastName"];
            if (IsOK)
                collector.LastName = UppercaseFirst(collector.LastName);

            checkBug += PostalCodeValidation(collector.PostalCode) ? String.Empty : LangPL.InsertFormLang["textBoxPostalCode"];
            
            checkBug += (IsOK = CityNameValidation(collector.City)) ? String.Empty : LangPL.InsertFormLang["textBoxCity"];
            if (IsOK)
                collector.City = UppercaseFirst(collector.City);

            checkBug += (EmptyString(collector.Address)) ? LangPL.InsertFormLang["textBoxAddress"] : String.Empty;            
            checkBug += PhoneValidation(collector.PhoneNumber) ? String.Empty : LangPL.InsertFormLang["textBoxPhoneNumber"];
            
            return checkBug;
        }

        static public string CustomerValidateString(Customer customer)
        {
            string checkBug = String.Empty;
            bool IsOK;

            if (!ContainsLetters(customer.CustomerId))
                checkBug += (IdentityValidation.CheckId(customer.CustomerId)) ? LangPL.InsertFormLang["textBoxCCID"] : String.Empty;
            else
                checkBug += LangPL.InsertFormLang["textBoxCCID"];
            

            checkBug += (IsOK = CityNameValidation(customer.Name)) ? String.Empty : LangPL.InsertFormLang["textBoxName"];
            if (IsOK)
                customer.Name = UppercaseFirst(customer.Name);

            checkBug += (IsOK = CityNameValidation(customer.LastName)) ? String.Empty : LangPL.InsertFormLang["textBoxLastName"];
            if (IsOK)
                customer.LastName = UppercaseFirst(customer.LastName);

            checkBug += PostalCodeValidation(customer.PostalCode) ? String.Empty : LangPL.InsertFormLang["textBoxPostalCode"];

            checkBug += (IsOK = CityNameValidation(customer.City)) ? String.Empty : LangPL.InsertFormLang["textBoxCity"];
            if (IsOK)
                customer.City = UppercaseFirst(customer.City);

            checkBug += (EmptyString(customer.Address)) ?String.Empty : LangPL.InsertFormLang["textBoxAddress"];            
            checkBug += PhoneValidation(customer.PhoneNumber) ? String.Empty : LangPL.InsertFormLang["textBoxPhoneNumber"];            
            
            return checkBug;
        }

        static public string AreaValidateString(Area a)
        {
            string checkBug = String.Empty;
            bool IsOK;

            checkBug += (IsOK = StreetValidation(a.Street)) ? String.Empty : LangPL.InsertFormLang["textBoxStreet"];
            if (IsOK)
                a.Street = UppercaseFirst(a.Street);

            checkBug += CollectorExists(a.CollectorId) ? String.Empty : LangPL.InsertFormLang["textBoxCollectorID"];

            return checkBug;
        }

        static public string CounterValidateString(Counter c)
        {
            string checkBug = String.Empty;

            checkBug += CheckCircuitAndCounterAndHouseAndFlatNumber(c.CounterNo) ? String.Empty : LangPL.InsertFormLang["textBoxCounterNo"];
            checkBug += CheckCircuitAndCounterAndHouseAndFlatNumber(c.CircuitNo) ? String.Empty : LangPL.InsertFormLang["textBoxCircuitNo"];
            checkBug += (EmptyString(c.AddressId.Value.ToString())) ? String.Empty : LangPL.InsertFormLang["textBoxAddress"];
            //if (checkBug == String.Empty)
            //    checkBug += AddressExists(c.AddressId.Value) ? LangPL.InsertFormLang["textBoxAddressID"] : String.Empty;
            checkBug += CustomerExists(c.CustomerId) ? String.Empty : LangPL.InsertFormLang["textBoxCustomerID"];
               
            return checkBug;
        }
    }
}
