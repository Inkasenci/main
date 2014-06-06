using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    /// <summary>
    /// Klasa pomocnicza dla okna CountersForm.
    /// </summary>
    class CountersFormClass
    {
        /// <summary>
        /// Id inkasenta.
        /// </summary>
        private string collectorId;

        /// <summary>
        /// Imię inkasenta.
        /// </summary>
        private string firstName;

        /// <summary>
        /// Nazwisko inkasenta.
        /// </summary>
        private string lastName;

        /// <summary>
        /// Liczba liczników do odczytu.
        /// </summary>
        private int countersCount;

        /// <summary>
        /// Konstruktor inicjujący pola związane z klasą.
        /// </summary>
        /// <param name="cid">Id inkasenta.</param>
        /// <param name="fn">Imię inkasenta.</param>
        /// <param name="ln">Nazwisko inkasenta.</param>
        /// <param name="cc">Liczba liczników.</param>
        public CountersFormClass(string cid, string fn, string ln, int cc)
        {
            this.collectorId = cid;
            this.firstName = fn;
            this.lastName = ln;
            this.countersCount = cc;
        }

        /// <summary>
        /// Metoda pozwalająca zwrócić i udostępnić id inkasenta.
        /// </summary>
        public string CollectorId
        {
            get
            {
                return collectorId;
            }
            set
            {
                value = collectorId;
            }
        }

        /// <summary>
        /// Metoda pozwalająca zwrócić i udostępnić imię inkasenta.
        /// </summary>
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                value = firstName;
            }
        }

        /// <summary>
        /// Metoda pozwalająca zwrócić i udostępnić nazwisko inkasenta.
        /// </summary>
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                value = lastName;
            }
        }

        /// <summary>
        /// Metoda pozwalająca zwrócić i udostępnić liczbę liczników do odczytu.
        /// </summary>
        public int CountersCount
        {
            get
            {
                return countersCount;
            }
            set
            {
                value = countersCount;
            }
        }
    }
}
