using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    public sealed class ComboBoxItem
    {
        public string longItemDescription { get; private set; }
        public string shortItemDescription { get; private set; }

        public ComboBoxItem(string longItemDescription, string shortItemDescription)
        {
            this.longItemDescription = longItemDescription;
            this.shortItemDescription = shortItemDescription;
        }
    }
}
