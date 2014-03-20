using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace SZI
{
    public static class DBManipulator
    {
        public static void DeleteFromDB(List<string> IDs, int TableNumber)
        {
            if (TableNumber == 0)
                DeleteFromCollectors(IDs);

            else if (TableNumber == 1)
                DeleteFromCustomers(IDs);

            else if (TableNumber == 2)
                DeleteFromAreas(IDs);

            else if (TableNumber == 3)
                DeleteFromCounters(IDs);
        }

        private static void DeleteFromCollectors(List<string> IDs)
        {
            using (var database = new CollectorsManagementSystemEntities())
            {
                foreach (var id in IDs)
                {
                    var result = from r in database.Collectors where r.CollectorId == id select r;

                    if (result.Count() > 0)
                    {
                        foreach (Collector c in result)
                            database.Collectors.Remove(c);
                    }
                    database.SaveChanges();

                }
            }
        }

        private static void DeleteFromCustomers(List<string> IDs)
        {
            using (var database = new CollectorsManagementSystemEntities())
            {
                foreach (var id in IDs)
                {
                    var result = from r in database.Customers where r.CustomerId == id select r;

                    if (result.Count() > 0)
                    {
                        foreach (Customer c in result)
                            database.Customers.Remove(c);
                    }
                    database.SaveChanges();

                }
            }
        }

        private static void DeleteFromAreas(List<string> IDs)
        {
            List<Guid> guidIDs = new List<Guid>(IDs.Count);

            for (int i = 0; i < IDs.Count; i++)
                guidIDs.Insert(i, Auxiliary.ToGuid(Convert.ToInt32(IDs[i])));

            using (var database = new CollectorsManagementSystemEntities())
            {
                foreach (var id in guidIDs)
                {
                    var result = from r in database.Areas where r.AreaId == id select r;

                    if (result.Count() > 0)
                    {
                        foreach (Area a in result)
                            database.Areas.Remove(a);
                    }
                    database.SaveChanges();

                }
            }
        }

        private static void DeleteFromCounters(List<string> IDs)
        {
            List<Guid> guidIDs = new List<Guid>(IDs.Count);

            for (int i = 0; i < IDs.Count; i++)
                guidIDs.Insert(i, Auxiliary.ToGuid(Convert.ToInt32(IDs[i])));

            using (var database = new CollectorsManagementSystemEntities())
            {
                foreach (var id in guidIDs)
                {
                    var result = from r in database.Counters where r.AddressId == id select r;

                    if (result.Count() > 0)
                    {
                        foreach (Counter c in result)
                            database.Counters.Remove(c);
                    }
                    database.SaveChanges();
                }
            }
        }

    }
}
