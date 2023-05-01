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
using System.Windows.Threading;

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
        public static SerialPort FindBarcodeScanner(string port, bool continous =false)
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
                scannerPort = new SerialPort(portNames[0], 115200);
                scannerPort.Handshake = Handshake.RequestToSend;
                //scannerPort.Encoding = Encoding.UTF8;
                try
                {
                    scannerPort.Open();
                    if (continous)
                    {
                        //Thread thread = new Thread(() =>
                        //{
                        //    Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
                            scannerPort.DataReceived += new SerialDataReceivedEventHandler(OnDataReceivedConti);
                            //Thread.Sleep(50);
                        //});
                        //thread.Start();
                    }
                    else
                    {
                        scannerPort.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived1);
                    }
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
        public static (string, string, string, string) DecodeString(string inputString)
        {

            string pattern = "[^a-zA-Z0-9]+";
            string value = Regex.Replace(inputString, pattern, "");

            string gtin = null, serialNumber = null, expirationDate = null, lotNumber = null;

            if (value.StartsWith("01"))
            {
                gtin = value.Substring(2, 14);
                value = value.Remove(0, 16);
            }
            int startindex17 = 0;
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

                    if (startindex17 > 0)
                    {
                        if (length > startindex17 + 10)
                        {

                            if (value.StartsWith("21") || value.StartsWith("10"))
                            {
                                expirationDate = value.Substring(startindex17 + 2, 6);
                                break;
                            }
                        }
                        else if (length == startindex17 + 8)
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
            if (startindex17 != 0)
            {
                if (value.StartsWith("21"))
                {
                    serialNumber = value.Substring(2, startindex17 - 2);
                }
                else if (value.StartsWith("10"))
                {
                    lotNumber = value.Substring(2, startindex17 - 2);
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

            if (startindex17 == 0)
            {
                if (length >= startindex17 + 8)
                {
                    if (value.StartsWith("17"))
                    {
                        expirationDate = value.Substring(2, 6);
                    }
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
            return (gtin, lotNumber, expirationDate, serialNumber);
        }



        /// <summary>
        /// Decodes the scanned 2d String gtin,lot,EXP,Serial
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static void ContinousDecodeString(string inputString)
        {

            string pattern = "[^a-zA-Z0-9]+";
            string value = Regex.Replace(inputString, pattern, "");

            string gtin = null, serialNumber = null, expirationDate = null, lotNumber = null;

            if (value.StartsWith("01"))
            {
                gtin = value.Substring(2, 14);
                value = value.Remove(0, 16);
            }
            int startindex17 = 0;
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

                    if (startindex17 > 0)
                    {
                        if (length > startindex17 + 10)
                        {

                            if (value.StartsWith("21") || value.StartsWith("10"))
                            {
                                expirationDate = value.Substring(startindex17 + 2, 6);
                                break;
                            }
                        }
                        else if (length == startindex17 + 8)
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
            if (startindex17 != 0)
            {
                if (value.StartsWith("21"))
                {
                    serialNumber = value.Substring(2, startindex17 - 2);
                }
                else if (value.StartsWith("10"))
                {
                    lotNumber = value.Substring(2, startindex17 - 2);
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

            if (startindex17 == 0)
            {
                if (length >= startindex17 + 8)
                {
                    if (value.StartsWith("17"))
                    {
                        expirationDate = value.Substring(2, 6);
                    }
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
            if (expirationDate != null && expirationDate != "")
            {
                DateTime expResult = DateTime.ParseExact(20 + expirationDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                expirationDate = expResult.ToString("yyyy-MM-dd");
            }
            ScanData sd = new ScanData(expirationDate, lotNumber, gtin, serialNumber);
            List<ScanData> lstSD = new List<ScanData>(ScanDisplayVM.Instance.LstScanData);
            lstSD.Add(sd);
            DispatchService.Invoke(() =>
            {
                ScanDisplayVM.Instance.LstScanData = new System.Collections.ObjectModel.ObservableCollection<ScanData>(lstSD); ;
            });
          
        }

     
        // create a ManualResetEvent
       // static readonly ManualResetEvent dataReceivedtemp = new ManualResetEvent(false);



        // wait for the data to be received or for 30 seconds to elapse
        //readonly bool dataReceivedOrTimeout = dataReceivedtemp.WaitOne(30000);


       
        static void OnDataReceived1(object sender, SerialDataReceivedEventArgs e)
        {
            //dataReceivedtemp.Set();
            // read the data from the serial port buffer
            SerialPort scannerPort = (SerialPort)sender;

            scantext = scannerPort.ReadExisting();


            //byte[] buffer = new byte[scannerPort.BytesToRead];
            //scannerPort.Read(buffer, 0, buffer.Length);
            //scantext = Encoding.ASCII.GetString(buffer);

        }

        static void OnDataReceivedConti(object sender, SerialDataReceivedEventArgs e)
        {
            // read the data from the serial port buffer
            SerialPort scannerPort = (SerialPort)sender;

            scantext = scannerPort.ReadExisting();

            ContinousDecodeString(scantext);
            //byte[] buffer = new byte[scannerPort.BytesToRead];
            //scannerPort.Read(buffer, 0, buffer.Length);
            //scantext = Encoding.ASCII.GetString(buffer);

        }

        private StringBuilder _buffer = new StringBuilder();

        private void OnDataReceived5(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort scannerPort = (SerialPort)sender;
            byte[] buffer = new byte[scannerPort.BytesToRead];
            scannerPort.Read(buffer, 0, buffer.Length);
            _buffer.Append(Encoding.ASCII.GetString(buffer));

            if (_buffer.ToString().Length > 14)
            {
                scantext = _buffer.ToString().Trim();
                _buffer.Clear();
                // Do something with the received data
            }
        }

        static void OnDataReceived2(object sender, SerialDataReceivedEventArgs e)
        {
            //dataReceivedtemp.Set();
            // read the data from the serial port buffer
            SerialPort scannerPort = (SerialPort)sender;
            scantext = scannerPort.ReadExisting();

        }

        static void OnDataReceived3(object sender, SerialDataReceivedEventArgs e)
        {
            //dataReceivedtemp.Set();
            // read the data from the serial port buffer
            SerialPort scannerPort = (SerialPort)sender;
            scantext = scannerPort.ReadExisting();

        }

        static void OnDataReceived4(object sender, SerialDataReceivedEventArgs e)
        {
            //dataReceivedtemp.Set();
            // read the data from the serial port buffer
            SerialPort scannerPort = (SerialPort)sender;
            scantext = scannerPort.ReadExisting();

        }
    }

    public static class DispatchService
    {
        public static void Invoke(Action action)
        {
            Dispatcher dispatch = System.Windows.Application.Current.Dispatcher;
            if(dispatch == null || dispatch.CheckAccess())
            {
                action();

            }
            else
            {
                dispatch.Invoke(action);
            }
        }
    }
}
