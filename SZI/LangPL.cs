using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    static class LangPL
    {
        static public Dictionary<string, string> InsertFormLang = new Dictionary<string,string>()
        {
            {"textBoxCCID", "Pole \"PESEL\": Zły numer PESEL.\n"},
            {"textBoxName", "Pole \"Imię\": Wypełniono niepoprawnie.\n"},
            {"textBoxLastName", "Pole \"Nazwisko\": Wypełniono niepoprawnie.\n"},
            {"textBoxPostalCode", "Pole \"Kod Pocztowy\": Wypełniono niepoprawnie.\n"},
            {"textBoxCity", "Pole \"Miasto\":Wypełniono niepoprawnie.\n"},
            {"textBoxAddress", "Pole \"Adres\": Należy uzupełnić pole.\n"},
            {"textBoxPhoneNumber", "Pole \"Numer Telefonu\": Wypełniono niepoprawnie.\n"},
            {"textBoxAreaID", "Pole \"Id Areny\": Należy uzupełnić pole.\n"},
            {"textBoxStreet", "Pole \"Ulica\": Wypełniono niepoprawnie.\n"},
            {"textBoxCounterNo", "Pole \"Numer Licznika\": Wypełniono niepoprawnie.\n"},
            {"textBoxCircuitNo", "Pole \"Numer Układu\": Wypełniono niepoprawnie..\n"},
            {"textBoxCollectorID", "Nieprawidłowy numer Inkasenta.\n"},
            {"textBoxCustomerID", "Nieprawidłowy numer Klienta.\n"},
            {"textBoxAddressID", "Nieprawidłowy numer Adresu.\n"}
        };
    }
}
