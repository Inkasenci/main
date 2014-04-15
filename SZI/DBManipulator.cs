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

        /// <summary>
        /// Usuwa rekordy z tabeli Inkasent. Zamienia identyfikatory tej tabeli będące kluczami obcymi w tabeli Teren na wartość null.
        /// Usuwa rekordy z tabeli Odczyt powiązane z usuwanymi rekordami tabeli Inkasent.
        /// </summary>
        /// <param name="IDs">Lista identyfikatorów inkasentów do skasowania.</param>
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

                    var foreignResult = from f in database.Areas where f.CollectorId == id select f;

                    if (foreignResult.Count() > 0)
                    {
                        foreach (Area a in foreignResult)
                            a.CollectorId = null;
                    }

                    var foreignResult2 = from f in database.Readings where f.CollectorId == id select f;

                    if (foreignResult2.Count() > 0)
                    {
                        List<string> foreignList = new List<string>();

                        foreach (Reading r in foreignResult2)
                            foreignList.Add(r.ReadingId.ToString());

                        DeleteFromReadings(foreignList);
                    }

                    database.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Usuwa rekordy z tabeli Klient. Identyfikatory usuwanych rekordów z tej tabeli będące kluczami obcymi w tabeli Licznik zamieniane są na wartość null.
        /// Zasady nałożone na bazę danych wymagają również ustawienia na wartość null odpowiednich kluczy obcych adresów w tabeli Licznik.
        /// </summary>
        /// <param name="IDs">Lista identyfikatorów rekordów przeznaczonych do usunięcia.</param>
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

                    var foreginResult = from f in database.Counters where f.CustomerId == id select f;

                    if (result.Count() > 0)
                    {
                        foreach (Counter c in foreginResult)
                        {
                            c.CustomerId = null;
                            c.AddressId = null;
                        }
                    }
                        
                    database.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Usuwa rekordy z tabeli Teren.
        /// Jeśli z usuwanymi rekordami są powiązanie rekordy w tabeli Adres, również są usuwane z wszystkim konsekwencjami związanymi z usuwaniem rekordów tabeli Adres.
        /// </summary>
        /// <param name="IDs">Lista identyfikatorów rekordów, które mają zostać skasowane.</param>
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

                    var foreignResult = from f in database.Addresses where f.AreaId == id select f;

                    if (foreignResult.Count() > 0)
                    {
                        List<string> foreignList = new List<string>();

                        foreach (Address a in foreignResult)
                            foreignList.Add(a.AddressId.ToString());

                        DeleteFromAddresses(foreignList);
                    }
                    
                    database.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Usuwa wybrane rekordy z tabeli Licznik. Rekordy z tabeli Odczyt powiązane z usuwanymi rekordami tabeli Licznik również są usuwane.
        /// </summary>
        /// <param name="IDs">Lista identyfikatorów rekordów tabeli Licznik przeznaczonych do usunięcia.</param>
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

                    var foreignResult = from f in database.Readings where f.CounterNo == Id select f;

                    if (foreignResult.Count() > 0)
                    {
                        List<string> foreignList = new List<string>();

                        foreach (Reading r in foreignResult)
                            foreignList.Add(r.ReadingId.ToString());

                        DeleteFromReadings(foreignList);
                    }

                    database.SaveChanges();
                }
            }
        }
        
        /// <summary>
        /// Usuwa rekordy z tabeli Adres. Jeśli w tabeli Licznik istniały odniesienia do usuwanych rekordów, zostają one zastąpione wartościami null.
        /// Zgodnie z zasadami nałożonymi na bazę danych, również odpowiednie identyfikatory klienta w tabeli Licznik są ustawiane na wartość null.
        /// </summary>
        /// <param name="IDs">Lista identyfikatorów rekordów z tabeli Adres przeznaczonych do usunięcia.</param>
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

                    var foreignResult = from f in database.Counters where f.AddressId == Id select f;

                    if (foreignResult.Count() > 0)
                    {
                        foreach (Counter c in foreignResult)
                        {
                            c.AddressId = null;
                            c.CustomerId = null;
                        }
                    }

                    database.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Usuwa rekordy z tabeli Odczyt.
        /// </summary>
        /// <param name="IDs">Lista identyfikatorów rekordów tabeli Odczyt przeznaczonych do usunięcia.</param>
        private static void DeleteFromReadings(List<string> IDs)
        {
            List<Guid> guidIDs = new List<Guid>(IDs.Count);

            for (int i = 0; i < IDs.Count; i++)
                guidIDs.Insert(i, new Guid(IDs[i]));

            using (var database = new CollectorsManagementSystemEntities())
            {
                foreach (var id in guidIDs)
                {
                    var result = from r in database.Readings where r.ReadingId == id select r;

                    if (result.Count() > 0)
                    {
                        foreach (Reading r in result)
                            database.Readings.Remove(r);
                    }

                    database.SaveChanges();
                }
            }
        }            

        /// <summary>
        /// Sprawdza, czy dla danego rekordu nie ma odniesienia w tabelach, które są w związku z tabelą, z której pochodzi rekord.
        /// </summary>
        /// <param name="tableName">Nazwa tabeli, z której pochodzi rekord.</param>
        /// <param name="id">Klucz rekordu.</param>
        /// <returns>true - odniesienie do rekordu znalezione. false - odniesienie do rekordu nieznalezione.</returns>
        static public bool IdExistsInOtherTable(string tableName, string id)
        {
            int count;
            Guid guidId;

            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                switch (tableName)
                {
                    case "Collector":
                        count = (from a in dataBase.Areas where a.CollectorId == id select a).Count();
                        break;
                    case "Customer":
                        count = (from c in dataBase.Counters where c.CustomerId == id select c).Count();
                        break;
                    case "Area":
                        guidId = new Guid(id);
                        count = (from a in dataBase.Addresses where a.AreaId == guidId select a).Count();
                        break;
                    case "Address":
                        guidId = new Guid(id);
                        count = (from c in dataBase.Counters where c.AddressId == guidId select c).Count();
                        break;
                    default:
                        count = 0;
                        break;
                }

            }

            if (count > 0)
                return true;
            else
                return false;
        }
    }
}
