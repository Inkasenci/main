using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    /// <summary>
    /// Generuje losowo rekordy w bazie danych. Używana do testów.
    /// </summary>
    static class SampleDataConfig
    {
        /// <summary>
        /// Liczba generowanych inkasentów.
        /// </summary>
        static int numberOfCollectors = 2;
        /// <summary>
        /// Liczba generowanych klientów.
        /// </summary>
        static int numberOfCustomers = 100;
        /// <summary>
        /// Liczba generowanych terenów. Nie może być większa niż liczba elementów tablicy streets w SampleDataSource
        /// </summary>
        static int numberOfAreas = 4;
        /// <summary>
        /// Liczba generowanych liczników.
        /// </summary>
        static int numberOfCounters = 100;
        /// <summary>
        /// Liczba generowanych adresów.
        /// </summary>
        static int numberOfAddresses = 100;
        /// <summary>
        /// Liczba generowanych odczytów.
        /// </summary>
        static int numberOfReadings = 300;
        /// <summary>
        /// Wczytane dane.
        /// </summary>
        static IDataBase[] DataBase = null;

        /// <summary>
        /// Wczytuje używane aktualnie dane.
        /// </summary>
        /// <param name="choose">Tablica tabel, które mają zostać wybrane z bazy danych.</param>
        static void ReadDataFromDataBase(int[] choose)
        {
            DataBase = new IDataBase[choose.Length];
            int i = 0;
            foreach (var choosen in choose)
            {
                switch (choosen)
                {
                    case 0:
                        DataBase[i++] = new Collectors();
                        break;
                    case 1:
                        DataBase[i++] = new Customers();
                        break;
                    case 2:
                        DataBase[i++] = new Areas();
                        break;
                    case 3:
                        DataBase[i++] = new Addresses();
                        break;
                    case 4:
                        DataBase[i++] = new Counters();
                        break;
                    default:
                        DataBase[i++] = new Collectors();
                        break;
                }
            }
        }

        /// <summary>
        /// Czyści z pamięci wczytane wcześniej dane.
        /// </summary>
        static void RemoveDataFromStatic()
        {
            DataBase = null;
        }

        /// <summary>
        /// Generuje inkasentów losowo dobierając wartości pól i dodaje ich do bazy.
        /// </summary>
        static void GenerateCollectors()
        {
            Random rnd = new Random();
            string[] nameAndLastName;
            string[] postalCodeAndCity;
            Collector collector;
            int tpmGender = 0;

            for (int i = 0; i < numberOfCollectors; i++)
            {
                collector = new Collector();
                tpmGender = rnd.Next(0, 9);
                collector.CollectorId = GenerateRandomPesel("Collector", tpmGender);
                nameAndLastName = GenerateNameAndLastName(tpmGender);
                collector.Name = nameAndLastName[0];
                collector.LastName = nameAndLastName[1];
                postalCodeAndCity = GeneratePostalCodeAndCity();
                collector.PostalCode = postalCodeAndCity[0];
                collector.City = postalCodeAndCity[1];
                collector.Address = SampleDataSource.streets[rnd.Next(0, SampleDataSource.streets.Length)] + ' ' + rnd.Next(1, 100).ToString();
                collector.PhoneNumber = GeneratePhoneNumber();

                collector.InsertIntoDB();
            }
        }

        /// <summary>
        /// Generuje klientów losowo dobierając wartości pól i dodaje ich do bazy.
        /// </summary>
        static void GenerateCustomers()
        {
            Random rnd = new Random();
            string[] nameAndLastName;
            string[] postalCodeAndCity;
            int tpmGender = 0;
            Customer customer;

            for (int i = 0; i < numberOfCustomers; i++)
            {
                customer = new Customer();
                tpmGender = rnd.Next(0, 9);
                customer.CustomerId = GenerateRandomPesel("Customer", tpmGender);
                nameAndLastName = GenerateNameAndLastName(tpmGender);
                customer.Name = nameAndLastName[0];
                customer.LastName = nameAndLastName[1];
                postalCodeAndCity = GeneratePostalCodeAndCity();
                customer.PostalCode = postalCodeAndCity[0];
                customer.City = postalCodeAndCity[1];
                customer.Address = SampleDataSource.streets[rnd.Next(0, SampleDataSource.streets.Length - 1)] + ' ' + rnd.Next(1, 100).ToString();
                customer.PhoneNumber = GeneratePhoneNumber();

                customer.InsertIntoDB();
            }
        }

        /// <summary>
        /// Generuje tereny losowo dobierając wartości pól. Losuje inkasenta przypisanego do danego terenu.
        /// </summary>
        static void GenerateAreas()
        {
            Random rnd = new Random();
            Area area;

            for (int i = 0; i < numberOfAreas; i++)
            {
                area = new Area();

                area.AreaId = Guid.NewGuid();
                area.Street = SampleDataSource.streets[i];
                area.CollectorId = ChooseRandomId(0);

                area.InsertIntoDB();
            }
        }

        /// <summary>
        /// Generuje adresy losowo dobierając wartości pól. Losuje teren przypisany do danego adresu.
        /// </summary>
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
                address.AreaId = new Guid(ChooseRandomId(0));

                address.InsertIntoDB();
            }
        }

        /// <summary>
        /// Generuje liczniki losowo dobierając wartości pól. Losuje adres i klienta przypisanego do danego licznika.
        /// </summary>
        static void GenerateCounters()
        {
            Random rnd = new Random();
            Counter counter;
            Counters dataBase = new Counters();

            for (int i = 0; i < numberOfCounters; i++)
            {
                counter = new Counter();

                counter.CounterNo = rnd.Next(1000, 10000);
                while (MainValidation.CounterExists(counter.CounterNo))
                    counter.CounterNo = rnd.Next(1000, 10000);
                counter.CircuitNo = rnd.Next(1000, 10000);
                counter.AddressId = new Guid(ChooseRandomId(1));
                counter.CustomerId = ChooseRandomId(0);

                counter.InsertIntoDB();
            }
        }

        /// <summary>
        /// Generuje losowo odczyty.
        /// </summary>
        static void GenerateReadings()
        {
            Random rnd = new Random();
            Reading reading;

            for (int i = 0; i < numberOfReadings; i++)
            {
                reading = new Reading();

                reading.ReadingId = Guid.NewGuid();
                reading.CounterNo = Convert.ToInt32(ChooseRandomId(0));

                using (var dataBase = new CollectorsManagementSystemEntities())
                {
                    var tmp = (from c in dataBase.Counters where c.CounterNo == reading.CounterNo select c.AddressId).FirstOrDefault();
                    var tmp2 = (from a in dataBase.Addresses where a.AddressId == tmp select a.AreaId).FirstOrDefault();
                    reading.CollectorId = (from a in dataBase.Areas where a.AreaId == tmp2 select a.CollectorId).FirstOrDefault().ToString();

                    var friendsOfReading = from r in dataBase.Readings where r.CounterNo == reading.CounterNo orderby r.Date descending select r;
                    if (friendsOfReading.Count() == 0)
                        reading.Value = rnd.Next(0, 1000);
                    else
                    {
                        reading.Value = friendsOfReading.FirstOrDefault().Value + rnd.Next(0, 1000);

                        foreach (Reading friendOfReading in friendsOfReading)
                            friendOfReading.Date = friendOfReading.Date.AddDays(-30);

                        dataBase.SaveChanges();
                    }

                    reading.Date = DateTime.Now;
                }

                reading.InsertIntoDB();
            }
        }

        /// <summary>
        /// Generuje losowo całą bazę danych.
        /// </summary>
        static public void GenerateDataBase()
        {
            ClearDataBase();
            GenerateCollectors();
            GenerateCustomers();
            ReadDataFromDataBase(new int[] { 0 });
            GenerateAreas();
            ReadDataFromDataBase(new int[] { 2 });
            GenerateAddresses();
            ReadDataFromDataBase(new int[] { 1, 3 });
            GenerateCounters();
            ReadDataFromDataBase(new int[] { 4 });
            GenerateReadings();
            RemoveDataFromStatic();
        }

        /// <summary>
        /// Usuwa wszystkie rekordy z wszystkich tabel.
        /// </summary>
        static public void ClearDataBase()
        {
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                string query = @"DELETE FROM Collector;" +
                                "DELETE FROM Customer;" +
                                "DELETE FROM Area;" +
                                "DELETE FROM Counter;" +
                                "DELETE FROM Address;" +
                                "DELETE FROM Reading;";

                dataBase.Database.ExecuteSqlCommand(query);
            }
        }

        /// <summary>
        /// Losuje jedenastocyfrową liczbę z odpowiedniego zakresu i sprawdza, czy ta liczba może być peselem.
        /// Jeśli jest to pesel, sprawdza, czy taki pesel nie istnieje już w tabeli, do której ma być wstawiony rekord z danym peselem.
        /// </summary>
        /// <param name="tableName">Nazwa tabeli, do której będzie wstawiony rekord z wylosowanym peselem.</param>
        /// <param name="gender">Płeć wylosowanej osoby.</param>
        /// <returns>Prawidłowy i unikalny w ramach odpowiedniej tabeli numer pesel.</returns>
        static string GenerateRandomPesel(string tableName, int gender)
        {
            Random rnd = new Random();
            IDataBase dataBase;
            string possiblePesel = String.Empty;
            string possibleYear;
            string possibleMonth;
            string possibleDay;
            bool uniquePesel = false;

            if (tableName == "Collector")
                dataBase = new Collectors();
            else
                dataBase = new Customers();

            while (!uniquePesel)
            {
                // Czyszczenie
                possiblePesel = String.Empty;
                possibleYear = String.Empty;
                possibleMonth = String.Empty;
                possibleDay = String.Empty;

                // Generowanie
                for (int i = 0; i < 2; ++i)
                    possibleYear += rnd.Next(0, 9).ToString();
                for (int i = 0; i < 2; ++i)
                    possibleMonth += rnd.Next(0, 9).ToString();
                for (int i = 0; i < 2; ++i)
                    possibleDay += rnd.Next(0, 9).ToString();

                // Sprawdzanie
                if (!IdentityValidation.CheckMonthId(possibleMonth))
                {
                    int tmp;
                    if (int.TryParse(possibleMonth, out tmp))
                        possibleMonth = (tmp + 1).ToString("00");
                }

                if (!IdentityValidation.CheckMonthId(possibleDay))
                {
                    int tmp;
                    if (int.TryParse(possibleDay, out tmp))
                        possibleDay = (tmp + 1).ToString("00");
                }

                // Łączenie
                possiblePesel = possibleYear + possibleMonth + possibleDay;

                // Numery kontrolne
                for (int i = 0; i < 3; ++i)
                    possiblePesel += rnd.Next(0, 9).ToString();

                // Płeć
                possiblePesel += gender.ToString();

                possiblePesel += IdentityValidation.CheckSum(possiblePesel);

                if (tableName == "Collector")
                    uniquePesel = !MainValidation.CollectorExists(possiblePesel);
                else
                    uniquePesel = !MainValidation.CustomerExists(possiblePesel);
            }
            return possiblePesel;
        }

        /// <summary>
        /// Losuje numer telefonu.
        /// </summary>
        /// <returns>Ciąg cyfr przedstawiający wylosowany numer telefonu.</returns>
        static string GeneratePhoneNumber()
        {
            Random rnd = new Random();

            string outputNumber = String.Empty;

            for (int i = 0; i < 9; ++i)
                outputNumber += rnd.Next(0, 9).ToString();

            return outputNumber;
        }

        /// <summary>
        /// Losuje płeć, a następnie imię i nazwisko z dostępnej puli.
        /// </summary>
        /// <param name="gender">Płeć wylosowanej osoby.</param>
        /// <returns>Dwuelementowa tablica zawierająca imię i nazwisko.</returns>
        static string[] GenerateNameAndLastName(int gender)
        {
            Random rnd = new Random();
            string[] nameAndLastName = new string[2];

            nameAndLastName[1] = SampleDataSource.lastNames[rnd.Next(0, SampleDataSource.lastNames.Length)];
            if (gender % 2 == 0)
                nameAndLastName[0] = SampleDataSource.maleNames[rnd.Next(0, SampleDataSource.maleNames.Length)];
            else
            {
                nameAndLastName[0] = SampleDataSource.femaleNames[rnd.Next(0, SampleDataSource.femaleNames.Length)];
                if (nameAndLastName[1][nameAndLastName[1].Length - 1] == 'i')
                    nameAndLastName[1] = nameAndLastName[1].Substring(0, nameAndLastName[1].Length - 1) + 'a';
            }

            return nameAndLastName;
        }

        /// <summary>
        /// Losuje kod pocztowy z dostępnej puli.
        /// </summary>
        /// <returns>Dwuelementowa tablica zawierająca kod pocztowy i odpowiadające mu miasto.</returns>
        static string[] GeneratePostalCodeAndCity()
        {
            Random rnd = new Random();
            string[] postalCodeAndCity = new string[2];
            int index = rnd.Next(0, SampleDataSource.postalCodes.Length);

            postalCodeAndCity[0] = SampleDataSource.postalCodes[index];
            postalCodeAndCity[1] = SampleDataSource.cities[index];

            return postalCodeAndCity;
        }

        /// <summary>
        /// Spośród identyfikatorów w podanej tabeli wybiera losowo jeden.
        /// </summary>
        /// <param name="choosen">Wybrana tabela,  z której ma być wylosowany klucz.</param>
        /// <returns>Wylosowany klucz.</returns>
        static string ChooseRandomId(int choosen)
        {
            Random rnd = new Random();

            return DataBase[choosen].itemList.ElementAt(rnd.Next(0, DataBase[choosen].itemList.Count))[0];
        }
    }
}