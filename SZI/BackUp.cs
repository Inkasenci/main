using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZI
{
    class BackUp
    {

        private String AddressBackUp(System.Guid AddressId, int HouseNo, Nullable<int> FlatNo, System.Guid AreaId)
        {
            return "INSERT INTO Address ( AddressId, HouseNo, FlatNo, AreaId ) VALUES ('" + AddressId.ToString() + "', '" +
                HouseNo.ToString() + "', '" + FlatNo.ToString() + "', '" + AreaId.ToString() + "');";
        }

        private String AdressTableBackUp()
        {
            return "-- CREATE TABLE Address" + System.Environment.NewLine + 
                "-- (" + System.Environment.NewLine +
                "-- AddressId UNIQUEIDENTIFIER PRIMARY KEY," + System.Environment.NewLine +
                "-- HouseNo INT NOT NULL," + System.Environment.NewLine +
                "-- FlatNo INT NULL," + System.Environment.NewLine +
                "-- AreaId UNIQUEIDENTIFIER NOT NULL" + System.Environment.NewLine +
                "-- )" + System.Environment.NewLine;
        }

        private String AreaBackUp(System.Guid AreaId, string Street, string CollectorId)
        {
            return "INSERT INTO Area ( AreaId, Street, CollectorId ) VALUES ('" + AreaId.ToString() + "', '" +
                Street + "', '" + CollectorId + "');";
        }

        private String AreaTableBackUp()
        {
            return "-- CREATE TABLE Area" + System.Environment.NewLine +
                "-- (" + System.Environment.NewLine +
                "-- AreaId UNIQUEIDENTIFIER PRIMARY KEY," + System.Environment.NewLine +
                "-- Street NVARCHAR(32) NOT NULL," + System.Environment.NewLine +
                "-- CollectorId NVARCHAR(11) NULL" + System.Environment.NewLine +
                "-- )" + System.Environment.NewLine;
        }

        private String CollectorBackUp(string CollectorId, string Name, string LastName, string PostalCode, string City, string Address, string PhoneNumber)
        {
            return "INSERT INTO Collector ( CollectorId, Name, LastName, PostalCode, City, Address, PhoneNumber ) VALUES ('" + CollectorId + "', '" +
                Name + "', '" + LastName + "', '" + PostalCode + "', '" + City + "', '" + Address + "', '" + PhoneNumber + "');";
        }

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

        private String CounterBackUp(int CounterNo, int CircuitNo, Nullable<System.Guid> AddressId, string CustomerId)
        {
            return "INSERT INTO Counter ( CounterNo, CircuitNo, AddressId, CustomerId ) VALUES ('" + CounterNo.ToString() + "', '" +
                CircuitNo.ToString() + "', '" + AddressId.ToString() + "', '" + CustomerId + "');";
        }

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

        private String CustomerBackUp(string CustomerId, string Name, string LastName, string PostalCode, string City, string Address, string PhoneNumber)
        {
            return "INSERT INTO Customer ( CustomerId, Name, LastName, PostalCode, City, Address, PhoneNumber ) VALUES ('" + CustomerId + "', '" +
                Name + "', '" + LastName + "', '" + PostalCode + "', '" + City + "', '" + Address + "', '" + PhoneNumber + "');";
        }

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

        private String ReadingBackUp(System.Guid ReadingId, System.DateTime Date, double Value, string CollectorId, int CounterNo)
        {
            return "INSERT INTO Reading ( ReadingId, Date, Value, CollectorId, CounterNo ) VALUES ('" + ReadingId.ToString() + "', '" +
                Date.ToString() + "', '" + Value.ToString() + "', '" + CollectorId + "', '" + CounterNo.ToString() + "');";
        }

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

        private String TableCommentBackUp(string tableName)
        {
            return  System.Environment.NewLine + "-- Dane zawarte w tabeli: " + tableName + System.Environment.NewLine;
        }

        public bool GenerateBackUp()
        {
            using (StreamWriter file = new StreamWriter(@"BackUp.txt"))
            {
                using (var dataBase = new CollectorsManagementSystemEntities())
                {
                    {
                        file.WriteLine(TableCommentBackUp("Address"));
                        file.WriteLine(AdressTableBackUp());
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
            return true;
        }

        public bool RestoreBackUp()
        {
            using (StreamReader file = new StreamReader(@"BackUp.txt"))
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
