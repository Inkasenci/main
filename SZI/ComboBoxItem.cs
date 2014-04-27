using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    /// <summary>
    /// Obiekty tej klasy są polami rozwijanej listy. Zawierają krótki i długi opis rekordu.
    /// </summary>
    public class ComboBoxItem
    {
        /// <summary>
        /// Długi opis rekordu. Wykorzystywany, gdy lista jest rozwinięta.
        /// </summary>
        public string longItemDescription;
        /// <summary>
        /// Krótki opis rekordu. Najważniejsze pola wybranego rekordu. Wyświetlane, gdy lista jest zwinięta i posiada wybrany element.
        /// </summary>
        public string shortItemDescription;
        public string formattedLongItemDescription;
        public string[] fields;

        /// <summary>
        /// Konwertuje tablicę napisów na jeden napis.
        /// </summary>
        /// <param name="recordFields">Tablica napisów. Każdy z napisów jest polem rekordu.</param>
        /// <returns>Napis będący połączonymi polami rekordu.</returns>
        private string ConvertRecordToString()
        {
            string convertedRecord = String.Empty;

            foreach (string recordField in fields)
                convertedRecord += recordField + " ";

            return convertedRecord;
        }

        /// <summary>
        /// Wydobywa z długiego opisu rekordu słowa będące jego krótkim opisem.
        /// </summary>
        /// <param name="item">Długi opis rekordu.</param>
        /// <returns>Napis będący krótkim opisem rekordu.</returns>
        private string MineDescriptionWords(int[] shortDescriptionWords)
        {
            string minedDescription = String.Empty;

            foreach (int shortDescriptionWord in shortDescriptionWords)
                minedDescription += fields[shortDescriptionWord] + " ";

            return minedDescription;
        }
        
        /// <summary>
        /// Inicjuje pola klasy.
        /// </summary>
        /// <param name="longItemDescription">Długi opis rekordu.</param>
        /// <param name="shortItemDescription">Krótki opis rekordu.</param>
        public ComboBoxItem(string[] fields, int[] shortDescriptionWords)
        {
            this.fields = fields;
            longItemDescription = ConvertRecordToString();
            shortItemDescription = MineDescriptionWords(shortDescriptionWords);
        }
    }
}
