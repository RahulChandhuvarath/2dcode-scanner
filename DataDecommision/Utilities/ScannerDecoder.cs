using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing;

using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Threading;
using System.Runtime.Remoting.Contexts;
using System.Windows.Markup;

namespace DataDecommision
{

    public class ScannerDecoder
    {

        public static string userSelectedPort = "";
        //Text of the scanned 2d Code
        private static string scantext = "";
        /// <summary>
        /// Scans and decodes the string
        /// </summary>
        /// <returns>gtin,lot,EXP,Serial</returns>
        public static (string,string,string,string) ScanAndDecode()
        {
            scantext = "";
            // find the first available serial port that has a barcode scanner attached
            SerialPort scannerPort = FindBarcodeScanner(userSelectedPort);
            if (scannerPort != null)
            {
                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(1000);
                    if(scantext != "")
                    {

                        return DecodeString(scantext);
                    }
                    
                }
               
            }
            return ("", "", "", "");
        }
           

        
        /// <summary>
        /// Open all the serial ports and register the event
        /// </summary>
        /// <returns></returns>
        static SerialPort FindBarcodeScanner(string port)
        {
            // get a list of all available serial ports
            List<string> portNames = new List<string>
            {
                port
            };

            SerialPort scannerPort = null;
                ;
            // configure the serial port settings for the scanner

            if (portNames.Count() >= 1)
            {
                scannerPort = new SerialPort(portNames[0]);
                try
                {
                    scannerPort.Open();
                    scannerPort.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived1);
                }
                catch
                {
                    // ignore any exceptions that occur when opening the port or communicating with the scanner
                }
                finally
                {
                    //scannerPort.Close();
                }
            }
            if (portNames.Count() >= 2)
            {
                scannerPort = new SerialPort(portNames[1]);
                try
                {
                    scannerPort.Open();
                    scannerPort.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived2);
                }
                catch
                {
                    // ignore any exceptions that occur when opening the port or communicating with the scanner
                }
                finally
                {
                    //scannerPort.Close();
                }
            }
            if (portNames.Count() >= 3)
            {
                scannerPort = new SerialPort(portNames[2]);
                try
                {
                    scannerPort.Open();
                    scannerPort.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived3);
                }
                catch
                {
                    // ignore any exceptions that occur when opening the port or communicating with the scanner
                }
                finally
                {
                    //scannerPort.Close();
                }
            }
            if (portNames.Count() >= 4)
            {
                scannerPort = new SerialPort(portNames[3]);
                try
                {
                    scannerPort.Open();
                    scannerPort.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived4);
                }
                catch
                {
                    // ignore any exceptions that occur when opening the port or communicating with the scanner
                }
                finally
                {
                    //scannerPort.Close();
                }
            }



            // no barcode scanner was found on any of the available ports
            return scannerPort;
        }


        /// <summary>
        /// Decodes the scanned 2d String gtin,lot,EXP,Serial
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static (String,string,string,string) DecodeString(string inputString)
        {

            string pattern = "[^a-zA-Z0-9]+";
            string value =  Regex.Replace(inputString, pattern, "");

            string gtin = null, serialNumber = null, expirationDate = null, lotNumber = null;

            if (value.StartsWith("01"))
            {
                gtin = value.Substring(2, 14);
                value = value.Remove(0, 16);
            }
            int startindex17 =0;
            int length = value.Length;
            if (value.StartsWith("21") || value.StartsWith("10"))
            {
               
                do
                {

                    int index = value.IndexOf("17", startindex17);
                    if (startindex17 == index)
                    {
                        break;
                    }
                    else
                    {
                        startindex17 = index;
                    }

                    if(startindex17 > 0)
                    {
                        if (length > startindex17 + 10)
                        {
                           
                            if (value.StartsWith("21") || value.StartsWith("10"))
                            {
                                expirationDate = value.Substring(startindex17 + 2, 6);
                                break;
                            }
                        }
                        else if(length == startindex17 + 8)
                        {
                            expirationDate = value.Substring(startindex17 + 2, 6);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }

                } while (startindex17 >= 0);
                
            }
            if(startindex17 != 0)
            {
                if(value.StartsWith("21"))
                {
                    serialNumber = value.Substring(2, startindex17-2);
                }
                else if (value.StartsWith("10"))
                {
                    lotNumber = value.Substring(2, startindex17-2);
                }

                if (length > startindex17 + 10)
                {
                    if (value.Substring(startindex17 + 8, 2) == "21")
                    {
                        serialNumber = value.Substring(startindex17 + 10);
                    }
                    else if (value.Substring(startindex17 + 8, 2) == "10")
                    {
                        lotNumber = value.Substring(startindex17 + 10);
                    }
                }
            }

            if(startindex17 == 0)
            {
                if (value.StartsWith("17"))
                {
                    expirationDate = value.Substring(2, 6);
                }
                if (value.Substring(startindex17 + 8, 2) == "21")
                {
                    serialNumber = value.Substring(startindex17 + 10);
                }
                else if (value.Substring(startindex17 + 8, 2) == "10")
                {
                    lotNumber = value.Substring(startindex17 + 10);
                }
            }
            return (gtin, lotNumber, expirationDate, serialNumber);
        }


        // create a ManualResetEvent
        static readonly ManualResetEvent dataReceivedtemp = new ManualResetEvent(false);



        // wait for the data to be received or for 30 seconds to elapse
        readonly bool dataReceivedOrTimeout = dataReceivedtemp.WaitOne(30000);


       
        static void OnDataReceived1(object sender, SerialDataReceivedEventArgs e)
        {
            dataReceivedtemp.Set();
            // read the data from the serial port buffer
            SerialPort scannerPort = (SerialPort)sender;
            scantext = scannerPort.ReadExisting();
           
        }

        static void OnDataReceived2(object sender, SerialDataReceivedEventArgs e)
        {
            dataReceivedtemp.Set();
            // read the data from the serial port buffer
            SerialPort scannerPort = (SerialPort)sender;
            scantext = scannerPort.ReadExisting();

        }

        static void OnDataReceived3(object sender, SerialDataReceivedEventArgs e)
        {
            dataReceivedtemp.Set();
            // read the data from the serial port buffer
            SerialPort scannerPort = (SerialPort)sender;
            scantext = scannerPort.ReadExisting();

        }

        static void OnDataReceived4(object sender, SerialDataReceivedEventArgs e)
        {
            dataReceivedtemp.Set();
            // read the data from the serial port buffer
            SerialPort scannerPort = (SerialPort)sender;
            scantext = scannerPort.ReadExisting();

        }
    }


}
