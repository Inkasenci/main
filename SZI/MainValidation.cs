using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SZI
{
    static class MainValidation
    {
        /// <summary>
        /// Metoda walidująca dwa ComboBoxy służące do wprowadzenia do bazy inkasenta.
        /// </summary>
        /// <param name="cb1">Pierwszy walidowany ComboBox.</param>
        /// <param name="cb2">Drugi walidowany ComboBox.</param>
        /// <param name="CBtoEP">Słownik mapujący ComboBoxy na ErrorProvidery do nich przypisane.</param>
        /// <param name="CBtoBool">Słownik mapujący ComboBoxy na wartości określające pomyślność walidacji do nich przypisane.</param>
       
        static public void XNOR_ComboBoxValidation(ComboBox cb1, ComboBox cb2, Dictionary<Control, ErrorProvider> CBtoEP, Dictionary<Control, bool> CBtoBool)
        {
            //jeśli nie został wybrany ani klient, ani adres, lub w drugim przypadku został wybrany i klient, i adres, to walidacja pomyślna
            if ((cb1.SelectedIndex == 0 && cb2.SelectedIndex == 0) ||
                (cb1.SelectedIndex > 0 && cb2.SelectedIndex > 0))
            {
                CBtoEP[cb1].SetError(cb1, String.Empty);
                CBtoBool[cb1] = true;
                CBtoEP[cb2].SetError(cb2, String.Empty);
                CBtoBool[cb2] = true;
            }
            else
            {
                CBtoEP[cb1].SetError(cb1, "Nieprawidłowo wypełnione pole.");
                CBtoBool[cb1] = false;
                CBtoEP[cb2].SetError(cb2, "Nieprawidłowo wypełnione pole.");
                CBtoBool[cb2] = false;
            }
        }

        /// <summary>
        /// Metoda walidująca ComboBox dla przypadku, gdy nie musi być z niego wybrany żaden item.
        /// </summary>
        /// <param name="index">Index wybranego itemu.</param>
        /// <returns>Wartość określająca pomyślność walidacji.</returns>
        static public bool OptionalChoice_ComboBox(string index)
        {
            int i = Convert.ToInt32(index);
            if (i < 0)
                return false;
            else 
                return true;
        }

        /// <summary>
        /// Metoda walidująca ComboBox dla przypadku, gdy musi być z niego wybrany item.
        /// </summary>
        /// <param name="index">Index wybranego itemu.</param>
        /// <returns>Wartość określająca pomyślność walidacji.</returns>
        static public bool MandatoryChoice_ComboBox(string index) //z Comboboxa musi zostać wybrany item
        {
            int i = Convert.ToInt32(index);
            if (i <= 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Metoda walidująca CheckBox dla przypadku, gdy przypisanie inkasenta jest opcjonalne.
        /// </summary>
        /// <param name="s">ID inkasenta.</param>
        /// <returns>Wartość określająca pomyślność walidacji.</returns>
        static public bool OptionalCollector(string s)
        {
            if (EmptyString(s))
                return true;
            else
                return CollectorExists(s);
        }

        /// <summary>
        /// Metoda walidująca CheckBox.
        /// </summary>
        /// <param name="s">ID.</param>
        /// <returns>Wartość określająca pomyślność walidacji.</returns>
        static public bool IDValidation(string ID)
        {
            Int64 Parse;
            if (!Int64.TryParse(ID, out Parse) || IdentityValidation.CheckId(ID))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Metode zwracająca string z wielką literą na jego początku.
        /// </summary>
        /// <param name="s">String do zamienienia wielkiej litery na jego początku.</param>
        /// <returns>Zamieniony string z wielką literą na początku.</returns>
        static public string UppercaseFirst(string s)
        {
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Walidacja numeru licznika, układu, mieszkania i domu.
        /// </summary>
        /// <param name="Number">Walidowany numer.</param>
        /// <returns>Wartość określająca pomyślność walidacji.</returns>
        static public bool CircuitAndCounterAndHouseAndFlatNumberValidation(string Number)
        {
            int Parse;

            if (Int32.TryParse(Number, out Parse))
                return CheckCircuitAndCounterAndHouseAndFlatNumber(Parse);
            else
                return false;
        }

        /// <summary>
        /// Walidacja numeru licznika, układu, mieszkania i domu.
        /// </summary>
        /// <param name="No">Walidowany numer.</param>
        /// <returns>Wartość określająca pomyślność walidacji.</returns>
        static private bool CheckCircuitAndCounterAndHouseAndFlatNumber(int No)
        {
            if (No > 0)
                return true;
            else 
                return false;
        }

        /// <summary>
        /// Określa czy string zawiera cyfry.
        /// </summary>
        /// <param name="S">String, który zostanie sprawdzony pod kątem zawartości cyfr.</param>
        /// <returns>Wartość określająca zawartość cyfr.</returns>
        static private bool ContainsNumbers(string S)
        {
            return S.Any(char.IsDigit);
        }

        /// <summary>
        /// Określa czy string zawiera litery.
        /// </summary>
        /// <param name="S">String, który zostanie sprawdzony pod kątem zawartości liter.</param>
        /// <returns>Wartość określająca zawartość liter.</returns>
        static public bool ContainsLetters(string S)
        {
            return S.Any(char.IsLetter);
        }

        /// <summary>
        /// Walidacja stringu z nazwą miasta.
        /// </summary>
        /// <param name="No">Walidowany string.</param>
        /// <returns>Wartość określająca pomyślność walidacji.</returns>
        static public bool CityNameValidation(string Name)
        {
            if (Name.Length < 2 || ContainsNumbers(Name))
                return false;
            else
                return true;                
        }

        /// <summary>
        /// Walidacja stringu z nazwą ulicy.
        /// </summary>
        /// <param name="No">Walidowany string.</param>
        /// <returns>Wartość określająca pomyślność walidacji.</returns>
        static public bool StreetValidation(string Name)
        {
            if (Name.Length < 2)
                return false;
            else
                return true;  
        }

        /// <summary>
        /// Walidacja stringu z kodem pocztowym.
        /// </summary>
        /// <param name="PostalCode">Walidowany kod pocztowy.</param>
        /// <returns>Wartość określająca pomyślność walidacji.</returns>
        static public bool PostalCodeValidation(string PostalCode)
        {
            Regex regex = new Regex(@"[0-9]{2}-[0-9]{3}");
            Match match = regex.Match(PostalCode);
            if (!match.Success)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Walidacja stringu z numerem telefonu.
        /// </summary>
        /// <param name="PostalCode">Walidowany numer telefonu.</param>
        /// <returns>Wartość określająca pomyślność walidacji.</returns>
        static public bool PhoneValidation(string Phone)
        {
            if (Phone.Length != 9 || ContainsLetters(Phone))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Sprawdza czy klient z ID istnieje w bazie.
        /// </summary>
        /// <param name="ID">Sprawdzane ID.</param>
        /// <returns>Wartośc logiczna określająca czy klient z ID istnieje w bazie.</returns>
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

        /// <summary>
        /// Sprawdza czy inkasent z ID istnieje w bazie.
        /// </summary>
        /// <param name="ID">Sprawdzane ID.</param>
        /// <returns>Wartośc logiczna określająca czy inkasent z ID istnieje w bazie.</returns>
        static public bool CollectorExists(string ID)
        {
            foreach (Collector c in Collectors.collectorList)
                if (c.CollectorId == ID)
                    return true;

            return false;
        }

        /// <summary>
        /// Sprawdza czy adres z ID istnieje w bazie.
        /// </summary>
        /// <param name="ID">Sprawdzane ID.</param>
        /// <returns>Wartośc logiczna określająca czy adres z ID istnieje w bazie.</returns>
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

        /// <summary>
        /// Sprawdza czy licznik z ID istnieje w bazie.
        /// </summary>
        /// <param name="ID">Sprawdzane ID.</param>
        /// <returns>Wartośc logiczna określająca czy licznik z ID istnieje w bazie.</returns>
        static public bool CounterExists(int ID)
        {
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                var Counters = from c in dataBase.Counters
                               select c.CounterNo;

                foreach (int counterNo in Counters)
                    if (counterNo == ID)
                        return true;
            }

            return false;
        }

        /// <summary>
        /// Sprawdza czy string jest pusty.
        /// </summary>
        /// <param name="stringToValidate">Sprawdzany string.</param>
        /// <returns>Wartośc logiczna określająca czy string jest pusty.</returns>
        static public bool EmptyString(string stringToValidate)
        {
            return (stringToValidate == String.Empty) ? false : true;
        }
    }
}
