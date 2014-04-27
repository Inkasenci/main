using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    static class IdentityValidation
    {
        static private int CutId(string nrId, int start, int length)
        {
            return ((start + length) <= nrId.Length) ? Convert.ToInt32(nrId.Substring(start, length)) : 0;
        }

        static private int CenturyId(string nrId)
        {
            return ((CutId(nrId, 3, 2) / 20) != 4) ? Convert.ToInt32((19 + (CutId(nrId, 3, 2) / 20))) : 18;
        }

        static private int YearId(string nrId)
        {
            return Convert.ToInt32((CutId(nrId, 0, 2)));
        }

        static private int MonthId(string nrId)
        {
            return Convert.ToInt32((CutId(nrId, 2, 2)));
        }

        static private int DayId(string nrId)
        {
            return Convert.ToInt32((CutId(nrId, 4, 2)));
        }

        static public DateTime FullDate(string nrId)
        {
            return (new DateTime(CenturyId(nrId) * 100 + YearId(nrId), MonthId(nrId), DayId(nrId)));
        }

        static public int SexId(string nrId)
        {
            return (CutId(nrId, 9, 1) % 2 == 0) ? 0 : 1;
        }

        static public bool CheckId(string nrId) //zwraca true, gdy PESEL jest nieprawidłowy
        {

            if(DayId(nrId) == 0)
                return true;
            if(MonthId(nrId) == 0)
                return true;

            int baseDate = (1 * CutId(nrId, 0, 1) + 3 * CutId(nrId, 1, 1) + 7 * CutId(nrId, 2, 1) + 9 * CutId(nrId, 3, 1) +
                1 * CutId(nrId, 4, 1) + 3 * CutId(nrId, 5, 1) + 7 * CutId(nrId, 6, 1) + 9 * CutId(nrId, 7, 1) + 1 * CutId(nrId, 8, 1) +
                3 * CutId(nrId, 9, 1)) % 10;

            return ((10 - baseDate) % 10 == CutId(nrId, 10, 1) && nrId.Length == 11) ? false : true;
        }
    }
}
