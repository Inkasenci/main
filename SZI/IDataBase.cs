using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    public interface IDataBase
    {
        string[] columnList { get; set; }
        string className { get; set; }
        int recordCount { get; }
        List<string[]> itemList { get; set; }
        void RefreshList();
    }
}
