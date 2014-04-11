using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    static class LangPL
    {
        static public Dictionary<string, string> InsertFormLang = new Dictionary<string, string>()
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
            {"aboutHelp","O tym dokumencie."},
            {"dataBaseTables", "Zarządzanie bazą danych."},
            {"dataBaseTableAdress", "Nie mogę dodać nowego adresu."},
            {"dataBaseTableArea", "Nie mogę dodać nowego terenu."},
            {"dataBaseTableCollector", "Nie mogę dodać nowego inkasenta."},
            {"dataBaseTableCounter", "Nie mogę dodać nowego licznika."},
            {"dataBaseTableCustomer", "Nie mogę dodać nowego klienta."}
        };

        static public Dictionary<string, string> FaqAnswers = new Dictionary<string, string>()
        {
            {"aboutHelp","Dokument ten ma służyć pomocą w podstawowych kwestiach dotyczących obsługi aplikacji pozwalającej zarządzać danymi dostarczanymi przez inkasentów jak i również nimi samymi. \r\n\r\n Autoram aplikacji SZI ( System Zarządzania Inkasentami ) są: \r\n Patryk Zawadzki [ 254155 ], \r\n Rafał Burzyński [ 254068 ], \r\n Marcin Nowak [ 254118 ]"},
            {"dataBaseTables", "W przypadku problemów związanych z zarządzaniem danymi w bazie, sekcja ta powinna pozwolić na rozwiązanie pojawiających się problemów, wyjaśniając krok po kroku powstałe błędy. \r\n\r\n Możemy napotkać się z dwoma rodzajami błędów, pierwszy dotyczy pojedynczego pola występujące podczas uzupełniania, informujący o błędnie wprowadzonych danych ( Rys. 1 ) HelpImg/errorIcon.png Drugim typem jest informacja zwrotna uzyskana podczas wysyłania formularza oznajmująca błędnie wypełniony formularz ( Rys. 2 ) HelpImg/errorMessage.png"},
            {"dataBaseTableAdress", "W przypadku problemu związanego z dodaniem nowego adresu należy podać odpowiednio: \r\n Numer domu - postać numeryczna \r\n Numer mieszkania - postać numeryczna \r\n Id terenu: wybierany spośród dostępnych terenów - wpisanych w bazie ulic, \r\n Przykład poprawnie uzupełnionego formularza ( Rys. 1 ) HelpImg/trueFormAdress.png"},
            {"dataBaseTableArea", "W przypadku problemu związanego z dodaniem nowego terenu: \r\n Ulica - nazwa ulicy należącej do danego terenu, forma tekstowa, zakładamy, że każdy teren jest oddzielną ulicą \r\n Id inkasenta: jeden z inkasentów wybranych spośród dostępnych, który zajmuje się sprawdzaniem stanu liczników na danym terenie \r\n Przykład poprawnie uzupełnionego formularza ( Rys. 1 ) HelpImg/trueFormArea.png"},
            {"dataBaseTableCollector", "W przypadku problemu związanego z dodaniem nowego inkasenta: \r\n Id inkasenta: numer PESEL inkasenta ( 11 cyfr ) \r\n Imię: imię inkasenta ( minimum 2 litery ) \r\n Nazwisko: nazwisko inkasenta ( minimum 2 litery ) \r\n Kod pocztowy: kod pocztowy w formacie XX-XXX \r\n Adres: ulica i nr lokalu w którym mieszka inkasent \r\n Telefon kontaktowy: numer telefonu inkasenta, 9 cyfr \r\n Przykład poprawnie uzupełnionego formularza ( Rys. 1 ) HelpImg/trueFormCollector.png"},
            {"dataBaseTableCounter", "W przypadku problemu związanego z dodaniem nowego licznika: \r\n Numer licznika: cyfrowy numer licznika \r\n Numer układu: fabryczny numer układu złożony z cyfr \r\n Id adresu: adres, pod którym znajduje się dany licznik \r\n Id klienta: wybierany właściciel licznika \r\n Przykład poprawnie uzupełnionego formularza ( Rys. 1 ) HelpImg/trueFormCounter.png"},
            {"dataBaseTableCustomer", "W przypadku problemu związanego z dodaniem nowego klienta: \r\n Id klienta: numer PESEL klienta ( 11 cyfr ) \r\n Imię: imię klienta ( minimum 2 litery ) \r\n Nazwisko: nazwisko klienta ( minimum 2 litery ) \r\n Kod pocztowy: kod pocztowy w formacie XX-XXX \r\n Adres: ulica i nr lokalu w którym mieszka inkasent \r\n Telefon kontaktowy: numer telefonu inkasenta, 9 cyfr \r\n Przykład poprawnie uzupełnionego formularza ( Rys. 1 ) HelpImg/trueFormCollector.png"}
        };

        static public Dictionary<string, string> FaqErrors = new Dictionary<string, string>()
        {
            {"indexOutOfRange","Nie istnieje pomoc dla wskazanego elementu!"},
            {"loadHelp","Problem z ładowanie danych!"}
        };

        static public Dictionary<string, string> IntegrityWarnings = new Dictionary<string, string>()
        {
            { "collectorRemoval", "Jeden lub więcej wybranych inkasentów ma przydzielony teren. Czy chcesz kontynuować? "},
            { "customerRemoval", "Jeden lub więcej wybranych klientów jest właścicielem licznika. Czy chcesz kontynuować? "},
            { "areaRemoval", "Do jednego lub więcej wybranych terenów należą adresy liczników. Czy chcesz kontynuować? "},
            { "addressRemoval", "Pod jednym lub więcej adresów zamontowane są liczniki. Czy chcesz kontynuować? "}
        };

        static public Dictionary<string, string> CountersWarnings = new Dictionary<string, string>()
        {
            { "noRecord", "----"},
            { "wrongFileName", "Błąd! Niepoprawna nazwa pliku!" },
            { "xmlError", "Błąd podczas przetwarzania piku XML!" }
        };
    }
}
