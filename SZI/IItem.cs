using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;


namespace SZI
{

    public interface IItem
    {
        void InsertIntoDB();
    }
}
