using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;
using Microsoft.Office.Interop.Access.Dao;
using ADOX;

namespace DataDecommision
{
    internal static class AccessOperation
    {

        public static void CreateDatabase()
        {
            try
            {
                string userName = DecomData.UserName;
                string formattedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string fileName = $"Decom_ScanData.accdb";
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);

                string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};";

                bool fileExists = File.Exists(filePath);
                if (!fileExists)
                {
                    ADOX.Catalog catalog = new ADOX.Catalog();
                    catalog.Create($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Jet OLEDB:Engine Type=5");
                }


                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    if (!fileExists)
                    {
                        string createTableQuery = "CREATE TABLE ScanDetails ([USER] TEXT,[DATE_TIME] TEXT, [SCAN_STRING] TEXT);";
                        using (OleDbCommand createTableCommand = new OleDbCommand(createTableQuery, connection))
                        {

                            createTableCommand.ExecuteNonQuery();
                        }
                    }

                    string insertDataQuery = "INSERT INTO ScanDetails ([USER], [DATE_TIME], [SCAN_STRING]) VALUES (?,?,?);";
                    using (OleDbCommand insertDataCommand = new OleDbCommand(insertDataQuery, connection))
                    {

                        List<string> scanData = XMLCreation.GetScanData();

                        // Insert each row into the table
                        for (int i = 0; i < scanData.Count; i++)
                        {
                            insertDataCommand.Parameters.Clear();
                            insertDataCommand.Parameters.AddWithValue("USER", userName);
                            insertDataCommand.Parameters.AddWithValue("DATE_TIME", formattedTime);
                            insertDataCommand.Parameters.AddWithValue("SCAN_STRING", scanData[i]);
                            insertDataCommand.ExecuteNonQuery();
                        }
                       
                    }

                    connection.Close();
                }
            }
            catch(Exception)
            {

            }

        }
    }
}
