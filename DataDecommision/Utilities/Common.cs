using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Xml.Linq;

namespace DataDecommision
{
    public static class Common
    {

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
    }
}
