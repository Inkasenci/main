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

        static public Dictionary<string, string> FaqQuestion = new Dictionary<string, string>()
        {
            {"dataBaseTables", "Zarządzanie bazą danych"},
            {"dataBaseTableAdress", "Adresy"},
            {"dataBaseTableArea", "Tereny"},
            {"dataBaseTableCollector", "Inkasenci"},
            {"dataBaseTableCounter", "Liczniki"},
            {"dataBaseTableCustomer", "Klienci"}
        };

        static public Dictionary<string, string> FaqAnswers = new Dictionary<string, string>()
        {
            {"dataBaseTables", "W przypadku problemów związanych z zarządzaniem danych, \n poniższe dzały odpowiedzą na pytania dotyczące każdej tabeli."},
            {"dataBaseTableAdress", "Zarządzanie adresami: \n Id adresu: generowany automatycznie \n Numer domu \n Numer mieszkania \n Id terenu: identyfikator terenu, \n inaczej mówiąc, ulicy, do której przypisany jest dany adres"},
            {"dataBaseTableArea", "Zarządzanie terenami: \n Id terenu: generowane automatycznie \n Ulica: nazwa ulicy należącej do terenu, \n zakładamy, że każdy teren jest oddzielną ulicą \n Id inkasenta: numer PESEL inkasenta, \n który zajmuje się sprawdzaniem stanu liczników na danym terenie"},
            {"dataBaseTableCollector", "Zarządzanie inkasentami: \n Id inkasenta: numer PESEL inkasenta \n Imię: imię inkasenta \n Nazwisko: nazwisko inkasenta \n Kod pocztowy: kod pocztowy z myślnikiem miejsca zamieszkania inkasenta \n Adres: ulica, na której mieszka inkasent \n Telefon kontaktowy: numer telefonu inkasenta, 9 cyfr"},
            {"dataBaseTableCounter", "Zarządzanie licznikami: \n Numer licznika: cyfrowy numer licznika \n Numer układu: fabryczny numer układu, złożony z cyfr \n Id adresu: identyfikator adresu, pod którym znajduje się dany licznik \n Id klienta: numer PESEL właściciela licznika"},
            {"dataBaseTableCustomer", "Zarządzanie klientami: \n Id klienta: numer PESEL klienta \n Imię: imię klienta \n Nazwisko: nazwisko klienta \n Kod pocztowy: kod pocztowy z myślnikiem miejsca zamieszkania klienta \n Adres: ulica, na której mieszka klient \n Telefon kontaktowy: numer telefonu klienta, 9 cyfr"}
        };

        static public Dictionary<string, string> FaqErrors = new Dictionary<string, string>()
        {
            {"indexOurOfRange","Nie istnieje pomoc dla wskazanego elementu!"}
        };
    }
}
