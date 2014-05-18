using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    /// <summary>
    /// Klasa pozwalająca na obsługę opcji "powiązane rekordy".
    /// </summary>
    static public class ConnectionRecordsQuery
    {
        /// <summary>
        /// Zwraca rekordy powiązane z zaznaczonym w ListView Inkasentem
        /// </summary>
        /// <returns>Rekordy powiązane z zaznaczonym w ListView Inkasentem</returns>
        static public List<List<string>> ReturnRecordsAssociatedWithCollector(string id)
        {
            string CollectorID = id;
            List<List<string>> AssociatedRecords = new List<List<string>>();

            using (var database = new CollectorsManagementSystemEntities())
            {
                var foreignResult = (from f in database.Areas
                                     where f.CollectorId == CollectorID
                                     select f).ToList();

                for (int i = 0; i < foreignResult.Count(); i++)
                {
                    AssociatedRecords.Add(new List<string>());
                    AssociatedRecords[i].Add(foreignResult[i].AreaId.ToString());
                    AssociatedRecords[i].Add(foreignResult[i].Street);
                }
            }

            return AssociatedRecords;
        }

        /// <summary>
        /// Zwraca rekordy powiązane z zaznaczonym w ListView Klientem
        /// </summary>
        /// <returns>Rekordy powiązane z zaznaczonym w ListView Klientem</returns>
        static public List<List<string>> ReturnRecordsAssociatedWithCustomer(string id)
        {
            string CustomerID = id;
            List<List<string>> AssociatedRecords = new List<List<string>>();

            using (var database = new CollectorsManagementSystemEntities())
            {

                var foreignResult = (from f in database.Counters
                                     where f.CustomerId == CustomerID
                                     select f).ToList();

                for (int i = 0; i < foreignResult.Count(); i++)
                {
                    AssociatedRecords.Add(new List<string>());
                    AssociatedRecords[i].Add(foreignResult[i].CircuitNo.ToString());
                    AssociatedRecords[i].Add(foreignResult[i].CounterNo.ToString());
                    AssociatedRecords[i].Add(foreignResult[i].AddressId.HasValue ? Counters.FetchFullAddress(foreignResult[i].AddressId.Value) : String.Empty);

                }
            }

            return AssociatedRecords;
        }

        /// <summary>
        /// Zwraca rekordy powiązane z zaznaczonym w ListView Terenem
        /// </summary>
        /// <returns>Rekordy powiązane z zaznaczonym w ListView Terenem</returns>
        static public List<List<string>> ReturnRecordsAssociatedWithArea(string id)
        {
            Guid AreaID = new Guid(id);
            List<List<string>> AssociatedRecords = new List<List<string>>();

            using (var database = new CollectorsManagementSystemEntities())
            {
                var foreignResult = (from f in database.Addresses
                                     where f.AreaId == AreaID
                                     select f).ToList();

                for (int i = 0; i < foreignResult.Count(); i++)
                {
                    AssociatedRecords.Add(new List<string>());
                    AssociatedRecords[i].Add(foreignResult[i].AddressId.ToString());
                    AssociatedRecords[i].Add(foreignResult[i].HouseNo.ToString());
                    AssociatedRecords[i].Add(foreignResult[i].FlatNo.ToString());
                }
            }
            return AssociatedRecords;
        }

        /// <summary>
        /// Zwraca rekordy powiązane z zaznaczonym w ListView Licznikiem
        /// </summary>
        /// <returns>Rekordy powiązane z zaznaczonym w ListView Licznikiem</returns>
        static public List<List<string>> ReturnRecordsAssociatedWithCounter(string id)
        {
            int CounterID = Convert.ToInt32(id);
            List<List<string>> AssociatedRecords = new List<List<string>>();

            using (var database = new CollectorsManagementSystemEntities())
            {
                var foreignResult = (from f in database.Readings
                                     where f.CounterNo == CounterID
                                     select f).ToList();

                for (int i = 0; i < foreignResult.Count(); i++)
                {
                    AssociatedRecords.Add(new List<string>());
                    AssociatedRecords[i].Add(foreignResult[i].ReadingId.ToString());
                    AssociatedRecords[i].Add(foreignResult[i].Date.ToShortDateString()+' '+foreignResult[i].Date.ToLongTimeString());
                    AssociatedRecords[i].Add(foreignResult[i].Value.ToString());
                }
            }
            return AssociatedRecords;
        }

        /// <summary>
        /// Zwraca rekordy powiązane z zaznaczonym w ListView Adresem
        /// </summary>
        /// <returns>Rekordy powiązane z zaznaczonym w ListView Adresem</returns>
        static public List<List<string>> ReturnRecordsAssociatedWithAddress(string id)
        {
            Guid AddressID = new Guid(id);
            List<List<string>> AssociatedRecords = new List<List<string>>();

            using (var database = new CollectorsManagementSystemEntities())
            {
                var foreignResult = (from f in database.Counters
                                     where f.AddressId == AddressID
                                     select f).ToList();

                for (int i = 0; i < foreignResult.Count(); i++)
                {
                    AssociatedRecords.Add(new List<string>());
                    AssociatedRecords[i].Add(foreignResult[i].CounterNo.ToString());
                    AssociatedRecords[i].Add(foreignResult[i].CircuitNo.ToString());
                    AssociatedRecords[i].Add(Counters.FetchFullAddress(foreignResult[i].AddressId.Value));
                    AssociatedRecords[i].Add(Counters.FetchCustomer(foreignResult[i].CustomerId));

                }
            }

            return AssociatedRecords;
        }
    }
}
