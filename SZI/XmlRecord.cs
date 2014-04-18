using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SZI
{
    /// <summary>
    /// Klasa obsługująca Model XML Odczytu - SZI.
    /// </summary>
    [Serializable()]
    public class CounterXML
    {
        #region XML Form
        /// <summary>
        /// Nr porządkowy odczytu.
        /// </summary>
        [XmlElement("ReadId")]
        public string ReadId { get; set; }
        /// <summary>
        /// Numer licznika.
        /// </summary>
        [XmlElement("CounterNo")]
        public string CounterNo { get; set; }
        /// <summary>
        /// Numer układu.
        /// </summary>
        [XmlElement("CircuitNo")]
        public string CircuitNo { get; set; }
        /// <summary>
        /// Inofrmacje na temat klienta.
        /// </summary>
        [XmlElement("Customer")]
        public string Customer { get; set; }
        /// <summary>
        /// Adres licznika.
        /// </summary>
        [XmlElement("Address")]
        public string Address { get; set; }
        /// <summary>
        /// Data ostatniego odczytu.
        /// </summary>
        [XmlElement("LastReadDate")]
        public string LastReadDate { get; set; }
        /// <summary>
        /// Ostatnio wykonujący odczyt Inkasent.
        /// </summary>
        public string LastCollector { get; set; }
        /// <summary>
        /// Wartość ostatniego odczytu.
        /// </summary>
        [XmlElement("LastValue")]
        public string LastValue { get; set; }
        /// <summary>
        /// Nowa wartość.
        /// </summary>
        [XmlElement("NewValue")]
        public string NewValue { get; set; }
        #endregion
        #region Custom Fun 
        /// <summary>
        /// Tablica wartości zapisywanych do pliku.
        /// </summary>
        public string[] StringArray
        {
            get
            {
                return new string[]
                {
                    ReadId,
                    CounterNo,
                    CircuitNo,
                    Customer,
                    Address,
                    LastReadDate,
                    LastValue,
                    NewValue
                };
            }
        }

        /// <summary>
        /// Tablica wartości wyświetlanych na ekranie.
        /// </summary>
        public string[] PrintStringArray
        {
            get
            {
                return new string[]
                {
                    ReadId,
                    CounterNo,
                    CircuitNo,
                    ( Customer != String.Empty ) ? Customer : LangPL.CountersWarnings["noRecord"],
                    Address,
                    ( LastReadDate != String.Empty ) ? LastReadDate : LangPL.CountersWarnings["noRecord"],
                    ( LastCollector != String.Empty ) ? LastCollector : LangPL.CountersWarnings["noRecord"],
                    ( LastValue != String.Empty ) ? LastValue : LangPL.CountersWarnings["noRecord"]
                };
            }
        }
        #endregion
    }
    /// <summary>
    /// Klasa zawierająca informacje dotyczące kolejnych odczytów.
    /// </summary>
    [Serializable()]
    [XmlRoot("Counters")]
    public class CountersCollection
    {
        #region XML Form
        /// <summary>
        /// Id Inkasenta wykonującego dane odczyty.
        /// </summary>
        [XmlAttribute("CollectorID")]
        public string collectorId { get; set; }
        /// <summary>
        /// Lista liczników.
        /// </summary>
        [XmlElement("Counter")]
        public List<CounterXML> counter { get; set; }
        #endregion
        #region Custom Fun
        /// <summary>
        /// Zwraca ilość liczników w bazie.
        /// </summary>
        public int RecordsCount
        {
            get
            {
                return (this.counter != null) ? this.counter.Count() : 0;
            }
        }

        /// <summary>
        /// Dodanie nowego elementu do listy elementów XML.
        /// </summary>
        /// <param name="item">Element, który ma być dodany do listy.</param>
        public void AddNewElement(CounterXML item)
        {
            if (counter == null)
                counter = new List<CounterXML>();
            counter.Add(item);
        }

        /// <summary>
        /// Zapis wartości do bazy danych.
        /// </summary>
        public void AddNewElementsToDataBase()
        {
            foreach (var element in counter)
            {
                if (element.NewValue != String.Empty)
                {
                    Reading newRecord = new Reading();
                    newRecord.CollectorId = this.collectorId;
                    newRecord.Date = DateTime.Now;
                    newRecord.CounterNo = Convert.ToInt32(element.CounterNo);
                    newRecord.Value = Convert.ToDouble(element.NewValue);
                    newRecord.ReadingId = Guid.NewGuid();
                    newRecord.InsertIntoDB();
                }
            }
        }
        #endregion
    }
}