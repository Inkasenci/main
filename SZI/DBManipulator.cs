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
            switch (TableNumber)
            {
                case 0:
                    DeleteFromCollectors(IDs);
                    break;

                case 1:
                    DeleteFromCustomers(IDs);
                    break;

                case 2:
                    DeleteFromAreas(IDs);
                    break;

                case 3:
                    DeleteFromCounters(IDs);
                    break;
                case 4:
                    DeleteFromAddresses(IDs);
                    break;

                default: 
                    break;
            }
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
                guidIDs.Insert(i, new Guid(IDs[i]));

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
            List<Int32> guidIDs = new List<Int32>(IDs.Count);

            for (int i = 0; i < IDs.Count; i++)
                guidIDs.Insert(i, Convert.ToInt32(IDs[i]));

            using (var database = new CollectorsManagementSystemEntities())
            {
                foreach (var Id in guidIDs)
                {
                    var counters = from counter in database.Counters
                                   where counter.CounterNo == Id
                                   select counter;

                    if (counters.Count() > 0)
                    {
                        foreach (Counter c in counters)
                            database.Counters.Remove(c);
                    }
                    database.SaveChanges();
                }
            }
        }
        
        private static void DeleteFromAddresses(List<string> IDs)
        {
            List<Guid> guidIDs = new List<Guid>(IDs.Count);

            for (int i = 0; i < IDs.Count; i++)
                guidIDs.Insert(i, new Guid(IDs[i]));

            using (var database = new CollectorsManagementSystemEntities())
            {
                foreach (var Id in guidIDs)
                {
                    var result = from r in database.Addresses where r.AddressId == Id select r;

                    if (result.Count() > 0)
                    {
                        foreach (Address a in result)
                            database.Addresses.Remove(a);
                    }
                    database.SaveChanges();
                }
            }
        }
    }
}
