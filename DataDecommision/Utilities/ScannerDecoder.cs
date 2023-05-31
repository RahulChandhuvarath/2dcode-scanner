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
using System.Windows;
using System.Windows.Media;

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
        public static (string, string, string, string) ScanAndDecode()
        {
            scantext = "";
            // find the first available serial port that has a barcode scanner attached
            SerialPort scannerPort = FindBarcodeScanner(userSelectedPort);
            if (scannerPort != null)
            {
                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(1000);
                    if (scantext != "" && scantext.Length > 32)
                    {
                        //scannerPort.Close();
                        return DecodeString(scantext);

                    }

                }

            }
            return ("", "", "", "");
        }


        private static SerialPort SerialClose = null;
        /// <summary>
        /// Open all the serial ports and register the event
        /// </summary>
        /// <returns></returns>
        public static SerialPort FindBarcodeScanner(string port, int type = 0)
        {
            if (SerialClose != null)
            {
                if (type == 1)
                {
                    SerialClose.DataReceived -= OnDataReceivedConti;

                }
                else if (type == 2)
                {
                    SerialClose.DataReceived -= OnDataReceivedCheckConti;
                }
                else
                {
                    SerialClose.DataReceived -= OnDataReceived1;
                }


                SerialClose.Close();
            }

            // get a list of all available serial ports
            List<string> portNames = new List<string>
            {
                port
            };

            SerialPort scannerPort = null;
            // configure the serial port settings for the scanner

            if (portNames.Count() >= 1)
            {
                scannerPort = new SerialPort(portNames[0], 9600, Parity.None, 8, StopBits.One);
                scannerPort.Handshake = Handshake.None;
                //scannerPort.Encoding = Encoding.UTF8;
                try
                {
                    scannerPort.Open();
                    SerialClose = scannerPort;
                    if (type == 1)
                    {
                        //Thread thread = new Thread(() =>
                        //{
                        //    Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
                        scannerPort.DataReceived += new SerialDataReceivedEventHandler(OnDataReceivedConti);
                        //Thread.Sleep(50);
                        //});
                        //thread.Start();
                    }
                    else if (type == 2)
                    {
                        scannerPort.DataReceived += new SerialDataReceivedEventHandler(OnDataReceivedCheckConti);
                    }
                    else
                    {
                        scantext = "";
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

            string pattern = "[^a-zA-Z0-9-]+";
            string value = Regex.Replace(inputString, pattern, "");

            string gtin = null, serialNumber = null, expirationDate = null, lotNumber = null;

            try
            {

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

                        int index = value.IndexOf("17", startindex17 + 2);
                        if (startindex17 == index)
                        {
                            startindex17 = 0;
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
                                string temp = value.Substring(startindex17 + 8, 2);
                                if (temp == "21" || temp == "10")
                                {
                                    expirationDate = value.Substring(startindex17 + 2, 6);
                                    break;
                                }
                            }
                            else if (length >= startindex17 + 8)
                            {
                                expirationDate = value.Substring(startindex17 + 2, 6);
                                break;
                            }
                            else
                            {
                                startindex17 = 0;
                                break;
                            }
                        }

                    } while (startindex17 >= 0);

                }
                if (startindex17 != 0)
                {

                    if (length > startindex17 + 10)  ///scenario 03,21,17,10  or 03,10,17,21
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
                    else if (length == startindex17 + 8) ///scenario 03,21,10,17  or 03,10,21,17
                    {
                        string[] escapeSequences = { "\\u001d"};
                        string updatedString = inputString.Substring(16);
                        updatedString = Regex.Replace(updatedString, @"\u001d", ",");
                        string[] output = updatedString.Split(',');

                        foreach (var item in output)
                        {
                            if (item.StartsWith("21"))
                            {
                                serialNumber = item.Substring(2);
                            }
                            if (item.StartsWith("10"))
                            {
                                lotNumber = item.Substring(2);
                            }
                        }  
                       
                    }
                    if (value.StartsWith("21"))
                    {
                        int indexlotNumber = startindex17 - 2;
                        if (lotNumber == null)
                        {
                            indexlotNumber = value.IndexOf("10", 2, startindex17);
                            lotNumber = value.Substring(indexlotNumber + 2, startindex17 - indexlotNumber - 2);
                            indexlotNumber = indexlotNumber - 2;
                        }
                        if (serialNumber == null)
                            serialNumber = value.Substring(2, indexlotNumber);
                    }
                    else if (value.StartsWith("10"))
                    {
                        int indexSerialNumber = startindex17 - 2;
                        if (serialNumber == null)
                        {
                            indexSerialNumber = value.IndexOf("21", 2, startindex17);
                            serialNumber = value.Substring(indexSerialNumber + 2, startindex17 - indexSerialNumber - 2);
                            indexSerialNumber = indexSerialNumber - 2;
                        }
                        if (lotNumber == null)
                            lotNumber = value.Substring(2, indexSerialNumber);

                    }

                }

                if (value.StartsWith("17")) ///scenario 03,17, 21,10  or 03,17, 10,21
                {
                    if (length > startindex17 + 10)
                    {

                        expirationDate = value.Substring(2, 6);
                        string[] escapeSequences = { "\\u001d" };
                        string updatedString = inputString.Substring(24);
                        updatedString = Regex.Replace(updatedString, @"\u001d", ",");
                        string[] output = updatedString.Split(',');

                        foreach (var item in output)
                        {
                            if (item.StartsWith("21"))
                            {
                                serialNumber = item.Substring(2);
                            }
                            if (item.StartsWith("10"))
                            {
                                lotNumber = item.Substring(2);
                            }
                        }

                    }
                }

            }
            catch
            {
                //MessageBox.Show("Error in Decode.\nInput String : " + inputString, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            string gtin = null, serialNumber = null, expirationDate = null, lotNumber = null;
            (gtin, lotNumber, expirationDate, serialNumber) = DecodeString(inputString);

            if (expirationDate != null && expirationDate != "")
            {
                DateTime expResult = DateTime.ParseExact(20 + expirationDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                expirationDate = expResult.ToString("yyyy-MM-dd");
            }
            ScanData sd = new ScanData(expirationDate, lotNumber, gtin, serialNumber);
            List<ScanData> lstSD = new List<ScanData>(ScanDisplayVM.Instance.LstScanData);

            if (lstSD.Count() == Convert.ToInt32(ScanDisplayVM.Instance.TextTotalBottle))
                return;

            if (sd.Expdate != DecomData.BulkExp || sd.LotNumber != DecomData.BulkLot || sd.GTIN != DecomData.BulkGtin)
            {
                return;
            }


            if (!lstSD.Contains(sd))
                lstSD.Add(sd);
            DispatchService.Invoke(() =>
            {
                ScanDisplayVM.Instance.LstScanData = new System.Collections.ObjectModel.ObservableCollection<ScanData>(lstSD); ;
            });

        }


        static void OnDataReceived1(object sender, SerialDataReceivedEventArgs e)
        {
            //dataReceivedtemp.Set();
            // read the data from the serial port buffer
            SerialPort scannerPort = (SerialPort)sender;

            //scantext = scannerPort.ReadExisting();
            byte[] buffer = new byte[scannerPort.BytesToRead];
            scannerPort.Read(buffer, 0, buffer.Length);
            string data = Encoding.ASCII.GetString(buffer);
            scantext += data;
            if (scantext.Length > 32)
            {
                Thread.Sleep(2);

            }

        }

        private static string receivedStringContinous = "";
        static void OnDataReceivedConti(object sender, SerialDataReceivedEventArgs e)
        {
            // read the data from the serial port buffer
            SerialPort scannerPort = (SerialPort)sender;

            //scantext = scannerPort.ReadExisting();
            byte[] buffer = new byte[scannerPort.BytesToRead];
            scannerPort.Read(buffer, 0, buffer.Length);
            string data = Encoding.ASCII.GetString(buffer);
            receivedStringContinous += data;

            if (receivedStringContinous.StartsWith("\u0002") && receivedStringContinous.Contains("\u0003"))
            {
                string input = receivedStringContinous;
                receivedStringContinous = "";
                ContinousDecodeString(input);
            }
            else if (!receivedStringContinous.StartsWith("\u0002") && receivedStringContinous.Length > 32)
            {
                //Thread.Sleep(2);
                string input = receivedStringContinous;
                receivedStringContinous = "";
                ContinousDecodeString(input);
            }
            else
            {
                Thread.Sleep(1);
            }


        }


        private static string receivedStringCheck = "";
        static void OnDataReceivedCheckConti(object sender, SerialDataReceivedEventArgs e)
        {
            // read the data from the serial port buffer
            SerialPort scannerPort = (SerialPort)sender;

            //scantext = scannerPort.ReadExisting();
            byte[] buffer = new byte[scannerPort.BytesToRead];
            scannerPort.Read(buffer, 0, buffer.Length);
            string data = Encoding.ASCII.GetString(buffer);
            receivedStringCheck += data;

            if (receivedStringCheck.StartsWith("\u0002") && receivedStringCheck.Contains("\u0003"))
            {
                string input = receivedStringCheck;
                receivedStringCheck = "";
                ContinousCheckString(input);
            }
            else if (!receivedStringCheck.StartsWith("\u0002") && receivedStringCheck.Length > 32)
            {
                //Thread.Sleep(2);
                string input = receivedStringCheck;
                receivedStringCheck = "";
                ContinousCheckString(input);
            }
            else
            {
                Thread.Sleep(1);
            }

        }

        private static void ContinousCheckString(string inputString)
        {
            string pattern = "[^a-zA-Z0-9-]+";
            inputString = Regex.Replace(inputString, pattern, "");
            if (CheckStringVM.Instance.TextString == null || CheckStringVM.Instance.TextString == "")
            {
                CheckStringVM.Instance.TextString = inputString;
                CheckStringVM.Instance.TextLength = inputString.Length.ToString();
                return;
            }

            if ((inputString.Substring(0, 3) == CheckStringVM.Instance.TextString.Substring(0, 3))
                && inputString.Length == CheckStringVM.Instance.TextString.Length)
            {
                CheckStringVM.Instance.TextMatching = (Convert.ToInt32(CheckStringVM.Instance.TextMatching) + 1).ToString();
            }
            else
            {
                CheckStringVM.Instance.TextNotMatching = (Convert.ToInt32(CheckStringVM.Instance.TextNotMatching) + 1).ToString();
            }

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
            if (dispatch == null || dispatch.CheckAccess())
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