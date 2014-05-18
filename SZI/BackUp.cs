using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    /// <summary>
    /// Klasa pozwalająca na generowanie BackUp-u bazy danych.
    /// </summary>
    class BackUp
    {
        /// <summary>
        /// Funkcja odpowiedzialna za generowanie zapytania SQL na podstawie rekordu z tabeli Address.
        /// <param name="AddressId">Id adresu.</param>
        /// <param name="AreaId">Id terenu.</param>
        /// <param name="FlatNo">Numer mieszkania.</param>
        /// <param name="HouseNo">Numer domu.</param>
        /// <returns>Zwraca rekord w postaci zapytania SQL.</returns>
        /// </summary>
        private String AddressBackUp(System.Guid AddressId, int HouseNo, Nullable<int> FlatNo, System.Guid AreaId)
        {
            return "INSERT INTO Address ( AddressId, HouseNo, FlatNo, AreaId ) VALUES ('" + AddressId.ToString() + "', '" +
                HouseNo.ToString() + "', " + ((FlatNo != null) ? "'" + FlatNo.ToString() + "'" : "NULL") + ", '" + AreaId.ToString() + "');";
        }

        /// <summary>
        /// Funkcja odpowiedzialna za generowanie zapytania SQL tworzącego tabelę Address.
        /// </summary>
        /// <returns>Zapytanie SQL tworzące tabelę Address.</returns>
        private String AddressTableBackUp()
        {
            return "-- CREATE TABLE Address" + System.Environment.NewLine + 
                "-- (" + System.Environment.NewLine +
                "-- AddressId UNIQUEIDENTIFIER PRIMARY KEY," + System.Environment.NewLine +
                "-- HouseNo INT NOT NULL," + System.Environment.NewLine +
                "-- FlatNo INT NULL," + System.Environment.NewLine +
                "-- AreaId UNIQUEIDENTIFIER NOT NULL" + System.Environment.NewLine +
                "-- )" + System.Environment.NewLine;
        }

        /// <summary>
        /// Funkcja odpowiedzialna za generowanie zapytania SQL na podstawie rekordu z tabeli Area.
        /// </summary>
        /// <param name="AreaId">Id terenu.</param>
        /// <param name="Street">Ulica.</param>
        /// <param name="CollectorId">Id Inkasenta.</param>
        /// <returns>Zwraca rekord w postaci zapytania SQL.</returns>
        private String AreaBackUp(System.Guid AreaId, string Street, string CollectorId)
        {
            return "INSERT INTO Area ( AreaId, Street, CollectorId ) VALUES ('" + AreaId.ToString() + "', '" +
                Street + "', " + ((CollectorId != null) ? "'" + CollectorId + "'" : "NULL") + ");";
        }

        /// <summary>
        /// Funkcja odpowiedzialna za generowanie zapytania SQL tworzącego tabelę Area.
        /// </summary>
        /// <returns>Zapytanie SQL tworzące tabelę Area.</returns>
        private String AreaTableBackUp()
        {
            return "-- CREATE TABLE Area" + System.Environment.NewLine +
                "-- (" + System.Environment.NewLine +
                "-- AreaId UNIQUEIDENTIFIER PRIMARY KEY," + System.Environment.NewLine +
                "-- Street NVARCHAR(32) NOT NULL," + System.Environment.NewLine +
                "-- CollectorId NVARCHAR(11) NULL" + System.Environment.NewLine +
                "-- )" + System.Environment.NewLine;
        }

        /// <summary>
        /// Funkcja odpowiedzialna za generowanie zapytania SQL na podstawie rekordu z tabeli Collector.
        /// </summary>
        /// <param name="CollectorId">Id Inkasenta.</param>
        /// <param name="Name">Imię.</param>
        /// <param name="LastName">Nazwisko.</param>
        /// <param name="PostalCode">Kod pocztowy.</param>
        /// <param name="City">Miasto.</param>
        /// <param name="Address">Adres.</param>
        /// <param name="PhoneNumber">Telefon kontaktowy.</param>
        /// <returns>Zwraca rekord w postaci zapytania SQL.</returns>
        private String CollectorBackUp(string CollectorId, string Name, string LastName, string PostalCode, string City, string Address, string PhoneNumber)
        {
            return "INSERT INTO Collector ( CollectorId, Name, LastName, PostalCode, City, Address, PhoneNumber ) VALUES ('" + CollectorId + "', '" +
                Name + "', '" + LastName + "', '" + PostalCode + "', '" + City + "', '" + Address + "', '" + PhoneNumber + "');";
        }

        /// <summary>
        /// Funkcja odpowiedzialna za generowanie zapytania SQL tworzącego tabelę Collector.
        /// </summary>
        /// <returns>Zapytanie SQL tworzące tabelę Collector.</returns>
        private String CollectorTableBackUp()
        {
            return "-- CREATE TABLE Collector" + System.Environment.NewLine +
                "-- (" + System.Environment.NewLine +
                "-- CollectorId NVARCHAR(11) PRIMARY KEY," + System.Environment.NewLine +
                "-- Name NVARCHAR(32) NOT NULL," + System.Environment.NewLine +
                "-- LastName NVARCHAR(64) NOT NULL," + System.Environment.NewLine +
                "-- PostalCode NVARCHAR(5) NOT NULL," + System.Environment.NewLine +
                "-- City NVARCHAR(32) NOT NULL," + System.Environment.NewLine +
                "-- Address NVARCHAR(32) NOT NULL," + System.Environment.NewLine +
                "-- PhoneNumber NVARCHAR(9) NOT NULL" + System.Environment.NewLine +
                "-- )" + System.Environment.NewLine;
        }

        /// <summary>
        /// Funkcja odpowiedzialna za generowanie zapytania SQL na podstawie rekordu z tabeli Counter.
        /// </summary>
        /// <param name="CounterNo">Numer licznika.</param>
        /// <param name="CircuitNo">Numer układu.</param>
        /// <param name="AddressId">Id Adresu.</param>
        /// <param name="CustomerId">Id Klienta.</param>
        /// <returns>Zwraca rekord w postaci zapytania SQL.</returns>
        private String CounterBackUp(int CounterNo, int CircuitNo, Nullable<System.Guid> AddressId, string CustomerId)
        {
            return "INSERT INTO Counter ( CounterNo, CircuitNo, AddressId, CustomerId ) VALUES ('" + CounterNo.ToString() + "', '" +
                CircuitNo.ToString() + "', " + ((AddressId != null) ? "'" + AddressId.ToString() + "'" : "NULL") + ", " + ((CustomerId != null) ? "'" + CustomerId + "'" : "NULL") + ");";
        }

        /// <summary>
        /// Funkcja odpowiedzialna za generowanie zapytania SQL tworzącego tabelę Counter.
        /// </summary>
        /// <returns>Zapytanie SQL tworzące tabelę Counter.</returns>
        private String CounterTableBackUp()
        {
            return "-- CREATE TABLE Counter" + System.Environment.NewLine +
                "-- (" + System.Environment.NewLine +
                "-- CounterNo INT PRIMARY KEY," + System.Environment.NewLine +
                "-- CircuitNo INT NOT NULL," + System.Environment.NewLine +
                "-- AddressId UNIQUEIDENTIFIER NULL," + System.Environment.NewLine +
                "-- CustomerId NVARCHAR(11) NULL" + System.Environment.NewLine +
                "-- )" + System.Environment.NewLine;
        }

        /// <summary>
        /// Funkcja odpowiedzialna za generowanie zapytania SQL na podstawie rekordu z tabeli Customer.
        /// </summary>
        /// <param name="CustomerId">Id Inkasenta.</param>
        /// <param name="Name">Imię.</param>
        /// <param name="LastName">Nazwisko.</param>
        /// <param name="PostalCode">Kod pocztowy.</param>
        /// <param name="City">Miasto.</param>
        /// <param name="Address">Adres.</param>
        /// <param name="PhoneNumber">Telefon kontaktowy.</param>
        /// <returns>Zwraca rekord w postaci zapytania SQL.</returns>
        private String CustomerBackUp(string CustomerId, string Name, string LastName, string PostalCode, string City, string Address, string PhoneNumber)
        {
            return "INSERT INTO Customer ( CustomerId, Name, LastName, PostalCode, City, Address, PhoneNumber ) VALUES ('" + CustomerId + "', '" +
                Name + "', '" + LastName + "', '" + PostalCode + "', '" + City + "', '" + Address + "', '" + PhoneNumber + "');";
        }

        /// <summary>
        /// Funkcja odpowiedzialna za generowanie zapytania SQL tworzącego tabelę Customer.
        /// </summary>
        /// <returns>Zapytanie SQL tworzące tabelę Customer.</returns>
        private String CustomerTableBackUp()
        {
            return "-- CREATE TABLE Customer" + System.Environment.NewLine +
                "-- (" + System.Environment.NewLine +
                "-- CustomerId NVARCHAR(11) PRIMARY KEY," + System.Environment.NewLine +
                "-- Name NVARCHAR(32) NOT NULL," + System.Environment.NewLine +
                "-- LastName NVARCHAR(64) NOT NULL," + System.Environment.NewLine +
                "-- PostalCode NVARCHAR(5) NOT NULL," + System.Environment.NewLine +
                "-- City NVARCHAR(32) NOT NULL," + System.Environment.NewLine +
                "-- Address NVARCHAR(32) NOT NULL," + System.Environment.NewLine +
                "-- PhoneNumber NVARCHAR(9) NOT NULL" + System.Environment.NewLine +
                "-- )" + System.Environment.NewLine;
        }

        /// <summary>
        /// Funkcja odpowiedzialna za generowanie zapytania SQL na podstawie rekordu z tabeli Reading.
        /// </summary>
        /// <param name="ReadingId">Id Odczytu.</param>
        /// <param name="Date">Data.</param>
        /// <param name="Value">Wartoć odczytu.</param>
        /// <param name="CollectorId">Id Inkasenta.</param>
        /// <param name="CounterNo">Numer licznika.</param>
        /// <returns></returns>
        private String ReadingBackUp(System.Guid ReadingId, System.DateTime Date, double Value, string CollectorId, int CounterNo)
        {
            return "INSERT INTO Reading ( ReadingId, Date, Value, CollectorId, CounterNo ) VALUES ('" + ReadingId.ToString() + "', '" +
                Date.ToString() + "', '" + Value.ToString() + "', '" + CollectorId + "', '" + CounterNo.ToString() + "');";
        }

        /// <summary>
        /// Funkcja odpowiedzialna za generowanie zapytania SQL tworzącego tabelę Reading.
        /// </summary>
        /// <returns>Zapytanie SQL tworzące tabelę Reading.</returns>
        private String ReadingTableBackUp()
        {
            return "-- CREATE TABLE Reading" + System.Environment.NewLine +
                "-- (" + System.Environment.NewLine +
                "-- ReadingId UNIQUEIDENTIFIER PRIMARY KEY," + System.Environment.NewLine +
                "-- Date DATETIME NOT NULL," + System.Environment.NewLine +
                "-- Value FLOAT NOT NULL," + System.Environment.NewLine +
                "-- CollectorId NVARCHAR(11) NOT NULL," + System.Environment.NewLine +
                "-- CounterNo INT NOT NULL" + System.Environment.NewLine +
                "-- )" + System.Environment.NewLine;
        }

        /// <summary>
        /// Funkcja odpowiedzialna za generowanie zapytania SQL czyszczącego wszystkie tabele.
        /// </summary>
        /// <returns>Zapytanie SQL czyszczące tabele.</returns>
        private String ClearTable()
        {
            return "-- Czyszczenie tabel" + System.Environment.NewLine + 
                "DELETE FROM Address;" + System.Environment.NewLine +
                "DELETE FROM Area;" + System.Environment.NewLine +
                "DELETE FROM Collector;" + System.Environment.NewLine +
                "DELETE FROM Counter;" + System.Environment.NewLine +
                "DELETE FROM Customer;" + System.Environment.NewLine +
                "DELETE FROM Reading;";
        }

        /// <summary>
        /// Zwracany komentarz informujący o aktualnie generowanym BackUp-ie ( tabeli na której zostaje wykonany zapis ).
        /// </summary>
        /// <param name="tableName">Nazwa tabeli.</param>
        /// <returns>Komentarz SQL.</returns>
        private String TableCommentBackUp(string tableName)
        {
            return  System.Environment.NewLine + "-- Dane zawarte w tabeli: " + tableName + System.Environment.NewLine;
        }

        /// <summary>
        /// Generowanie pliku z BackUp-em bazy danych SZI.
        /// </summary>
        /// <returns>Informacje o poprawności utworzenia pliku.</returns>
        public bool GenerateBackUp(string filePath)
        {
            using (StreamWriter file = new StreamWriter(filePath))
            {
                using (var dataBase = new CollectorsManagementSystemEntities())
                {
                    {
                        file.WriteLine(ClearTable());
                    }
                    {
                        file.WriteLine(TableCommentBackUp("Address"));
                        file.WriteLine(AddressTableBackUp());
                        foreach (var element in dataBase.Addresses)
                            file.WriteLine(AddressBackUp(element.AddressId, element.HouseNo, element.FlatNo, element.AreaId));
                    }
                    {
                        file.WriteLine(TableCommentBackUp("Area"));
                        file.WriteLine(AreaTableBackUp());
                        foreach (var element in dataBase.Areas)
                            file.WriteLine(AreaBackUp(element.AreaId, element.Street, element.CollectorId));
                    }
                    {
                        file.WriteLine(TableCommentBackUp("Collector"));
                        file.WriteLine(CollectorTableBackUp());
                        foreach (var element in dataBase.Collectors)
                            file.WriteLine(CollectorBackUp(element.CollectorId, element.Name, element.LastName,
                                element.PostalCode, element.City, element.Address, element.PhoneNumber));
                    }
                    {
                        file.WriteLine(TableCommentBackUp("Counter"));
                        file.WriteLine(CounterTableBackUp());
                        foreach (var element in dataBase.Counters)
                            file.WriteLine(CounterBackUp(element.CounterNo, element.CircuitNo, element.AddressId, element.CustomerId));
                    }
                    {
                        file.WriteLine(TableCommentBackUp("Customer"));
                        file.WriteLine(CustomerTableBackUp());
                        foreach (var element in dataBase.Customers)
                            file.WriteLine(CustomerBackUp(element.CustomerId, element.Name, element.LastName,
                                element.PostalCode, element.City, element.Address, element.PhoneNumber));
                    }
                    {
                        file.WriteLine(TableCommentBackUp("Reading"));
                        file.WriteLine(ReadingTableBackUp());
                        foreach (var element in dataBase.Readings)
                            file.WriteLine(ReadingBackUp(element.ReadingId, element.Date, element.Value, element.CollectorId, element.CounterNo));
                    }
                }
            }

            if (File.Exists(filePath))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Wczytywanie pliku z BackUp-em bazy danych SZI.
        /// </summary>
        /// <returns>Informacje o poprawności wczytania pliku.</returns>
        public bool RestoreBackUp(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            using (StreamReader file = new StreamReader(filePath))
            {
                string fileRead = file.ReadToEnd();
                using (var dataBase = new CollectorsManagementSystemEntities())
                {
                    dataBase.Database.ExecuteSqlCommand(fileRead);
                }
            }

            return true;
        }

    }
}
