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
            {"textBoxCCID", "Pole \"Id\": Zły numer PESEL.\n"},
            {"textBoxName", "Pole \"Imię\": Wypełniono niepoprawnie.\n"},
            {"textBoxLastName", "Pole \"Nazwisko\": Wypełniono niepoprawnie.\n"},
            {"textBoxPostalCode", "Pole \"Kod pocztowy\": Wypełniono niepoprawnie.\n"},
            {"textBoxCity", "Pole \"Miasto\":Wypełniono niepoprawnie.\n"},
            {"textBoxAddress", "Pole \"Adres\": Należy uzupełnić pole.\n"},
            {"textBoxPhoneNumber", "Pole \"Numer kontaktowy\": Wypełniono niepoprawnie.\n"},
            {"textBoxAreaID", "Pole \"Id terenu\": Należy uzupełnić pole.\n"},
            {"textBoxStreet", "Pole \"Ulica\": Wypełniono niepoprawnie.\n"},
            {"textBoxCounterNo", "Pole \"Numer licznika\": Wypełniono niepoprawnie.\n"},
            {"textBoxCircuitNo", "Pole \"Numer układu\": Wypełniono niepoprawnie..\n"},
            {"textBoxCollectorID", "Nieprawidłowy numer inkasenta.\n"},
            {"textBoxCustomerID", "Nieprawidłowy numer klienta.\n"},
            {"textBoxAddressID", "Nieprawidłowy numer adresu.\n"},
            {"Fill in all fields", "Wypełnij poprawnie wszystkie pola"}
        };
    }
}
