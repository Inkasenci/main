using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    /// <summary>
    /// Klasa obslugująca język w aplikacji ( błędy, treści, nazwy ).
    /// </summary>
    static class LangPL
    {
        /// <summary>
        /// Treści używane podczas dodawania rekordów do bazy danych - błędy / walidacja.
        /// </summary>
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

        /// <summary>
        /// Treści używane podczas generowania pomocy ( HELP ) programu - [ytania.
        /// </summary>
        static public Dictionary<string, string> FaqQuestion = new Dictionary<string, string>()
        {
            {"aboutHelp","O tym dokumencie."},
            {"dataBaseTables", "Zarządzanie bazą danych."},
            {"dataBaseRecordAddition", "Dodawanie rekordów. "},
            {"dataBaseTableAddress", "Dlaczego nie mogę dodać nowego adresu?"},
            {"dataBaseTableArea", "Dlaczego nie mogę dodać nowego terenu?"},
            {"dataBaseTableCollector", "Dlaczego nie mogę dodać nowego inkasenta?"},
            {"dataBaseTableCounter", "Dlaczego nie mogę dodać nowego licznika?"},
            {"dataBaseTableCustomer", "Dlaczego nie mogę dodać nowego klienta?"},
            {"dataBaseRecordRemoval", "Usuwanie rekordów. "},
            {"dataBaseTableAddressRemoval", "Czym grozi usunięcie adresu? "},
            {"dataBaseTableAreaRemoval", "Czym grozi usunięcie terenu? "},
            {"dataBaseTableCollectorRemoval", "Czym grozi usunięcie inkasenta? "},
            {"dataBaseTableCounterRemoval", "Czym grozi usunięcie licznika? "},
            {"dataBaseTableCustomerRemoval", "Czym grozi usunięcie klienta? "},
            {"dataBaseComboBox", "Rozwijane listy. "},
            {"dataBaseComboBoxUse", "Jak korzystać z rozwijanych list? "},
            {"dataBaseComboBoxFindItem", "Jak znaleźć właściwy rekord na rozwijanej liście? "},
            {"dataBaseReading", "Zarządzanie odczytami inkasentów."},
            {"dataBaseReadingEmptyDataBase", "Dlaczego wyświetla mi się pusta lista?"},
            {"dataBaseReadingZeroReading", "Brak odczytów do wykonania, o co chodzi?"},
            {"dataBaseReadingExport", "Jak przekazać Inkasentowi informację o odczytach jakie ma wykonać?"},
            {"dataBaseReadingImport", "Jak dodać wykonane odczyty do bazy danych?"},
            {"dataBaseReadingImportError", "Dlaczego odczyty nie zostały dodane do bazy danych?"},
            {"dataBaseXML", "XML w odniesieniu do SZI."},
            {"dataBaseXMLFormat", "Co oznaczają poszczególne pola pliku XML?"},
            {"dataBaseXMLEmptyElement", "Puste pole w pliku XML?"},
            {"dataBaseXMLPriority", "Zmiany niektórych pól nie wpływają na właściwości zapisu?"},
            {"XMLTextEditor", "Edytor plików XML by SZI."},
            {"XMLTextEditorReadOnly", "Dlaczego nie mogę edytować niektórych pól?"},
            {"XMLTextEditorNextPrevError", "Klikam \"Następny\"/\"Poprzedni\" i nic się nie dzieje?"},
            {"XMLTextEditorImportError", "Po otworzeniu pliku, nic się nie wyświetliło?"},
            {"XMLTextEditorSaveError", "Dlaczego po kliknięciu \"Następny\"/\"Poprzedni\" tracę wcześniej wpisane dane?"}
        };

        /// <summary>
        /// Treści używane podczas generowania pomocy ( HELP ) programu - odpowiedzi.
        /// </summary>
        static public Dictionary<string, string> FaqAnswers = new Dictionary<string, string>()
        {
            {"aboutHelp","Dokument ten ma służyć pomocą w podstawowych kwestiach dotyczących obsługi aplikacji pozwalającej zarządzać danymi dostarczanymi przez inkasentów jak i również nimi samymi. \r\n\r\n Autoram aplikacji SZI ( System Zarządzania Inkasentami ) są: \r\n Patryk Zawadzki [ 254155 ], \r\n Rafał Burzyński [ 254068 ], \r\n Marcin Nowak [ 254118 ]"},
            {"dataBaseTables", "W przypadku problemów związanych z zarządzaniem danymi w bazie, sekcja ta powinna pozwolić na rozwiązanie pojawiających się trudności, wyjaśniając krok po kroku powstałe błędy. "},
            {"dataBaseRecordAddition", "Możemy napotkać się z dwoma rodzajami błędów, pierwszy dotyczy pojedynczego pola występujące podczas uzupełniania, informujący o błędnie wprowadzonych danych ( Rys. 1 ) HelpImg/errorIcon.png Drugim typem jest informacja zwrotna uzyskana podczas wysyłania formularza oznajmująca błędnie wypełniony formularz ( Rys. 2 ) HelpImg/errorMessage.png"},
            {"dataBaseTableAddress", "W przypadku problemu związanego z dodaniem nowego adresu należy podać odpowiednio: \r\n Numer domu - postać numeryczna \r\n Numer mieszkania - postać numeryczna \r\n Id terenu: wybierany spośród dostępnych terenów - wpisanych w bazie ulic, \r\n Przykład poprawnie uzupełnionego formularza ( Rys. 1 ) HelpImg/trueFormAdress.png"},
            {"dataBaseTableArea", "W przypadku problemu związanego z dodaniem nowego terenu: \r\n Ulica - nazwa ulicy należącej do danego terenu, forma tekstowa, zakładamy, że każdy teren jest oddzielną ulicą \r\n Id inkasenta: jeden z inkasentów wybranych spośród dostępnych, który zajmuje się sprawdzaniem stanu liczników na danym terenie \r\n Przykład poprawnie uzupełnionego formularza ( Rys. 1 ) HelpImg/trueFormArea.png"},
            {"dataBaseTableCollector", "W przypadku problemu związanego z dodaniem nowego inkasenta: \r\n Id inkasenta: numer PESEL inkasenta ( 11 cyfr ) \r\n Imię: imię inkasenta ( minimum 2 litery ) \r\n Nazwisko: nazwisko inkasenta ( minimum 2 litery ) \r\n Kod pocztowy: kod pocztowy w formacie XX-XXX \r\n Adres: ulica i nr lokalu w którym mieszka inkasent \r\n Telefon kontaktowy: numer telefonu inkasenta, 9 cyfr \r\n Przykład poprawnie uzupełnionego formularza ( Rys. 1 ) HelpImg/trueFormCollector.png"},
            {"dataBaseTableCounter", "W przypadku problemu związanego z dodaniem nowego licznika: \r\n Numer licznika: cyfrowy numer licznika \r\n Numer układu: fabryczny numer układu złożony z cyfr \r\n Id adresu: adres, pod którym znajduje się dany licznik \r\n Id klienta: wybierany właściciel licznika \r\n Przykład poprawnie uzupełnionego formularza ( Rys. 1 ) HelpImg/trueFormCounter.png"},
            {"dataBaseTableCustomer", "W przypadku problemu związanego z dodaniem nowego klienta: \r\n Id klienta: numer PESEL klienta ( 11 cyfr ) \r\n Imię: imię klienta ( minimum 2 litery ) \r\n Nazwisko: nazwisko klienta ( minimum 2 litery ) \r\n Kod pocztowy: kod pocztowy w formacie XX-XXX \r\n Adres: ulica i nr lokalu w którym mieszka klient \r\n Telefon kontaktowy: numer telefonu klienta, 9 cyfr \r\n Przykład poprawnie uzupełnionego formularza ( Rys. 1 ) HelpImg/trueFormCustomer.png"},
            {"dataBaseRecordRemoval", "Przy próbie usunięcia rekordu z tabeli na ekranie może pojawić się ostrzeżenie (rys. 1). Oznacza to, że klucz główny usuwanego rekordu jest kluczem obcym w innej tabeli (np. usuwany inkasent ma przypisany teren). Okno z ostrzeżeniem pozwala nam potwierdzić usunięcie lub wycofać się z niego. \r\n HelpImg/collectorRemovalWarning.png"},
            {"dataBaseTableAddressRemoval", "Jeśli podczas próby usunięcia adresów, wyświetli się okno z ostrzeżeniem, umożliwiające potwierdzenie lub wycofanie się z usunięcia, oznacza to, że pod conajmniej jednym z usuwanych adresów znajdują się liczniki. Potwierdzenie usunięcia spowoduje wyczyszczenie pól \"Id klienta\" i \"Id adresu\" dla liczników znajdujących się pod usuwanym adresem. "},
            {"dataBaseTableAreaRemoval", "Jeśli podczas próby usunięcia terenów wyświetli się okno z ostrzeżeniem, umożliwiające potwierdzenie lub wycofanie się z usunięcia, oznacza to, że na co najmniej jednym z usuwanych terenów istnieją adresy. Potwierdzenie usunięcia spowoduje skasowanie terenu i adresów na nim się znajdujących. "},
            {"dataBaseTableCollectorRemoval", "Jeśli podczas próby usunięcia inkasentów wyświetli się okno z ostrzeżeniem, umożliwiające potwierdzenie lub wycofanie się z usunięcia, oznacza to, że co najmniej jeden z usuwanych inkasentów ma przypisany teren lub dokonał jakichś odczytów. Potwierdzenie usunięcia spowoduje wyczyszczenie pola \"Id inkasenta\" dla terenów przypisanych do usuwanego inkasenta i skasowanie odczytów, których dokonał. "},
            {"dataBaseTableCounterRemoval", "Jeśli podczas próby usunięcia liczników wyświetli się okno z ostrzeżeniem, umożliwiające potwierdzenie lub wycofanie się z usunięcia, oznacza to, że co najmniej jeden z usuwanych liczników posiada odczyty. Potwierdzenie usunięcia spowoduje skasowanie odczytów dokonanych na usuwanym liczniku. "},
            {"dataBaseTableCustomerRemoval", "Jeśli podczas próby usunięcia klientów wyświetli się okno z ostrzeżeniem, umożliwiające potwierdzenie lub wycofanie się z usunięcia, oznacza to, że co najmniej jeden z usuwanych klientów jest właścicielem licznika. Potwierdzenie usunięcia spowoduje wyczyszczenie pól \"Id klienta\" i \"Id adresu\" dla liczników należących do usuwanego klienta. "},
            {"dataBaseComboBox", "Pola niektórych tabel są zasilane danymi z innych tabel. Zawartość tych pól jest wybierana przez użytkownika za pomocą rozwijanych list (rys. 1), które zawierają wszystkie rekordy z obcej tabeli. HelpImg/comboBox.png"},
            {"dataBaseComboBoxUse", "Aby móc przeglądać zawartość rozwijanej listy (rys. 1), należy ją rozwinąć poprzez kliknięcie. Po zdecydowaniu, który rekord jest właściwy w danym przypadku, należy go kliknąć, aby został wybrany. Lista zostanie zwinięta i zacznie wyświetlać skrócone informacje o wybranym rekordzie. \r\n W niektórych przypadkach wskazane jest wybranie pustego rekordu, jest on umieszczony na pierwszym miejscu w rozwijanych listach. \r\n Jeśli chcemy wycofać się z możliwości wyboru rekordu rozwijanej listy, należy kliknąć miejsce poza nią. HelpImg/comboBoxUse.png"},
            {"dataBaseComboBoxFindItem", "Niektóre rozwijane listy mogą zawierać dużą ilość rekordów, co utrudnia dokonanie wyboru. Kiedy lista jest rozwinięta (rys. 1), możemy filtrować jej zawartość. Wystarczy zacząć pisać poszukiwany tekst. \r\n Jeśli chcemy usunąć ostatni element filtra, należy nacisnąć klawisz Backspace. \r\n Filtr jest nieczuły na wielkość liter i polskie znaki. \r\n Z listy przefiltrowanej rekordy wybiera się w ten sam sposób co ze zwykłej listy. \r\n Aby zrozumieć, jak działa filtrowanie list, należy wyobrazić sobie, że każdy rekord jest pojedynczym napisem, a pola rekordu są oddzielone spacjami. HelpImg/comboBoxFindItem.png"},
            {"dataBaseReading", "W przypadku problemów związanych z zarządzaniem odczytami, sekcja ta powinna pozwolić na rozwiązanie pojawiających się trudności, wyjaśniając krok po kroku proces naprawczy."},
            {"dataBaseReadingEmptyDataBase", "Jeżeli wchodząc w okienko Zarządzania Odczytami widzimy pustą liste ( Rys. 1 ). HelpImg/emptyCounterForm.png należy sprawdzić czy w naszej firmie są zatrudnieni jacykolwiek inkasenci. \r\n Aby tego dokonać należy wybrać \"Zarządzanie bazą danych\" w Menu głównym, Zakładka \"Inkasenci\". "},
            {"dataBaseReadingZeroReading", "W przypadku gdy obserwujemy obok inkasenta wartość równą 0 w kolumnie \"Liczba odczytów\" ( Rys 1. ) HelpImg/zeroCounterForm.png możemy spodziewać się następujących sytuacji: \r\n Danemu inkasentowu nie zostały przypisane żadne Tereny ( Liczniki ) w bazie, może to się wiązać np. z nowo zatrudniony pracownikiem. \r\n Inkasent wykonał już wszystkie powierzone mu na dany okres czasu odczyty, tzn w przeciagu ostatnich 30 dni odwiedził wszystkie przypisane mu Tereny."},
            {"dataBaseReadingExport", "Gdy chcemy uzyskać informację na temat odczytów danego Inkasenta, należy zaznaczając go kliknąć przycisk \"Sprawdź odczyty\". W tym miejscu znajdują się informacje na temat wszystkich odczytów jakie musi wykonać dany Inkasent na dany moment. \r\n W tym miejscu również mamy dostęp do exportu danych do pliku XML - format przenośny dla jednostek pracujących."},
            {"dataBaseReadingImport", "Informację o wykonanych odczytach uzyskujemy w formacie pliku XML - uzyskanego od Inkasenta. Aby dodać je do naszej bazy danych należy klikajac na przycisk \"Import\" w oknie \"Zarządzania odczytami\" i wybrać plik."},
            {"dataBaseReadingImportError", "Powodem nie dodania odczytów do bazy danych, może być błędy format pliku XML - np powstały podczas ręcznej edycji pliku. Więcej na temat formatu generowanego pliku XML szukaj w dziale \"XML\"."},
            {"dataBaseXML", "W przypadku problemów związanych z plikami XML, sekcja ta powinna pozwolić na rozwiązanie pojawiających się trudności, wyjaśniając krok po kroku proces naprawczy."},
            {"dataBaseXMLFormat", "Format pliku XML wygląda następująco ( Rys. 1 ) HelpImg/xmlBySZIFileFormat.png Pola kolejno określają: \r\n CollectorID - określa numer identyfikujący inkasenta wykonującego dany odczyt, \r\n ReadId - numer porządkowy danego rekordu, \r\n CounterNo - numer licznika, \r\n CircuitNo - numer układu, \r\n Customer - klient / właściciel licznika, \r\n Address - adres pod którym znajduje się licznik, \r\n LastReadDate - data ostatniego odczytu, \r\n LastCollector - inkasent który wykonywał ostatnio odczyt, \r\n LastValue - wartość ostatniego odczytu, \r\n NewValue - wartość odczytu, który inasent musi dostarczyć"},
            {"dataBaseXMLEmptyElement", "Jeżeli obserwujesz puste pole w pliku XML ( Rys. 1 ) HelpImg/xmlBySZIFileFormat.png Nie należy się tym przejmować. Jest to format pozwalający zaoszczędzić na rozmiarze pliku. W momencie którym chcesz wprowadzić zmianę w owym polu, przekształcasz ( na przykładzie NewValue ) <NewValue/> >> <NewValue>Wartość odczytu</NewValue>."},
            {"dataBaseXMLPriority", "Niektóre pola zawarte w pliku XML są jedynie dla informacji - ułatwienia inkasenta, natomiast część z nich ma znaczący wpływ na zapis informacji, tak kolejno do pól uczestniczących w zapisie należą: \r\n CollectorId, \r\n CounterNo, \r\n NewValue. \r\n Reszta pól służy jedynie celom informacyjnym."},
            {"XMLTextEditor", "W przypadku problemów związanych z Edytorem plików XML, sekcja ta powinna pozwolić na rozwiązanie pojawiających się trudności, wyjaśniając krok po kroku proces naprawczy."},
            {"XMLTextEditorReadOnly", "Powodem dla którego niektóre pola są zablokowane ( Rys. 1 ) HelpImg/xmlTextEditorReadOnly.png jest brak konieczności ich edycji - oraz brak wpływu na zwracany rezultat. Jest to normalne i pozwala zabezpieczeć inasenta przed ewentuanymi przypadkowymi zmianami w innych polach."},
            {"XMLTextEditorNextPrevError", "Prawdopodobnie przynany został inkasentowy do wykonania tylko jeden odczyt ( zarówno poprzednim, jak i następnym jest rekord o numerze 1 )."},
            {"XMLTextEditorImportError", "W przypadku problemów z wczytaniem danych, sprawdź czy format pliku jest na pewno odpowiedni ( pliku exportowany przez SZI jest na pewno poprawny ). \r\n\r\n Drugim elementem który może mieć wpływ jest zerowa ilość rekordów w danym pliku. "},
            {"XMLTextEditorSaveError", "Sprawdź czy na pewno został wciśnięty przycisk \"Zapisz zmiany\" znadujący się w dolnej części okna ( Rys. 1 ) HelpImg/xmlTextEditorSaveError.png"}
        };

        /// <summary>
        /// Treści używane podczas generowania pomocy ( HELP ) programu - błędy.
        /// </summary>
        static public Dictionary<string, string> FaqErrors = new Dictionary<string, string>()
        {
            {"indexOutOfRange","Nie istnieje pomoc dla wskazanego elementu!"},
            {"loadHelp","Problem z ładowanie danych!"}
        };

        /// <summary>
        /// Treści używane podczas działań na polach posiadających odniesienie do innych tabel - ostrzeżenia.
        /// </summary>
        static public Dictionary<string, string> IntegrityWarnings = new Dictionary<string, string>()
        {
            { "collectorRemoval", "Jeden lub więcej wybranych inkasentów ma przydzielony teren lub dokonał odczytu. Czy chcesz kontynuować? "},
            { "customerRemoval", "Jeden lub więcej wybranych klientów jest właścicielem licznika. Czy chcesz kontynuować? "},
            { "areaRemoval", "Do jednego lub więcej wybranych terenów należą ADRESY LICZNIKÓW, które również ZOSTANĄ SKASOWANE. Czy chcesz kontynuować? " },
            { "addressRemoval", "Pod jednym lub więcej adresów zamontowane są liczniki. Czy chcesz kontynuować? "},
            { "counterRemoval", "Jeden lub więcej liczników posiada ODCZYTY, które również ZOSTANĄ SKASOWANE. Czy chcesz kontynuować? "}
        };

        /// <summary>
        /// Treści używane podczas obsługi odczytów - błędy.
        /// </summary>
        static public Dictionary<string, string> CountersWarnings = new Dictionary<string, string>()
        {
            { "noRecord", "----"},
            { "wrongFileName", "Błąd! Niepoprawna nazwa pliku!" },
            { "xmlError", "Błąd podczas przetwarzania piku XML!" }
        };
    }
}
