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

        static private bool IncorrectNumberAmount(string stringToValidate, int expectedNumberAmount)
        {
            return (stringToValidate.LongCount(str => Char.IsNumber(str)) != expectedNumberAmount) ? true : false;
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

            checkBug += (IncorrectNumberAmount(collector.PostalCode, 5)) ? LangPL.InsertFormLang["textBoxPostalCodeNumberAmount"] : String.Empty;
            checkBug += (IncorrectNumberAmount(collector.PhoneNumber, 9)) ? LangPL.InsertFormLang["textBoxPhoneNumberNumberAmount"] : String.Empty;

            return checkBug;
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

            checkBug += (IncorrectNumberAmount(customer.PostalCode, 5)) ? LangPL.InsertFormLang["textBoxPostalCodeNumberAmount"] : String.Empty;
            checkBug += (IncorrectNumberAmount(customer.PhoneNumber, 9)) ? LangPL.InsertFormLang["textBoxPhoneNumberNumberAmount"] : String.Empty;

            return checkBug;
        }
    }
}
