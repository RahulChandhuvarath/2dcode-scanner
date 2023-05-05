using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDecommision
{
    public class DecomData
    {
        public static string Password { get; set; }

        public static string BulkExp { get; set; }

        public static string BulkLot { get; set; }

        public static string BulkGtin { get; set; }

        public static string BulkBottle { get; set; }

        public static string BulkCase { get; set; }

        public static string RepackExp { get; set; }

        public static string RepackLot { get; set; }

        public static string RepackGtin { get; set; }

        public static List<ScanData> ScandataDetails = new List<ScanData>();
    }

    public class ScanData : IEquatable<ScanData>
    {
        public ScanData(string strExp, string strLot, string strGtin, string strSerial)
        {
            Expdate = strExp;
            LotNumber = strLot;
            GTIN = strGtin;
            SerialNumber = strSerial;

        }

        public string Expdate { get; set; }
        public string LotNumber { get; set; }
        public string GTIN { get; set; }
        public string SerialNumber { get; set; }

        public bool Equals(ScanData other)
        {
            if (other == null) return false;
            return Expdate == other.Expdate &&
                   LotNumber == other.LotNumber &&
                   GTIN == other.GTIN &&
                   SerialNumber == other.SerialNumber;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            ScanData other = obj as ScanData;
            if (other == null) return false;
            else return Equals(other);
        }

        public override int GetHashCode()
        {
            return (Expdate + LotNumber + GTIN + SerialNumber).GetHashCode();
        }

    }
}
