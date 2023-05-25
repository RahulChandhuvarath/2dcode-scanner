using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Xml.Linq;
using Microsoft.Office.Interop.Excel;

namespace DataDecommision
{
    public static class Common
    {

        public static Dictionary<string, string> loginCredentials = new Dictionary<string, string>();
        public static Dictionary<string, string> GetCredentials()
        {
            Dictionary<string, string> loginDictionary = new Dictionary<string, string>();

            string assemblyDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(assemblyDirectory, "LoginCredentials.config");

            // read all lines of the configuration file
            string[] lines = File.ReadAllLines(filePath);

            // split each line into name and number pairs and add them to the dictionary
            foreach (string line in lines)
            {
                string[] credentials = line.Split('\t').Select(s => s.Trim()).ToArray();

                if (credentials.Length == 2)
                {
                    if (!loginDictionary.Keys.Contains(credentials[0]))
                        loginDictionary.Add(credentials[0], credentials[1]); ;
                }
            }
            return loginDictionary;
        }

        public static Dictionary<string, string> GetCredentialsExcel()
        {
            Dictionary<string, string> loginDictionary = new Dictionary<string, string>();

            string assemblyDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(assemblyDirectory, "LoginCredentials.xlsx");
            string password = "12345";

            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Open(filePath, Password: password);

            Worksheet worksheet = workbook.Sheets[1]; // Assuming the data is on the first sheet

            Range range = worksheet.UsedRange;
            int rowCount = range.Rows.Count;
;

            for (int i = 1; i <= rowCount; i++)
            {
                string key = range.Cells[i, 1].Value.ToString();
                string value = range.Cells[i, 2].Value.ToString();

                // Store key-value pairs in the dictionary
                loginDictionary.Add(key, value);
            }

            // Close Excel workbook and release resources
            workbook.Close();
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            return loginDictionary;
        }
    }
}
