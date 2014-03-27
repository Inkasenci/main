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
            {"textBoxName", "Pole \"Imię\": Należy uzupełnić pole.\n"},
            {"textBoxLastName", "Pole \"Nazwisko\": Należy uzupełnić pole.\n"},
            {"textBoxPostalCode", "Pole \"Kod Pocztowy\": Należy uzupełnić pole.\n"},
            {"textBoxCity", "Pole \"Miasto\": Należy uzupełnić pole.\n"},
            {"textBoxAddress", "Pole \"Adres\": Należy uzupełnić pole.\n"},
            {"textBoxPhoneNumber", "Pole \"Telefon kontaktowy\": Należy uzupełnić pole.\n"},
            {"textBoxAreaID", "Pole \"Id Areny\": Należy uzupełnić pole.\n"},
            {"textBoxStreet", "Pole \"Ulica\": Należy uzupełnić pole.\n"},
            {"textBoxCounterNo", "Pole \"Numer Licznika\": Należy uzupełnić pole.\n"},
            {"textBoxCircuitNo", "Pole \"Numer Układu\": Należy uzupełnić pole.\n"},
            {"textBoxPostalCodeNumberAmount", "Pole \"Kod pocztowy\": Kod pocztowy powinien składać się z pięciu cyfr.\n"},
            {"textBoxPhoneNumberNumberAmount", "Pole \"Telefon kontaktowy\": Numer telefonu powinien składać się z dziewięciu cyfr.\n"}
        };
    }
}
