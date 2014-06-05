using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    /// <summary>
    /// Statyczna klasa służąca do walidacji numeru pesel.
    /// </summary>
    static class IdentityValidation
    {
        /// <summary>
        /// Funkcja wycinająca ze stringu wybrane znaki.
        /// </summary>
        /// <param name="nrId">11-cyfrowy numer PESEL.</param>
        /// <param name="length">Ilość znaków które należy wyciąć z numeru PESEL.</param>
        /// <param name="start">Element od którego rozpoczynamy wycinanie tekstu.</param>
        /// <returns>Wycięty z numeru PESEL tekst.</returns>
        static private int CutId(string nrId, int start, int length)
        {
            return ((start + length) <= nrId.Length) ? Convert.ToInt32(nrId.Substring(start, length)) : 0;
        }

        /// <summary>
        /// Funkcja określająca wiek, w którym urodziła się osoba.
        /// </summary>
        /// <param name="nrId">11-cyfrowy numer PESEL.</param>
        /// <returns>Wiek z numeru PESEL.</returns>
        static private int CenturyId(string nrId)
        {
            return ((CutId(nrId, 3, 2) / 20) != 4) ? Convert.ToInt32((19 + (CutId(nrId, 3, 2) / 20))) : 18;
        }

        /// <summary>
        /// Funkcja określająca rok, w którym urodziła się osoba.
        /// </summary>
        /// <param name="nrId">11-cyfrowy numer PESEL.</param>
        /// <returns>Rok z numeru PESEL.</returns>
        static private int YearId(string nrId)
        {
            return Convert.ToInt32((CutId(nrId, 0, 2)));
        }

        /// <summary>
        /// Funkcja określająca miesiąc, w którym urodziła się osoba.
        /// </summary>
        /// <param name="nrId">11-cyfrowy numer PESEL.</param>
        /// <returns>Miesiąc z numeru PESEL.</returns>
        static private int MonthId(string nrId)
        {
            return Convert.ToInt32((CutId(nrId, 2, 2))) % 20;
        }

        /// <summary>
        /// Funkcja określająca dzień, w którym urodziła się osoba.
        /// </summary>
        /// <param name="nrId">11-cyfrowy numer PESEL.</param>
        /// <returns>Dzień z numeru PESEL.</returns>
        static private int DayId(string nrId)
        {
            return Convert.ToInt32((CutId(nrId, 4, 2)) );
        }

        /// <summary>
        /// Funkcja sprawdzająca, czy miesiąc jest różny od zera.
        /// </summary>
        /// <param name="month">2-cyfrowy string zawierajacy miesiąc urodzenia osoby.</param>
        /// <returns>Poprawność miesiąca, bądź nie.</returns>
        static public bool CheckMonthId(string month)
        {
            return (Convert.ToInt32(month) % 20 != 0) ? true : false;
        }

        /// <summary>
        /// Funkcja sprawdzająca, czy dzień jest różny od zera.
        /// </summary>
        /// <param name="day">2-cyfrowy string zawierajacy dzień urodzenia osoby.</param>
        /// <returns>Poprawność dnia, bądź nie.</returns>
        static public bool CheckDayId(string day)
        {
            return (Convert.ToInt32(day) % 20 != 0) ? true : false;
        }

        /// <summary>
        /// Funkcja zwracająca pełną datę urodzena danej osoby.
        /// </summary>
        /// <param name="nrId">11-cyfrowy numer PESEL.</param>
        /// <returns>Pełna data urodzenia.</returns>
        static public DateTime FullDate(string nrId)
        {
            return (new DateTime(CenturyId(nrId) * 100 + YearId(nrId), MonthId(nrId), DayId(nrId)));
        }

        /// <summary>
        /// Funkcja sprawdzająca płeć osoby.
        /// </summary>
        /// <param name="nrId">11-cyfrowy numer PESEL.</param>
        /// <returns>Wartość określająca płeć.</returns>
        static public int SexId(string nrId)
        {
            return (CutId(nrId, 9, 1) % 2 == 0) ? 0 : 1;
        }

        /// <summary>
        /// Funkcja obliczająca sumę kontrolą numeru PESEL.
        /// </summary>
        /// <param name="nrId">11-cyfrowy numer PESEL.</param>
        /// <returns>Wartość sumy kontrolnej.</returns>
        static public int IdSum(string nrId)
        {
            return (1 * CutId(nrId, 0, 1) + 3 * CutId(nrId, 1, 1) + 7 * CutId(nrId, 2, 1) + 9 * CutId(nrId, 3, 1) +
                1 * CutId(nrId, 4, 1) + 3 * CutId(nrId, 5, 1) + 7 * CutId(nrId, 6, 1) + 9 * CutId(nrId, 7, 1) + 1 * CutId(nrId, 8, 1) +
                3 * CutId(nrId, 9, 1)) % 10;
        }

        /// <summary>
        /// Funkcja sprawdzająca sumę kontrolną numeru PESEL.
        /// </summary>
        /// <param name="nrId">11-cyfrowy numer PESEL.</param>
        /// <returns>Wartość kontrolna.</returns>
        static public int CheckSum(string nrId)
        {
            return (10 - IdSum(nrId)) % 10;
        }

        /// <summary>
        /// Funkcja sprawdzająca poprawność numer PESEL.
        /// </summary>
        /// <param name="nrId">11-cyfrowy numer PESEL.</param>
        /// <returns>Wartość bool określająca poprawność numeru PESEL.</returns>
        static public bool CheckId(string nrId) //zwraca true, gdy PESEL jest nieprawidłowy
        {

            if(DayId(nrId) == 0)
                return true;
            if(MonthId(nrId) == 0)
                return true;

            return (CheckSum(nrId) == CutId(nrId, 10, 1) && nrId.Length == 11) ? false : true;
        }
    }
}
