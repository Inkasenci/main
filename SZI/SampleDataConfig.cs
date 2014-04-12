using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    /**
     * Generuje losowo rekordy w bazie danych. Używana do testów.
     */
    static class SampleDataConfig
    {
        /// <summary>
        /// Liczba generowanych inkasentów.
        /// </summary>
        static int numberOfCollectors = 5;
        /// <summary>
        /// Liczba generowanych klientów.
        /// </summary>
        static int numberOfCustomers = 10;
        /// <summary>
        /// Liczba generowanych terenów. Nie może być większa niż liczba elementów tablicy streets w SampleDataSource
        /// </summary>
        static int numberOfAreas = 5;
        /// <summary>
        /// Liczba generowanych liczników.
        /// </summary>
        static int numberOfCounters = 10;
        /// <summary>
        /// Liczba generowanych adresów.
        /// </summary>
        static int numberOfAddresses = 10;

        //! Generuje inkasentów losowo dobierając wartości pól i dodaje ich do bazy.
        static void GenerateCollectors()
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

        //! Generuje klientów losowo dobierając wartości pól i dodaje ich do bazy.
        static void GenerateCustomers()
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

        //! Generuje tereny losowo dobierając wartości pól. Losuje inkasenta przypisanego do danego terenu.
        static void GenerateAreas()
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

        //! Generuje adresy losowo dobierając wartości pól. Losuje teren przypisany do danego adresu.
        static void GenerateAddresses()
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

        //! Generuje liczniki losowo dobierając wartości pól. Losuje adres przypisany do danego licznika.
        static void GenerateCounters()
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

        //! Generuje losowo całą bazę danych.
        static public void GenerateDataBase()
        {
            ClearDataBase();
            GenerateCollectors();
            GenerateCustomers();
            GenerateAreas();
            GenerateAddresses();
            GenerateCounters();
        }

        //! Usuwa wszystkie rekordy z wszystkich tabel.
        static public void ClearDataBase()
        {
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                string query = @"DELETE FROM Collector;"+
                                "DELETE FROM Customer;"+
                                "DELETE FROM Area;"+
                                "DELETE FROM Counter;"+
                                "DELETE FROM Address;";

                dataBase.Database.ExecuteSqlCommand(query);
            }
        }

        //! Losuje jedenastocyfrową liczbę z odpowiedniego zakresu i sprawdza, czy ta liczba może być peselem.
        //! Jeśli jest to pesel, sprawdza, czy taki pesel nie istnieje już w tabeli, do której ma być wstawiony rekord z danym peselem.
        //! \param tableName Nazwa tabeli, do której będzie wstawiony rekord z wylosowanym peselem.
        //! \return Prawidłowy i unikalny w ramach odpowiedniej tabeli numer pesel.
        static string GenerateRandomPesel(string tableName)
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

        //! Losuje płeć, a następnie imię i nazwisko z dostępnej puli.
        //! \return Dwuelementowa tablica zawierająca imię i nazwisko.
        static string[] GenerateNameAndLastName()
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

        //! Losuje kod pocztowy z dostępnej puli.
        //! \return Dwuelementowa tablica zawierająca kod pocztowy i odpowiadające mu miasto.
        static string[] GeneratePostalCodeAndCity()
        {
            Random rnd = new Random();
            string[] postalCodeAndCity = new string[2];
            int index = rnd.Next(0, SampleDataSource.postalCodes.Length);

            postalCodeAndCity[0] = SampleDataSource.postalCodes[index];
            postalCodeAndCity[1] = SampleDataSource.cities[index];

            return postalCodeAndCity;
        }

        //! Spośród identyfikatorów w podanej tabeli wybiera losowo jeden.
        //! \param tableName Nazwa tabeli, z której ma być wylosowany klucz.
        //! \return Wylosowany klucz.
        static string ChooseRandomId(string tableName)
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
