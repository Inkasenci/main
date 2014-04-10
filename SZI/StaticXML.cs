using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace SZI
{
    static class StaticXML
    {
        static public void XMLExport( string output, string collectorId, List<string[]> itemList )
        {
            string strRegex = @"([a-zA-Z0-9]*)\.xml";
            XmlTextWriter XmlWriter = new XmlTextWriter(Regex.IsMatch(output, strRegex, RegexOptions.None) ? output : output+".xml", null);
            XmlWriter.WriteStartDocument();
            XmlWriter.WriteComment("XML wygenerowany przez aplikację SZI.");
            XmlWriter.WriteStartElement("Counters");
            XmlWriter.WriteAttributeString("CollectorID", collectorId);

            foreach (var element in itemList)
            {
                XmlWriter.WriteStartElement("Counter");
                XmlWriter.WriteElementString("ReadId", element[0]);
                XmlWriter.WriteElementString("CounterNo", element[1]);
                XmlWriter.WriteElementString("CircuitNo", element[2]);
                XmlWriter.WriteElementString("Address", element[3]);
                XmlWriter.WriteElementString("LastRead", (element[4] != "Brak danych!") ? element[4] : String.Empty);
                XmlWriter.WriteElementString("NewRead", String.Empty);
                XmlWriter.WriteEndElement();
            }

            XmlWriter.WriteEndDocument();
            XmlWriter.Close();  
        }

        static public void XMLImport(string output)
        {
            Reading newRecord = new Reading();
            string collectorId = String.Empty;
            string strRegex = @"([a-zA-Z0-9]*)\.xml";
            if (Regex.IsMatch(output, strRegex, RegexOptions.None))
            {
                XmlTextReader reader = new XmlTextReader(output);
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "Counters":
                                collectorId = reader.GetAttribute("CollectorID");
                                break;
                            case "ReadId":
                                newRecord = new Reading();
                                newRecord.Date = DateTime.MinValue;
                                break;
                            case "CounterNo":
                                newRecord.CounterNo = Convert.ToInt32(reader.ReadString());
                                break;
                            case "CircuitNo":
                                break;
                            case "Address":
                                break;
                            case "LastRead":
                                string lastRead = reader.ReadString();
                                if (lastRead == String.Empty)
                                    newRecord.Date = DateTime.ParseExact("2001-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                else
                                    newRecord.Date = DateTime.ParseExact(lastRead, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                break;
                            case "NewRead":
                                string newRead = reader.ReadString();
                                if (newRead != String.Empty)
                                {
                                    newRecord.Date = DateTime.Now;
                                    newRecord.Value = Convert.ToDouble(newRead);
                                }
                                else
                                    newRecord.Value = 0;

                                newRecord.ReadingId = Guid.NewGuid();
                                newRecord.CollectorId = collectorId;
                                newRecord.InsertIntoDB();
                                break;
                        }
                    }

                }
            }
        }
    }
}
