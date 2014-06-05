using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{

    /// <summary>
    /// Interfejs, który musi być implementowany przez ModifyForm i InsertForm. Potrzebny, aby wiedzieć czy cokolwiek zostało zmienione w bazie.
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
