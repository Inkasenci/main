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
    public sealed class ComboBoxItem
    {
        /// <summary>
        /// Długi opis rekordu. Wykorzystywany, gdy lista jest rozwinięta.
        /// </summary>
        public string longItemDescription { get; private set; }
        /// <summary>
        /// Krótki opis rekordu. Najważniejsze pola wybranego rekordu. Wyświetlane, gdy lista jest zwinięta i posiada wybrany element.
        /// </summary>
        public string shortItemDescription { get; private set; }

        /// <summary>
        /// Inicjuje pola klasy.
        /// </summary>
        /// <param name="longItemDescription">Długi opis rekordu.</param>
        /// <param name="shortItemDescription">Krótki opis rekordu.</param>
        public ComboBoxItem(string longItemDescription, string shortItemDescription)
        {
            this.longItemDescription = longItemDescription;
            this.shortItemDescription = shortItemDescription;
        }
    }
}
