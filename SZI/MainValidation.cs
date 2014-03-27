using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    static class MainValidation
    {

        static private string UppercaseFirst(string s)
        {
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        static private bool CounterAndCircuitNumberValidation(int No)
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

        static private bool ContainsLetters(string S)
        {
            return S.Any(char.IsLetter);
        }

        static private bool CityNameStreetValidation(string Name)
        {
            if (Name.Length < 2 || ContainsNumbers(Name))
                return false;
            else
            {
                UppercaseFirst(Name);
                return true;
            }
                
        }
        static private bool PostalCodeAndPhoneValidation(string PostalCode, int Length)
        {
            if (PostalCode.Length != Length || ContainsLetters(PostalCode))
                return false;
            else
                return true;
        }

        static private bool CustomerExists(string ID)
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

        static private bool CollectorExists(string ID)
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

        static private bool EmptyString(string stringToValidate)
        {
            return (stringToValidate == String.Empty) ? true : false;
        }

        static private bool IncorrectNumberAmount(string stringToValidate, int expectedNumberAmount)
        {
            return (stringToValidate.LongCount(str => Char.IsNumber(str)) != expectedNumberAmount) ? true : false;
        }

        static public string CollectorValidateString(Collector collector)
        {
            string checkBug = String.Empty;

            if (!ContainsLetters(collector.CollectorId))
                checkBug += IdentityValidation.CheckId(collector.CollectorId) ? LangPL.InsertFormLang["textBoxCCID"] : String.Empty;
            else
                checkBug += LangPL.InsertFormLang["textBoxCCID"];
            checkBug += (EmptyString(collector.Address)) ? LangPL.InsertFormLang["textBoxAddress"] : String.Empty;
            checkBug += CityNameStreetValidation(collector.City) ? String.Empty : LangPL.InsertFormLang["textBoxCity"];
            checkBug += CityNameStreetValidation(collector.LastName) ? String.Empty : LangPL.InsertFormLang["textBoxLastName"];
            checkBug += CityNameStreetValidation(collector.Name) ? String.Empty : LangPL.InsertFormLang["textBoxName"];
            checkBug += PostalCodeAndPhoneValidation(collector.PhoneNumber, 9) ? String.Empty : LangPL.InsertFormLang["textBoxPhoneNumber"];
            checkBug += PostalCodeAndPhoneValidation(collector.PostalCode, 5) ? String.Empty : LangPL.InsertFormLang["textBoxPostalCode"];

            return checkBug;
        }

        static public string CustomerValidateString(Customer customer)
        {
            string checkBug = String.Empty;

            if (!ContainsLetters(customer.CustomerId))
                checkBug += (IdentityValidation.CheckId(customer.CustomerId)) ? LangPL.InsertFormLang["textBoxCCID"] : String.Empty;
            else
                checkBug += LangPL.InsertFormLang["textBoxCCID"];
            checkBug += (EmptyString(customer.Address)) ? LangPL.InsertFormLang["textBoxAddress"] : String.Empty;
            checkBug += CityNameStreetValidation(customer.City) ? String.Empty : LangPL.InsertFormLang["textBoxCity"];
            checkBug += CityNameStreetValidation(customer.LastName) ? String.Empty : LangPL.InsertFormLang["textBoxLastName"];
            checkBug += CityNameStreetValidation(customer.Name) ? String.Empty : LangPL.InsertFormLang["textBoxName"];
            checkBug += PostalCodeAndPhoneValidation(customer.PhoneNumber, 9) ? String.Empty : LangPL.InsertFormLang["textBoxPhoneNumber"];
            checkBug += PostalCodeAndPhoneValidation(customer.PostalCode, 5) ? String.Empty : LangPL.InsertFormLang["textBoxPostalCode"];
            
            return checkBug;
        }

        static public string AreaValidateString(Area a)
        {
            string checkBug = String.Empty;

            checkBug += CityNameStreetValidation(a.Street) ? String.Empty : LangPL.InsertFormLang["textBoxStreet"];
            checkBug += CollectorExists(a.CollectorId) ? String.Empty : LangPL.InsertFormLang["textBoxCollectorID"];

            return checkBug;
        }

        static public string CounterValidateString(Counter c)
        {
            string checkBug = String.Empty;

            checkBug += CounterAndCircuitNumberValidation(c.CounterNo) ? String.Empty : LangPL.InsertFormLang["textBoxCounterNo"];
            checkBug += CounterAndCircuitNumberValidation(c.CircuitNo) ? String.Empty : LangPL.InsertFormLang["textBoxCircuitNo"];
            checkBug += (EmptyString(c.AddressId.Value.ToString())) ? LangPL.InsertFormLang["textBoxAddress"] : String.Empty;
            //if (checkBug == String.Empty)
            //    checkBug += AddressExists(c.AddressId.Value) ? LangPL.InsertFormLang["textBoxAddressID"] : String.Empty;
            checkBug += CustomerExists(c.CustomerId) ? String.Empty : LangPL.InsertFormLang["textBoxCustomerID"];
               
            return checkBug;
        }
    }
}
