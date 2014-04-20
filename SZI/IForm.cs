using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{

    /// <summary>
    /// Interfejs który musi być implementowany przez ModifyForm i InsertForm. Potrzebny do tego, by wiedzieć czy cokolwiek w bazie zostało zmienione.
    /// </summary>
    interface IForm
    {
        /// <summary>
        /// Zwraca wartość logiczną, która określa czy baza została zmieniona.
        /// </summary>
        bool Modified
        {
            get;
        }
    }
}
