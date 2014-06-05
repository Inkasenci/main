using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;


namespace SZI
{
    /// <summary>
    /// Interfejs łączący poszczególne tabele bazy danych.
    /// </summary>
    public interface IItem
    {

        /// <summary>
        /// Dodaje nowy rekord do tabeli.
        /// </summary>
        void InsertIntoDB();

        /// <summary>
        /// Pobranie elementu w formie tablicy string.
        /// </summary>
        string[] GetElements { get; }

        /// <summary>
        /// Modyfikuje odpowiedni rekord.
        /// </summary>
        /// <param name="id">Id modyfikowanego rekordu.</param>
        void ModifyRecord(string id);
    }
}
