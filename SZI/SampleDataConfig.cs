using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    static class SampleDataConfig
    {
        static int numberOfCollectors = 5;
        static int numberOfCustomers = 10;
        static int numberOfAreas = 5; //aczkolwiek nie moze byc ich wiecej niz ulic w SampleDataSource
        static int numberOfCounters = 10;
        static int numberOfAddresses = 10;

        static void GenerateCollectors() //generuje losowo inkasentow
        {
            Random rnd=new Random();
            string[] nameAndLastName;
            string[] postalCodeAndCity;
            Collector collector;

            for (int i = 0; i < numberOfCollectors; i++)
            {
                collector = new Collector();

                collector.CollectorId = GenerateRandomPesel("Collector");
                nameAndLastName = GenerateNameAndLastName();
                collector.Name = nameAndLastName[0];
                collector.LastName = nameAndLastName[1];
                postalCodeAndCity = GeneratePostalCodeAndCity();
                collector.PostalCode = postalCodeAndCity[0];
                collector.City = postalCodeAndCity[1];
                collector.Address = SampleDataSource.streets[rnd.Next(0, SampleDataSource.streets.Length)] + ' ' + rnd.Next(1, 100).ToString();
                collector.PhoneNumber = rnd.Next(500000000, 999999999).ToString();

                collector.InsertIntoDB();
            }
        }

        static void GenerateCustomers() //generuje losowo klientow
        {
            Random rnd = new Random();
            string[] nameAndLastName;
            string[] postalCodeAndCity;
            Customer customer;

            for (int i = 0; i < numberOfCustomers; i++)
            {
                customer = new Customer();

                customer.CustomerId = GenerateRandomPesel("Customer");
                nameAndLastName = GenerateNameAndLastName();
                customer.Name = nameAndLastName[0];
                customer.LastName = nameAndLastName[1];
                postalCodeAndCity = GeneratePostalCodeAndCity();
                customer.PostalCode = postalCodeAndCity[0];
                customer.City = postalCodeAndCity[1];
                customer.Address = SampleDataSource.streets[rnd.Next(0, SampleDataSource.streets.Length - 1)] + ' ' + rnd.Next(1, 100).ToString();
                customer.PhoneNumber = rnd.Next(500000000, 999999999).ToString();

                customer.InsertIntoDB();
            }
        }

        static void GenerateAreas() //generuje losowo tereny
        {
            Random rnd = new Random();
            Area area;

            for (int i = 0; i < numberOfAreas; i++)
            {
                area = new Area();

                area.AreaId = Guid.NewGuid();
                area.Street = SampleDataSource.streets[i];
                area.CollectorId = ChooseRandomId("Collector");

                area.InsertIntoDB();
            }
        }

        static void GenerateAddresses() //generuje losowo adresy
        {
            Random rnd = new Random();
            Address address;

            for (int i = 0; i < numberOfAddresses; i++)
            {
                address = new Address();

                address.AddressId = Guid.NewGuid();
                address.HouseNo = rnd.Next(1, 100);
                address.FlatNo = rnd.Next(1, 100);
                address.AreaId = new Guid(ChooseRandomId("Area"));

                address.InsertIntoDB();
            }
        }

        static void GenerateCounters() //generuje losowo liczniki
        {
            Random rnd = new Random();
            Counter counter;
            Counters dataBase = new Counters();

            for (int i = 0; i < numberOfCounters; i++)
            {
                counter = new Counter();

                counter.CounterNo = rnd.Next(1, 10000);
                while (MainValidation.CounterExists(counter.CounterNo))
                    counter.CounterNo = rnd.Next(1000, 10000);
                counter.CircuitNo = rnd.Next(1000, 10000);
                counter.AddressId = new Guid(ChooseRandomId("Address"));
                counter.CustomerId = ChooseRandomId("Customer");

                counter.InsertIntoDB();
            }
        }

        static public void GenerateDataBase() //generuje wszystko za jednym zamachem
        {
            ClearDataBase();
            GenerateCollectors();
            GenerateCustomers();
            GenerateAreas();
            GenerateAddresses();
            GenerateCounters();
        }

        static public void ClearDataBase() //czysci baze do cna
        {
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                string query = @"DELETE FROM Collector;"+
                                "DELETE FROM Customer;"+
                                "DELETE FROM Area;"+
                                "DELETE FROM Counter;"+
                                "DELETE FROM Address;"+
                                "DELETE FROM Reading;";

                dataBase.Database.ExecuteSqlCommand(query);
            }
        }

        static string GenerateRandomPesel(string tableName)
        /* przyjmuje nazwe tabeli, do ktorej ma wygenerowac pesel, jest to wazne, zeby pesel byl unikalny
         * zwraca prawidlowy pesel*/
        {
            Random rnd = new Random();
            IDataBase dataBase;
            int possiblePeselA;
            int possiblePeselB;
            string possiblePesel = String.Empty;
            bool correctPesel = false;
            bool uniquePesel = false;

            if (tableName == "Collector")
                dataBase = new Collectors();
            else
                dataBase = new Customers();

            while (!correctPesel || !uniquePesel)
            {
                possiblePeselA = rnd.Next(540000000, 949999999);
                possiblePeselB = rnd.Next(10, 99);
                possiblePesel = ((long)possiblePeselA * 100 + possiblePeselB).ToString();
                correctPesel = !IdentityValidation.CheckId(possiblePesel);
                if (correctPesel)
                {
                    if (tableName == "Collector")
                        uniquePesel = !MainValidation.CollectorExists(possiblePesel);
                    else
                        uniquePesel = !MainValidation.CustomerExists(possiblePesel);
                }
            }

            return possiblePesel;
        }

        static string[] GenerateNameAndLastName() //generuje imie i nazwisko zgodne z wylosowana plcia
        {
            Random rnd = new Random();
            int gender = rnd.Next(0, 2);
            string[] nameAndLastName = new string[2];

            nameAndLastName[1] = SampleDataSource.lastNames[rnd.Next(0, SampleDataSource.lastNames.Length)];
            if (gender == 0)
                nameAndLastName[0] = SampleDataSource.maleNames[rnd.Next(0, SampleDataSource.maleNames.Length)];
            else
            {
                nameAndLastName[0] = SampleDataSource.femaleNames[rnd.Next(0, SampleDataSource.femaleNames.Length)];
                if (nameAndLastName[1][nameAndLastName[1].Length - 1] == 'i')
                    nameAndLastName[1] = nameAndLastName[1].Substring(0, nameAndLastName[1].Length - 1) + 'a';
            }

            return nameAndLastName;
        }

        static string[] GeneratePostalCodeAndCity() //generuje pare kod pocztowy + miasto
        {
            Random rnd = new Random();
            string[] postalCodeAndCity = new string[2];
            int index = rnd.Next(0, SampleDataSource.postalCodes.Length);

            postalCodeAndCity[0] = SampleDataSource.postalCodes[index];
            postalCodeAndCity[1] = SampleDataSource.cities[index];

            return postalCodeAndCity;
        }

        static string ChooseRandomId(string tableName)
        /* przyjmuje nazwe tabeli, z ktorej ma pobrac klucz glowny
         * zwraca losowo wybrany klucz glowny*/
        {
            Random rnd = new Random();
            IDataBase dataBase;

            switch (tableName)
            {
                case "Collector":
                    dataBase=new Collectors();
                    break;
                case "Customer":
                    dataBase=new Customers();
                    break;
                case "Area":
                    dataBase=new Areas();
                    break;
                case "Address":
                    dataBase=new Addresses();
                    break;
                default:
                    dataBase=new Collectors();
                    break;
            }

            List<string[]> itemList = dataBase.itemList;

            return itemList.ElementAt(rnd.Next(0, itemList.Count))[0];
        }
    }
}
