using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataDecommision
{
    public class XMLCreation
    {
        
        public static List<string> GetAHPScanData()
        {
            List<string> ahpScandata = new List<string>();
            try
            {
                foreach (var item in ScanDisplayVM.Instance.LstScanData)
                {
                    DateTime date = DateTime.ParseExact(item.Expdate, "yyyy-MM-dd", null);
                    string convertedString = date.ToString("yyyyMMdd").Substring(2);
                    ahpScandata.Add( "(01)"+ item.GTIN+ "(21)"+ item.SerialNumber + "(17)"+ convertedString + "(10)"+ item.LotNumber);
                }
                

            }
            catch (Exception)
            {

                throw;
            }
            return ahpScandata;
        }

        public static List<string> GetScanData()
        {
            List<string> normalScandata = new List<string>();
            try
            {
                foreach (var item in ScanDisplayVM.Instance.LstScanData)
                {
                    DateTime date = DateTime.ParseExact(item.Expdate, "yyyy-MM-dd", null);
                    string convertedString = date.ToString("yyyyMMdd").Substring(2);
                    normalScandata.Add("01" + item.GTIN + "21" + item.SerialNumber + "17" + convertedString + "10" + item.LotNumber);
                }

            }
            catch (Exception)
            {

                throw;
            }
            return normalScandata;
        }
        public static void CreateXML(List<string> scandata)
        {
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.Now;
            string formattedDateTime = currentDateTimeOffset.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");
            TimeSpan timeZoneOffset = currentDateTimeOffset.Offset;
            string sign = timeZoneOffset >= TimeSpan.Zero ? "+" : "-";
            TimeSpan absoluteOffset = timeZoneOffset.Duration();
            string formattedOffset = string.Format("{0}{1:hh\\:mm}", sign, absoluteOffset);
            // Create the XML document
            XmlDocument xmlDoc = new XmlDocument();

            // Create the root element <epcis:EPCISDocument>
            XmlElement rootElement = xmlDoc.CreateElement("epcis", "EPCISDocument", "urn:epcglobal:epcis:xsd:1");
            xmlDoc.AppendChild(rootElement);

            // Add namespaces
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceManager.AddNamespace("epcis", "urn:epcglobal:epcis:xsd:1");
            namespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            // Set attributes for root element
            rootElement.SetAttribute("xmlns:epcis", "urn:epcglobal:epcis:xsd:1");
            rootElement.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            rootElement.SetAttribute("creationDate", formattedDateTime);
            rootElement.SetAttribute("schemaVersion", "1");

            // Create the <epcis:EPCISBody> element
            XmlElement epcisBodyElement = xmlDoc.CreateElement("epcis", "EPCISBody", "urn:epcglobal:epcis:xsd:1");
            rootElement.AppendChild(epcisBodyElement);

            // Create the <epcis:EventList> element
            XmlElement eventListElement = xmlDoc.CreateElement("epcis", "EventList", "urn:epcglobal:epcis:xsd:1");
            epcisBodyElement.AppendChild(eventListElement);

            // Create the <epcis:ObjectEvent> element
            XmlElement objectEventElement = xmlDoc.CreateElement("epcis", "ObjectEvent", "urn:epcglobal:epcis:xsd:1");
            eventListElement.AppendChild(objectEventElement);

            // Create and append child elements under <epcis:ObjectEvent>
            objectEventElement.AppendChild(CreateElementWithText(xmlDoc, "epcis", "eventTime", formattedDateTime));
            objectEventElement.AppendChild(CreateElementWithText(xmlDoc, "epcis", "eventTimeZoneOffset", formattedOffset));

            // Create the <epcis:epcList> element
            XmlElement epcListElement = xmlDoc.CreateElement("epcis", "epcList", "urn:epcglobal:epcis:xsd:1");
            objectEventElement.AppendChild(epcListElement);

            // Add <epcis:epc> elements under <epcis:epcList>

            foreach (string epc in scandata)
            {
                XmlElement epcElement = xmlDoc.CreateElement("epcis", "epc", "urn:epcglobal:epcis:xsd:1");
                epcElement.InnerText = epc;
                epcListElement.AppendChild(epcElement);
            }

            // Create and append other elements under <epcis:ObjectEvent>
            objectEventElement.AppendChild(CreateElementWithText(xmlDoc, "epcis", "action", "ADD"));
            objectEventElement.AppendChild(CreateElementWithText(xmlDoc, "epcis", "bizStep", "urn:epcglobal:cbv:bizstep:commissioning"));
            objectEventElement.AppendChild(CreateElementWithText(xmlDoc, "epcis", "disposition", "urn:epcglobal:cbv:disp:active"));

            // Create the <epcis:readPoint> element
            XmlElement readPointElement = xmlDoc.CreateElement("epcis", "readPoint", "urn:epcglobal:epcis:xsd:1");
            objectEventElement.AppendChild(readPointElement);

            // Create and append <epcis:id> element under <epcis:readPoint>
            readPointElement.AppendChild(CreateElementWithText(xmlDoc, "epcis", "id", "urn:systechcitadel.com:device:sgln:101"));

            // Create the <epcis:bizLocation> element
            XmlElement bizLocationElement = xmlDoc.CreateElement("epcis", "bizLocation", "urn:epcglobal:epcis:xsd:1");
            objectEventElement.AppendChild(bizLocationElement);

            // Create and append <epcis:id> element under <epcis:bizLocation>
            bizLocationElement.AppendChild(CreateElementWithText(xmlDoc, "epcis", "id", "urn:epc:id:sgln:08662190003.0.0"));

            // Create the <epcis:extension> element
            XmlElement extensionElement = xmlDoc.CreateElement("epcis", "extension", "urn:epcglobal:epcis:xsd:1");
            objectEventElement.AppendChild(extensionElement);

            // Create and append <epcis:field> elements under <epcis:extension>
            string[] fieldNames = new string[]
            {
            "Lot Number (Bulk)",
            "Expiration Date",
              "Lot Number (RePacked)"
            };

            string[] fieldValues = new string[]
            {
            DecomData.BulkLot,
            DecomData.RepackExp,
            DecomData.RepackLot
                // Add more field values here...
            };

            for (int i = 0; i < fieldNames.Length; i++)
            {
                XmlElement fieldElement = xmlDoc.CreateElement("epcis", "field", "urn:epcglobal:epcis:xsd:1");
                fieldElement.SetAttribute("name", fieldNames[i]);
                fieldElement.SetAttribute("value", fieldValues[i]);
                extensionElement.AppendChild(fieldElement);
            }
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string xmlFileName = DecomData.BulkLot + "-" + DecomData.RepackLot+".xml";

            // Check if the file already exists
            //if (File.Exists(Path.Combine(documentsPath, xmlFileName)))
            //{
            //    // Delete the existing file
            //    File.Delete(Path.Combine(documentsPath, xmlFileName));
            //}
            xmlDoc.Save(Path.Combine(documentsPath, xmlFileName));
            
           
        }

        // Helper method to create an element with text content
        static XmlElement CreateElementWithText(XmlDocument xmlDoc, string prefix, string localName, string textContent)
        {
            XmlElement element = xmlDoc.CreateElement(prefix, localName, "urn:epcglobal:epcis:xsd:1");
            element.InnerText = textContent;
            return element;
        }
    }





}
