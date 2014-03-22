using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    static class MainValidation
    {
        static private bool EmptyString(string stringToValidate)
        {
            return (stringToValidate == String.Empty) ? true : false;
        }

        static public bool CollectorValidate(Collector collector)
        {
            int checkBug = 0;

            checkBug += (IdentityValidation.CheckId(collector.CollectorId)) ? 1 : 0;
            checkBug += (EmptyString(collector.Address)) ? 1 : 0;
            checkBug += (EmptyString(collector.City)) ? 1 : 0;
            checkBug += (EmptyString(collector.LastName)) ? 1 : 0;
            checkBug += (EmptyString(collector.Name)) ? 1 : 0;
            checkBug += (EmptyString(collector.PhoneNumber)) ? 1 : 0;
            checkBug += (EmptyString(collector.PostalCode)) ? 1 : 0;

            return (checkBug == 0) ? true : false; 
        }

        static public string CollectorValidateString(Collector collector)
        {
            string checkBug = String.Empty;

            checkBug += (IdentityValidation.CheckId(collector.CollectorId)) ? LangPL.InsertFormLang["textBoxCCID"] : String.Empty;
            checkBug += (EmptyString(collector.Address)) ? LangPL.InsertFormLang["textBoxAddress"] : String.Empty;
            checkBug += (EmptyString(collector.City)) ? LangPL.InsertFormLang["textBoxCity"] : String.Empty;
            checkBug += (EmptyString(collector.LastName)) ? LangPL.InsertFormLang["textBoxLastName"] : String.Empty;
            checkBug += (EmptyString(collector.Name)) ? LangPL.InsertFormLang["textBoxName"] : String.Empty;
            checkBug += (EmptyString(collector.PhoneNumber)) ? LangPL.InsertFormLang["textBoxPhoneNumber"] : String.Empty;
            checkBug += (EmptyString(collector.PostalCode)) ? LangPL.InsertFormLang["textBoxPostalCode"] : String.Empty;

            return checkBug;
        }

        static public bool CustomerValidate(Customer customer)
        {
            int checkBug = 0;

            checkBug += (IdentityValidation.CheckId(customer.CustomerId)) ? 1 : 0;
            checkBug += (EmptyString(customer.Address)) ? 1 : 0;
            checkBug += (EmptyString(customer.City)) ? 1 : 0;
            checkBug += (EmptyString(customer.LastName)) ? 1 : 0;
            checkBug += (EmptyString(customer.Name)) ? 1 : 0;
            checkBug += (EmptyString(customer.PhoneNumber)) ? 1 : 0;
            checkBug += (EmptyString(customer.PostalCode)) ? 1 : 0;

            return (checkBug == 0) ? true : false;
        }

        static public string CustomerValidateString(Customer customer)
        {
            string checkBug = String.Empty;

            checkBug += (IdentityValidation.CheckId(customer.CustomerId)) ? LangPL.InsertFormLang["textBoxCCID"] : String.Empty;
            checkBug += (EmptyString(customer.Address)) ? LangPL.InsertFormLang["textBoxAddress"] : String.Empty;
            checkBug += (EmptyString(customer.City)) ? LangPL.InsertFormLang["textBoxCity"] : String.Empty;
            checkBug += (EmptyString(customer.LastName)) ? LangPL.InsertFormLang["textBoxLastName"] : String.Empty;
            checkBug += (EmptyString(customer.Name)) ? LangPL.InsertFormLang["textBoxName"] : String.Empty;
            checkBug += (EmptyString(customer.PhoneNumber)) ? LangPL.InsertFormLang["textBoxPhoneNumber"] : String.Empty;
            checkBug += (EmptyString(customer.PostalCode)) ? LangPL.InsertFormLang["textBoxPostalCode"] : String.Empty;

            return checkBug;
        }
    }
}
