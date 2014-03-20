using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    interface IDataBase
    {
        ListView lv { get; set; }
        string[] columnList { get; set; }
        string className { get; set; }
        int recordCount { get; }
        ListView ListViewInitiate();
    }
}
